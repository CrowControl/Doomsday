using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using _Project.Scripts.Units;
using _Project.Scripts.Units.Abilities;

namespace _Project.Scripts.Effects
{
    class SpawnEffect : Effect
    {
        #region Editor Variables

        [SerializeField] private GameObject _spawnPrefab;
        [SerializeField] private int _maxInstancesOnOneTarget;

        #endregion

        /// <summary>
        /// Only works on objects with health at  the moment.
        /// </summary>
        /// <param name="gameObj"></param>
        /// <returns></returns>
        public override bool IsValidTarget(GameObject gameObj)
        {
            return (gameObj.GetComponent<HealthController>() != null) &&
                   !TargetHasMaxInstances(gameObj);
        }

        /// <summary>
        /// Checks if the given target already has the maximum amount of instances of this effect with this name.
        /// </summary>
        /// <param name="target">The target to check</param>
        /// <returns></returns>
        private bool TargetHasMaxInstances(GameObject target)
        {
            //Get all other instances of this effect on the target.
            IEnumerable<SpawnEffect> effectInstances = target.GetComponentsInChildren<SpawnEffect>()
                .Where(c => c.gameObject.name == gameObject.name);

            return effectInstances.Count() >= _maxInstancesOnOneTarget;
        }

        protected override void Apply(GameObject gameObj)
        {
            Instantiate(_spawnPrefab, gameObj.transform);
        }
    }
}
