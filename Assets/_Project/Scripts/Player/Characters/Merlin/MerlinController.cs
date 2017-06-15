using UnityEngine;
using _Project.Scripts.Units.Abilities;
using _Project.Scripts.Units.Spawners;

namespace _Project.Scripts.Player.Characters.Merlin
{
    public class MerlinController : PlayerCharacterController
    {
        #region Components
        private AbilitySpawner _lightningSpawner;
        private SwordShooter _swordShooter;
        #endregion

        private void Awake()
        {
            Transform arm = transform.Find("Back Arm");
            _lightningSpawner = arm.GetComponentInChildren<AbilitySpawner>();

            _swordShooter = GetComponentInChildren<SwordShooter>();
        }

        protected override void HandleInput()
        {

            //Shoot excalibur.
            if (Device.RightTrigger.WasPressed && _swordShooter.IsLoaded)
                _swordShooter.Activate(this);

            //Shoot lightning.
            else if (Device.LeftTrigger.WasPressed)
                _lightningSpawner.Spawn(this);
        }
    }
}
