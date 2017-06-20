using UnityEngine;
using _Project.Scripts.Player;

namespace _Project.Scripts.Units.Abilities
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleAbility : Ability
    {
        #region Components
        private ParticleSystem _particleSystem;
        #endregion
        
        private void Awake()
        {
            _particleSystem = GetComponentInChildren<ParticleSystem>();
            _particleSystem.Stop(true);
        }

        public override void Activate(ICharacterAimSource aimSource)
        {
            base.Activate(aimSource);

            RotateTowardAim(aimSource);
            _particleSystem.Play(true);
        }
    }
}
