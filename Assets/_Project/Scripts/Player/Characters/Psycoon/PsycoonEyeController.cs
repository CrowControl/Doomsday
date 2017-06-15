using UnityEngine;
using _Project.Scripts.General;
using _Project.Scripts.Units.Abilities;
using _Project.Scripts.Units.Spawners;

namespace _Project.Scripts.Player.Characters.Psycoon
{
    class PsycoonEyeController : CustomMonoBehaviour
    {
        #region Properties

        public AbilitySpawner AbilitySpawner { get; private set; }

        #endregion

        private ICharacterAimSource _aimsource;

        private void Awake()
        {
            _aimsource = GetComponentInParent<ICharacterAimSource>();
            AbilitySpawner = GetComponentInChildren<AbilitySpawner>();

            AbilitySpawner.UseCustomAimSource = true;
            AbilitySpawner.CustomAimSource = new ProxyAimSource(_aimsource, transform);
        }

        private void Update()
        {
            //Rotate arm to where the player is aiming.
            float degree = _aimsource.AimingDegree;
            transform.rotation = Quaternion.AngleAxis(degree, Vector3.forward);
        }

        
    }
}
