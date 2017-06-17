using UnityEngine;
using _Project.Scripts.General;

namespace _Project.Scripts.Units.Spawners
{
    public class Spawner : CustomMonoBehaviour
    {
        [SerializeField] private GameObject _spawnPrefab;
        [SerializeField] private bool _destroyOnSpawn;

        /// <summary>
        /// Spawns the spawn prefab of this spawner.
        /// </summary>
        /// <param name="parent">If this isn't null, used to set the spawned objects parent.</param>
        /// <param name="position">Position to spawn the object at.</param>
        /// <param name="rotation">Rotation to spawn the object at.</param>
        protected void Spawn(Transform parent, Vector3 position, Quaternion rotation)
        {
            //Spawn.
            GameObject spawnedObject = Instantiate(_spawnPrefab, position, rotation);
            
            //Set parent if not null.
            if (parent != null)
            {
                //We need an intermediate object because Unity messes with scale and rotation when trying to set a parent.
                GameObject empty = new GameObject {name = "Spawn Container"};
                empty.transform.SetParent(parent);

                spawnedObject.transform.SetParent(empty.transform);
            }

            //Destroy if wanted.
            if (_destroyOnSpawn)
                Destroy(gameObject);
        }
    }
}
