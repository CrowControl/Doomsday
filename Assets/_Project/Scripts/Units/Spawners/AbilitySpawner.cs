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
        #endregion

        #region Events
        public event BehaviourEventHandler OnAbilitySpawnFinished;
        #endregion

        /// <summary>
        /// Shoots a projectile.
        /// </summary>
        /// <param name="aimSource">Source of aiming.</param>
        public Ability Spawn(ICharacterAimSource aimSource)
        {
            //Spawn the ability.
            Ability ability = AbilityPrefab.SpawnAsChild ? 
                Instantiate(AbilityPrefab, transform) :
                Instantiate(AbilityPrefab, transform.position, transform.rotation);
            
            //Activate the ability.
            ability.OnNoLongerOccuppiesCaster += OnAbilityNoLongerOccuppiesCaster;
            ability.Activate(aimSource);

            return ability;
        }

        #region Spawn overloads
        /// <summary>
        /// Spawns the ability.
        /// </summary>
        /// <param name="aimSource">The source of aiming.</param>
        /// <param name="sourceTransform">Transform to het the source position from.</param>
        /// <returns></returns>
        public Ability Spawn(ICharacterAimSource aimSource, Transform sourceTransform)
        {
            return Spawn(new ProxyAimSource(aimSource, sourceTransform));
        }

        /// <summary>
        /// Spawns the ability.
        /// </summary>
        /// <param name="aimSource">The source of aiming.</param>
        /// <param name="sourceTransform">Transform to het the source position from.</param>
        /// <param name="prefab">ability to spawn.</param>
        /// <returns></returns>
        public Ability Spawn(ICharacterAimSource aimSource, Ability prefab, Transform sourceTransform)
        {
            AbilityPrefab = prefab;
            return Spawn(new ProxyAimSource(aimSource, sourceTransform));
        }

        /// <summary>
        /// Spawns the ability.
        /// </summary>
        /// <param name="aimSource">The source of aiming.</param>
        /// <param name="prefab">ability to spawn.</param>
        /// <returns></returns>
        public Ability Spawn(ICharacterAimSource aimSource, Ability prefab, bool useSpawnerTransform = false)
        {
            if(useSpawnerTransform)
                return Spawn(aimSource, prefab, transform);

            AbilityPrefab = prefab;
            return Spawn(aimSource);
        }

        #endregion

        private void OnAbilityNoLongerOccuppiesCaster()
        {
            if (OnAbilitySpawnFinished != null)
                OnAbilitySpawnFinished();
        }
    }
}
