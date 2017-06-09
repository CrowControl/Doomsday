using System;
using InControl;
using UnityEngine;
using _Project.Scripts.Units.Abilities;

namespace _Project.Scripts.Player.Characters.Jean
{
    public class JeanController : PlayerCharacterController
    {
        #region Editor Variables
        [SerializeField] private Ability _rocketPrefab;
        [SerializeField] private Ability _shieldPrefab;

        [SerializeField] private float _maxShieldTime = 4;


        [SerializeField] private float _cooldown = 0.2f;
        #endregion

        #region Internal Variables
        private JeanState _state;
        #endregion

        protected override void Awake()
        {
            base.Awake();
            //Start in idle state.
            TransitionTo(new IdleState());
        }

        protected override void HandleInput()
        {
            //Update state.
            JeanState newState = _state.Update(Device, this);

            //check for new state.
            if(newState != null)
                TransitionTo(newState);
        }

        /// <summary>
        /// transitions to the new state.
        /// </summary>
        /// <param name="newState">State to transition to.</param>
        private void TransitionTo(JeanState newState)
        {
            if (_state != null)
                _state.Exit(this);

            _state = newState;
            _state.Enter(this);
        }

        #region State Mchine
        /// <summary>
        /// Base class for states in Jean's logic state machine.
        /// </summary>
        private abstract class JeanState
        {
            private float _enterTime;

            //Called when the state is entered.
            public virtual void Enter(JeanController jean)                                                    
            {
                _enterTime = Time.time;
            }    
            public abstract JeanState Update(InputDevice device, JeanController jean);  //Called every update.
            public virtual void Exit(JeanController jean) { }                           //Called when the state is exited.

            /// <summary>
            /// Time since this state was entered.
            /// </summary>
            protected float ElapsedTime { get { return Time.time - _enterTime; } }
        }

        /// <summary>
        /// State when Jean is not performing any abilities.
        /// </summary>
        private class IdleState : JeanState
        {
            public override JeanState Update(InputDevice device, JeanController jean)
            {
                if(device.RightTrigger.WasPressed)  //Shot pressed.
                    return new ShootingState();
                if (device.LeftTrigger.WasPressed)  //Shield button pressed.
                    return new ShieldState();

                return null;
            }
        }

        /// <summary>
        /// State when Jean is using her shield.
        /// </summary>
        private class ShieldState : JeanState
        {
            private Ability _shield;            //Reference to the shield.
            private float _enterTime;           //Time when this state was entered.
            private bool _shouldTransition;     //bool to check if we should transition.

            public override void Enter(JeanController jean)
            {
                base.Enter(jean);

                //Spawn shield.
                _shield = jean.AbilitySpawner.Spawn(jean, jean._shieldPrefab);
                _shield.OnDestroyed += OnShieldDestroyed;
            }

            public override JeanState Update(InputDevice device, JeanController jean)
            {
                //Start cooldown.
                if (device.LeftTrigger.WasReleased    ||    //Shield button released.
                    ElapsedTime > jean._maxShieldTime ||    //Max. shield use time reached.
                    _shouldTransition)                      //An event triggered the transition.
                    return new CooldownState(jean._cooldown);       

                return null;
            }

            public override void Exit(JeanController jean)
            {
                //Destroy the shield if it isn't yet.
                if (_shield == null) return;

                _shield.OnDestroyed -= OnShieldDestroyed;
                Destroy(_shield.gameObject);
            }

            /// <summary>
            /// Called when the shield is destroyed.
            /// </summary>
            private void OnShieldDestroyed()
            {
                _shouldTransition = true;
            }
        }

        /// <summary>
        /// State when Jean is shooting a rocket.
        /// </summary>
        private class ShootingState : JeanState
        {
            private bool _shouldTransition;

            public override void Enter(JeanController jean)
            {
                base.Enter(jean);
                
                //Spawn rocket.
                jean.AbilitySpawner.OnAbilitySpawnFinished += () => _shouldTransition = true;
                jean.AbilitySpawner.Spawn(jean, jean._rocketPrefab);
            }

            public override JeanState Update(InputDevice device, JeanController jean)
            {
                return _shouldTransition ? new CooldownState(jean._cooldown) : null;
            }
        }

        /// <summary>
        /// State when Jean is on cooldown after an ability was triggered.
        /// </summary>
        private class CooldownState : JeanState
        {
            private readonly float _duration;

            public CooldownState(float duration)
            {
                _duration = duration;
            }

            public override JeanState Update(InputDevice device, JeanController jean)
            {
                return ElapsedTime > _duration ? new IdleState() : null;
            }
           
        }
        #endregion
    }
}
