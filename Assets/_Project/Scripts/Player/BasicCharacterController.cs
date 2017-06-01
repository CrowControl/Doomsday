using UnityEngine;
using _Project.Scripts.Units.Abilities;

namespace _Project.Scripts.Player
{
    class BasicCharacterController : PlayerCharacterController
    {
        #region Editor Variables
        [SerializeField] private float _cooldown;

        [SerializeField] private Ability _rightTriggerAbilityPrefab;
        [SerializeField] private Ability _leftTriggerAbilityPrefab;
        #endregion 

        #region Internal Variables
        private bool _onCooldown;
        #endregion

        protected override void HandleInput()
        {
            //Check for cooldown.
            if (_onCooldown)
                return;

            //Shoot excalibur.
            if (Device.RightTrigger.WasPressed)
                AbilitySpawner.Spawn(this, _rightTriggerAbilityPrefab);

            //Shoot lightning.
            else if (Device.LeftTrigger.WasPressed)
                AbilitySpawner.Spawn(this, _leftTriggerAbilityPrefab);
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
