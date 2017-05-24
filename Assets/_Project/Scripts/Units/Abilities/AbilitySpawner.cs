using UnityEngine;
using _Project.Scripts.Player;

namespace _Project.Scripts.Units.Abilities
{
    public class AbilitySpawner : MonoBehaviour
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

        /// <summary>
        /// Shoots a projectile.
        /// </summary>
        /// <param name="aimsource">Source of aiming.</param>
        public Ability Spawn(ICharacterAimSource aimsource)
        {
            //Spawn a projectile.
            Ability ability = Instantiate(AbilityPrefab, transform);
            ability.Do(aimsource);

            return ability;
        }
    }
}
