using System;
using UnityEngine;
using _Project.Scripts.General;
using _Project.Scripts.Player;

namespace _Project.Scripts.Units.Abilities
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
            //Spawn a projectile.
            Ability ability = Instantiate(AbilityPrefab, transform.position, transform.rotation);
            
            ability.OnNoLongerOccuppiesCaster += OnAbilityNoLongerOccuppiesCaster;
            ability.Do(aimsource);

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
