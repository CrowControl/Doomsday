using UnityEngine;
using _Project.Scripts.General;

namespace _Project.Scripts.Units.Spawners
{
    public class Spawner : CustomMonoBehaviour
    {
        [SerializeField] private GameObject _spawnPrefab;
        [SerializeField] private bool _destroyOnSpawn;

        protected void Spawn()
        {
            //Spawn.
            Instantiate(_spawnPrefab, transform.position, transform.rotation);

            //Destroy if wanted.
            if (_destroyOnSpawn)
                Destroy(gameObject);
        }
    }
}
