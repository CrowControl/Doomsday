using UnityEngine;

namespace _Project.Scripts.Units.Spawners
{
    public class OnCollisionSpawner : Spawner
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            Spawn();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Spawn();
        }
    }
}
