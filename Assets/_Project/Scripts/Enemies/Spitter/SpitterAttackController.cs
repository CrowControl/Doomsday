using UnityEngine;
using _Project.Scripts.General;
using _Project.Scripts.Player;
using _Project.Scripts.Units.Abilities;
using _Project.Scripts.Units.Spawners;

namespace _Project.Scripts.Enemies
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AbilitySpawner))]
    public class SpitterAttackController : CustomMonoBehaviour, IEnemyAttackController
    {
        #region Variables

        #region Editor

        [SerializeField] private float _shootingDistance;
        [SerializeField] private float _cooldown;
        #endregion

        #region Events
        public event BehaviourEventHandler OnFinished;
        #endregion

        #region Properties
        /// <summary>
        /// Maximum distance this enemy will try it's attack from.
        /// </summary>
        public float Distance { get{return _shootingDistance;} }           
        /// <summary>
        /// Cooldown of this attack.
        /// </summary>
        public float Cooldown { get { return _cooldown; }}
        #endregion

        #region Components
        private Animator _animator;
        private AbilitySpawner _abilitySpawner;
        #endregion

        #region Internal 
        private PlayerCharacterController _targetPlayer;
        #endregion

        #endregion

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _abilitySpawner = GetComponent<AbilitySpawner>();
        }

        public void StartAttack(PlayerCharacterController targetPlayer)
        {
            _targetPlayer = targetPlayer;
            _animator.SetTrigger("Attack");
        }

        /// <summary>
        /// Called from Animation. Spawns a projectile.
        /// </summary>
        private void SpawnProjectile()
        {
            //Calculate direction.
            Direction shootingDirection = GetShootingDirection();

            //Spawn.
            _abilitySpawner.OnAbilitySpawnFinished += Finish;
            _abilitySpawner.Spawn(shootingDirection);
        }

        private Direction GetShootingDirection()
        {
            //Get direction vector.
            Vector2 directionVector = _targetPlayer.transform.position - transform.position;
            return new Direction(directionVector, transform.position);
        }

        private void Finish()
        {
            if (OnFinished != null)
                OnFinished();
        }
    }
}
