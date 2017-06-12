using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using _Project.Scripts.General;
using _Project.Scripts.Player;
using _Project.Scripts.Units;

namespace _Project.Scripts.Enemies
{
    [RequireComponent(typeof(IEnemyAttackController))]
    [RequireComponent(typeof(Animator))]
    public class EnemyStateMachine : CustomMonoBehaviour, IMovementInputSource, ICharacterAimSource
    {
        #region Variables

        #region Components

        private Animator _animator;
        private EnemyRange _noticeRange;
        private IEnemyAttackController _attackController;

        #endregion

        #region Properties

        #region IMovement Input Source Properties
        /// <summary>
        /// Direction that this character is moving in.
        /// </summary>
        public Vector2 MovementDirection { get; private set; }
        #endregion

        #region ICharacter Aim SOurce Properties
        /// <summary>
        /// Degree that this chaacter is aiming towards.
        /// </summary>
        public float AimingDegree
        {
            get
            {
                Vector2 direction = MovementDirection;
                direction.Normalize();
                return MathHelper.Vector2Degree(direction);
            }
        }

        /// <summary>
        /// Position of this Aiming source.
        /// </summary>
        public Vector2 SourcePosition { get { return transform.position; }}
        #endregion

        #endregion

        #region Internal

        private IEnemyState _state;

        #endregion

        #endregion

        #region Methods

        #region Awake
        private void Awake()
        {
            GetComponents();
            TransitionTo(new IdleState());
        }

        private void GetComponents()
        {
            _animator = GetComponent<Animator>();
            _noticeRange = GetComponentInChildren<EnemyRange>();
            _attackController = GetComponent<IEnemyAttackController>();
        }

        #endregion

        #region Update

        private void Update()
        {
            IEnemyState nextState = _state.Update(this);
            if (nextState != null)
                TransitionTo(nextState);
        }

        /// <summary>
        /// Transitions to the new state.
        /// </summary>
        /// <param name="newState">The new state.</param>
        private void TransitionTo(IEnemyState newState)
        {
            //Exit the old state.
            if(_state != null)
                _state.Exit(this);

            //Enter the new state.
            _state = newState;
            _state.Enter(this);
        }

        #endregion

        #endregion

        #region States

        /// <summary>
        /// Interface for the states used in the enemy logic state machine.
        /// </summary>
        private interface IEnemyState
        {
            /// <summary>
            /// Called when this state is first entered.
            /// </summary>
            /// <param name="enemy">Enemy instance this is the state of.</param>
            void Enter(EnemyStateMachine enemy);

            /// <summary>
            /// Called every update while this is the active state.
            /// </summary>
            /// <param name="enemy">Enemy instance this is the state of.</param>
            /// <returns>The next state if a transition is appropriatee, null otherwise.</returns>
            IEnemyState Update(EnemyStateMachine enemy);

            /// <summary>
            /// Called when this state is exited.
            /// </summary>
            /// <param name="enemy">Enemy instance this is the state of.</param>
            void Exit(EnemyStateMachine enemy);
        }

        /// <summary>
        /// State for  when the enemy is idling.
        /// </summary>
        private class IdleState : IEnemyState
        {
            /// <summary>
            /// reference that is assigned when a player enters the notice-range.
            /// </summary>
            private PlayerCharacterController _noticedPlayer;

            //Subsrcibe to the noticing event.
            public  void Enter(EnemyStateMachine enemy)
            {
                enemy._animator.SetTrigger("Idle");
                enemy._noticeRange.OnFirstPlayerEnteredRange += OnPlayerNoticed;
            }

            //Transition to moving state if we have noticed a player.
            public  IEnemyState Update(EnemyStateMachine enemy)
            {
                return _noticedPlayer != null ? new MovingState(_noticedPlayer) : null;
            }

            //unsubscribe from thhe noticng event.
            public  void Exit(EnemyStateMachine enemy)
            {
                enemy._noticeRange.OnFirstPlayerEnteredRange += OnPlayerNoticed;
            }

            /// <summary>
            /// Called when a player is noticed. 
            /// </summary>
            /// <param name="player">The player.</param>
            private void OnPlayerNoticed(PlayerCharacterController player)
            {
                _noticedPlayer = player;
            }
        }

        /// <summary>
        /// State for when the enemy is moving towards a player.
        /// </summary>
        private class MovingState : IEnemyState
        {
            private PlayerCharacterController _targetPlayer;

            /// <summary>
            /// We initiate our moving state with a target.
            /// </summary>
            /// <param name="player"></param>
            public MovingState(PlayerCharacterController player)
            {
                _targetPlayer = player;
            }

