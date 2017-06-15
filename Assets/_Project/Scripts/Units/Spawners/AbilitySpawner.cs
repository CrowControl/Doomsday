using UnityEngine;
using _Project.Scripts.General;
using _Project.Scripts.Player;
using _Project.Scripts.Units.Abilities;

namespace _Project.Scripts.Units.Spawners
{
    public class AbilitySpawner : CustomMonoBehaviour
    {
        #region Editor Variables
        //projectile.
        [SerializeField] private Ability _abilityPrefab;

        #endregion

        #region Properties
        public Ability AbilityPrefab
        {
            get { return _abilityPrefab; }
            set { _abilityPrefab = value; }
        }

        public bool UseCustomAimSource { get; set; }
        public ICharacterAimSource CustomAimSource { get; set; }
        #endregion

        #region Events
        public event BehaviourEventHandler OnAbilitySpawnFinished;
        #endregion

        /// <summary>
        /// Shoots a projectile.
        /// </summary>
        /// <param name="aimsource">Source of aiming.</param>
        public Ability Spawn(ICharacterAimSource aimsource)
        {
            //Spawn the ability.
            Ability ability = AbilityPrefab.SpawnAsChild ? 
                Instantiate(AbilityPrefab, transform) :
                Instantiate(AbilityPrefab, transform.position, transform.rotation);
            
            //Activate the ability.
            ability.OnNoLongerOccuppiesCaster += OnAbilityNoLongerOccuppiesCaster;
            ICharacterAimSource aimingSource = UseCustomAimSource ? CustomAimSource : aimsource;
            ability.Activate(aimingSource);

            return ability;
        }

        private void OnAbilityNoLongerOccuppiesCaster()
        {
            if (OnAbilitySpawnFinished != null)
                OnAbilitySpawnFinished();
        }

        public Ability Spawn(ICharacterAimSource aimsource, Ability prefab)
        {
            AbilityPrefab = prefab;
            return Spawn(aimsource);
        }
    }
}
