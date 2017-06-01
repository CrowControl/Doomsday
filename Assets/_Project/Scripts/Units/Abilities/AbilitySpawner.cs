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
        public event BehaviourEventHandler OnFinished;
        #endregion

        /// <summary>
        /// Shoots a projectile.
        /// </summary>
        /// <param name="aimsource">Source of aiming.</param>
        public Ability Spawn(ICharacterAimSource aimsource)
        {
            //Spawn a projectile.
            Ability ability = Instantiate(AbilityPrefab, transform);
            
            ability.OnNoLongerOccuppiesCaster += OnAbilityFinished;
            ability.Do(aimsource);

            return ability;
        }

        private void OnAbilityFinished()
        {
            if (OnFinished != null)
                OnFinished();
        }

        public Ability Spawn(ICharacterAimSource aimsource, Ability prefab)
        {
            AbilityPrefab = prefab;
            return Spawn(aimsource);
        }
    }
}
