using UnityEngine;
using _Project.Scripts.General;


namespace Assets._Project.Scripts.Units.Abilities
{ 
    [RequireComponent(typeof(ParticleSystem))]
    public class ChildCollider : CustomMonoBehaviour
    {
        public delegate void CollisionEventHandler(GameObject other);
        public event CollisionEventHandler OnCollisionEnter;

        private void OnTriggerEnter2D(Collider2D other)
        {
            CheckCollision(other.gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            CheckCollision(collision.gameObject);
        }

        private void OnParticleCollision(GameObject other)
        {
            CheckCollision(other);
        }
        
        private void CheckCollision(GameObject other)
        {
            if (OnCollisionEnter != null)
                OnCollisionEnter(other);
        }
    }
}
