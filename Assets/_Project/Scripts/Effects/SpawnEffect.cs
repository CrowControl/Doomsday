using UnityEngine;
using _Project.Scripts.Units;
using _Project.Scripts.Units.Abilities;

namespace _Project.Scripts.Effects
{
    class SpawnEffect : Effect
    {
        #region Editor Variables

        [SerializeField] private GameObject _spawnPrefab;

        #endregion

        /// <summary>
        /// Only works on objects with health at  the moment.
        /// </summary>
        /// <param name="gameObj"></param>
        /// <returns></returns>
        public override bool HasTargetComponent(GameObject gameObj)
        {
            return (gameObj.GetComponent<HealthController>() != null);
        }

        protected override void Apply(GameObject gameObj)
        {
            Instantiate(_spawnPrefab, gameObj.transform);
        }
    }
}
