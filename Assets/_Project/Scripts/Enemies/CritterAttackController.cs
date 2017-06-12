using System.Collections.Generic;
using UnityEngine;
using _Project.Scripts.General;
using _Project.Scripts.Player;
using _Project.Scripts.Units;
using _Project.Scripts.Effects;

namespace _Project.Scripts.Enemies
{
    [RequireComponent(typeof(Animator))]
    class CritterAttackController : CustomMonoBehaviour, IEnemyAttackController
    {
        #region Variables

        #region Editor
        [SerializeField] private float _tryAttackDistance;
        [SerializeField] private float _hitAttackDistance;


        [SerializeField] private float _cooldown;
        [SerializeField] private List<Effect> _effects;
        #endregion
        
        #region Properties

        public float Distance { get { return _tryAttackDistance; } }
        public float Cooldown { get { return _cooldown; } }
        #endregion

        #region Components

        private Animator _animator;

        #endregion

        #region Events

        //Called when the attack is finished.
        public event CustomMonoBehaviour.BehaviourEventHandler OnFinished;

        #endregion

        #region Internal
        private PlayerCharacterController _targetPlayer;
        #endregion

        #endregion

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void StartAttack(PlayerCharacterController targetPlayer)
        {
            _targetPlayer = targetPlayer;
            _animator.SetTrigger("Attack");
        }

        /// <summary>
        /// Called from animation.
        /// Applies effects.
        /// </summary>
        private void ApplyEffects()
        {
            //Check if the critter is still close enough.
            float distance = Vector2.Distance(transform.position, _targetPlayer.transform.position);
            if (distance > _hitAttackDistance) return;

            //Try to apply all the effects.
            foreach (Effect effect in _effects)
                Effect.TryApplyEffect(effect, _targetPlayer.gameObject);
        }

        /// <summary>
        /// Called from animation.
        /// Finishes the attack.
        /// </summary>
        private void FinishAttack()
        {
            if (OnFinished != null)
                OnFinished();
        }
    }
}
