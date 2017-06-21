using UnityEngine;
using _Project.Scripts.General;

namespace _Project.Scripts.Units.Spawners
{
    internal class OnDestroySpawner : CustomMonoBehaviour
    {
        [SerializeField] private GameObject _spawnPrefab;

        protected override void OnDestroy()
        {
            Instantiate(_spawnPrefab, transform.position, Quaternion.identity);
        }
    }
}
