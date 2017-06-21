using UnityEngine;
using _Project.Scripts.General;

namespace _Project.Scripts.Units
{
    [RequireComponent(typeof(HealthController))]
    [RequireComponent(typeof(Animator))]
    internal class AnimatorEventHandler : CustomMonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            InitializeAnimationEvents();
        }

        private void InitializeAnimationEvents()
        {
            HealthController healthController = GetComponent<HealthController>();

            healthController.OnDeath += OnPlayerDeath;
            healthController.OnHit += OnPlayerHit;
        }

        private void OnPlayerDeath()
        {
            _animator.SetTrigger("Die");
        }

        private void OnPlayerHit(float damage)
        {
            _animator.SetTrigger("GetHit");
        }
    }
}
