using UnityEngine;
using _Project.Scripts.Units.Abilities;

namespace _Project.Scripts.Player.Characters
{
    [RequireComponent(typeof(AbilitySpawner))]
    public class MerlinController : PlayerCharacterController
    {
        #region Editor Variables
        [SerializeField] private float _cooldown;

        [SerializeField] private Projectile _excaliburPrefab;
        [SerializeField] private ShotBeamController _lightningPrefab;
        #endregion

        #region Components
        private AbilitySpawner _abilitySpawner;
        #endregion

        #region Internal Variables
        private bool _onCooldown;

        #endregion

        private void Awake()
        {
            _abilitySpawner = GetComponent<AbilitySpawner>();
        }

        protected override void Update()
        {
            base.Update();

            //Check for cooldown.
            if (_onCooldown)
                return;

            //Shoot excalibur.
            if (Device.RightTrigger.WasPressed)
                _abilitySpawner.Spawn(this, _excaliburPrefab);

            //Shoot lightning.
            else if (Device.LeftTrigger.WasPressed)
                _abilitySpawner.Spawn(this, _lightningPrefab);
        }

        #region Cooldown
        private void StartCooldown()
        {
            _onCooldown = true;
            Invoke("StopCooldown", _cooldown);
        }

        private void StopCooldown()
        {
            _onCooldown = false;
        }
        #endregion
    }
}
