using UnityEngine;

namespace _Project.Scripts.Target
{
    public class TargetManager : MonoBehaviour
    {
        #region Editor variables
        [SerializeField] private float _respawnTime;
        [SerializeField] private TargetController _targetPrefab;
        [SerializeField] private float _targetSpeed;
        #endregion

        // Use this for initialization
        private void Start ()
        {
            SpawnTarget();
        }

        /// <summary>
        /// Spawns a target.
        /// </summary>
        private void SpawnTarget()
        {
            TargetController target = Instantiate(_targetPrefab, transform.position, Quaternion.identity);
            target.Speed = _targetSpeed;
            target.StartMoving();

            RegisterToTargetHealth(target);
        }

        /// <summary>
        /// Registers to the targets health Ondeath event. if the target doesn't have a health component, adds it.
        /// </summary>
        /// <param name="target"></param>
        private void RegisterToTargetHealth(TargetController target)
        {
            HealthController health = target.GetComponent<HealthController>() ?? AddHealthComponent(target.gameObject);
            health.OnDeath += OnTargetDeath;
        }

        /// <summary>
        /// Adds a health component to the target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>A reference to the health.</returns>
        private static HealthController AddHealthComponent(GameObject target)
        {
            HealthController health = target.gameObject.AddComponent<HealthController>();
            health.Hp = 5;
            return health;
        }

        /// <summary>
        /// Spawns a new target after a short time.
        /// </summary>
        private void OnTargetDeath()
        {
            Invoke("SpawnTarget", _respawnTime);
        }
    }
}
