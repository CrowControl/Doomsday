using UnityEngine;
using _Project.Scripts.General;

namespace _Project.Scripts.Units.Spawners
{
    internal class OnDestroySpawner : CustomMonoBehaviour
    {
        [SerializeField] private GameObject _spawnPrefab;

        private bool _applicationIsQuitting;

        protected virtual void OnApplicationQuit()
        {
            _applicationIsQuitting = true;
        }

        protected override void OnDestroy()
        {
            if(!_applicationIsQuitting)
                Instantiate(_spawnPrefab, transform.position, Quaternion.identity);
        }
    }
}