            //Subscribe to events.
            public void Enter(EnemyStateMachine enemy)
            {
                enemy._animator.ResetTrigger("Idle");
                enemy._animator.SetTrigger("Walk");

                enemy._noticeRange.OnLastPlayerExitedRange += OnNoPlayerInRange;
                enemy._noticeRange.OnNewNearestPlayer += OnNewNearestPlayer;
            }

            public  IEnemyState Update(EnemyStateMachine enemy)
            {
                //If no more players in range, back to idle.
                if(_targetPlayer == null)
                    return new IdleState();

                //If we're close enough to the player, attack.
                if(CloseEnoughToTarget(enemy))
                    return new AttackState(_targetPlayer);

                //Move towards the target.
                MoveTowardsTarget(enemy, _targetPlayer.transform);
                return null;
            }


            //Unsubscibe from events and stop moving.
            public void Exit(EnemyStateMachine enemy)
            {
                //unsubscribe from events.
                enemy._noticeRange.OnLastPlayerExitedRange -= OnNoPlayerInRange;
                enemy._noticeRange.OnNewNearestPlayer -= OnNewNearestPlayer;

                //Stop moving.
                enemy.MovementDirection = Vector2.zero;
            }

            private static void MoveTowardsTarget(EnemyStateMachine enemy, Transform targetPlayerTransform)
            {
                Vector3 direction = targetPlayerTransform.position - enemy.transform.position;
                direction.Normalize();

                enemy.MovementDirection = direction;
            }


            /// <summary>
            /// Checks if the enemy is close enough to it's target.
            /// </summary>
            /// <param name="enemy">The enemy.</param>
            /// <returns>True if close enough, false if not.</returns>
            private bool CloseEnoughToTarget(EnemyStateMachine enemy)
            {
                //Calculate distance to target.
                Vector2 position = enemy.transform.position;
                Vector2 playerPosition = _targetPlayer.transform.position;
                double distance = Vector2.Distance(position, playerPosition);

                return distance <= enemy._attackController.Distance;
            }

            #region Event Methods
            private void OnNoPlayerInRange(PlayerCharacterController player)
            {
                _targetPlayer = null;
            }

            private void OnNewNearestPlayer(PlayerCharacterController player)
            {
                _targetPlayer = player;
            }
            #endregion
        }

        /// <summary>
        /// State for when the player is attacking the player.
        /// </summary>
        private class AttackState : IEnemyState
        {
            //The target of the attack.
            private readonly PlayerCharacterController _targetPlayer; 
            
            //Set to true when the attack is finished.
            private bool _attackFinished;

            public AttackState(PlayerCharacterController targetPlayer)
            {
                _targetPlayer = targetPlayer;
            }

            //Attack, subscribe to attack finished event.
            public void Enter(EnemyStateMachine enemy)
            {
                enemy._attackController.OnFinished += OnAttackFinished;
                enemy._attackController.StartAttack(_targetPlayer);
            }
            
            //Transition to cooldown when attack is finished.
            public  IEnemyState Update(EnemyStateMachine enemy)
            {
                return _attackFinished ? new CooldownState() : null;
            }

            //Unsubscribe from attack finished event.
            public void Exit(EnemyStateMachine enemy)
            {
                enemy._attackController.OnFinished -= OnAttackFinished;
            }

            #region Event Methods
            /// <summary>
            /// Called when the attack is finished.
            /// </summary>
            private void OnAttackFinished()
            {
                _attackFinished = true;
            }

            #endregion
        }

        /// <summary>
        /// State for when the enemy is on cooldown after attacking the player.
        /// </summary>
        private class CooldownState : IEnemyState
        {
            private float _endTime;

            public void Enter(EnemyStateMachine enemy)
            {
                _endTime = Time.time + enemy._attackController.Cooldown;
            }

            public  IEnemyState Update(EnemyStateMachine enemy)
            {
                //Wait for cooldown to finish.
                if (!Finished) return null;

                EnemyRange range = enemy._noticeRange;
                //If there are still players in range, transition to moving.
                if (range.PlayersInRange > 0)
                    return new MovingState(range.NearestPlayer);
                
                //If ther are no players in range, transition to idle.
                return new IdleState();
            }


            public void Exit(EnemyStateMachine enemy) { }

            /// <summary>
            /// True if the cooldown is finished, false if not. 
            /// </summary>
            private bool Finished { get { return Time.time >= _endTime; } }
        }

        #endregion
    }
}