using System;
using UnityEngine;

namespace _Project.Scripts.Units.Spawners
{
    public class OnCollisionSpawner : Spawner
    {

        [SerializeField] private SpawnParent _spawnParent = SpawnParent.None;
        [SerializeField] private int _maxSpawnCount = 1;

        private int _spawnCount;

        private enum SpawnParent
        {
            None,
            This,
            Other
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            //Use point of collision as spawn position.
            Vector3 collisionPoint = other.contacts[0].point;
            Spawn(other.collider.transform, collisionPoint, transform.rotation);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Spawn(other.transform, other.transform.position, transform.rotation);
        }

        private void MaybeSpawn(Transform otherTransform, Vector3 position)
        {
            //Don't spawn more then we want.
            if (_spawnCount > _maxSpawnCount) return;

            //Choose parent, spawn.
            Transform parent = ChooseParent(otherTransform);
            Spawn(parent, position, transform.rotation);
            _spawnCount++;
        }

        /// <summary>
        /// Chooses what we want to use as parent for he spawned object.
        /// </summary>
        /// <param name="other">The transform of the other collider in the collision. One of the options.</param>
        /// <returns>The transform that we'll use.</returns>
        private Transform ChooseParent(Transform other)
        {
            switch (_spawnParent)
            {
                case SpawnParent.This:
                    return transform;

                case SpawnParent.Other:
                    return other;

                default:
                    return null;
            }
        }
    }
}
