using UnityEngine;

namespace _Project.Scripts.Units
{

    [RequireComponent(typeof(Rigidbody2D))]
    class Projectile : MonoBehaviour
    {
        #region Editor Variables
        [SerializeField] private float _speed;  //Speed the projectile travels at.
        [SerializeField] private float _range;  //Maximum distance the projectile can travel.
        [SerializeField] private float _damage; //Damage this projectile deals to units it hits.
        #endregion

        #region Components
        private Rigidbody2D _rigidbody;         //Handles physics behaviour.
        #endregion

        #region Internal Variables
        private Vector2 _previousPosition;      //This projectile's position from the privious update.
        private float _distanceTraveled;        //The total distance traveled by this projectile.
        #endregion

        #region Properties
        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public float Range
        {
            get { return _range; }
            set { _range = value; }
        }

        public float Damage
        {
            get { return _damage; }
            set { _damage = value; }
        }
        #endregion

        #region Unity Methods (Messages)
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _previousPosition = transform.position;
        }

        private void Update()
        {
            //Update distance traveled.
            Vector2 position = transform.position;
            _distanceTraveled += Vector2.Distance(_previousPosition, position);

            //Destroy if we've reached max. range.
            if(_distanceTraveled > Range)
                Destroy(gameObject);

            //save position for next update.
            _previousPosition = position;
        }
        private void OnCollisionEnter2D(Collision2D coll)
        {
            //Apply damage to what we collided with if it has health.
            HealthController other = coll.gameObject.GetComponent<HealthController>();
            if (other != null)
                other.GetHit(_damage);

            Destroy(gameObject);
        }
        #endregion

        /// <summary>
        /// Start moving.
        /// </summary>
        public void StartMoving()
        {
            //make the projectile move forward.
            float rotationAngle = transform.eulerAngles.z;                  //Sprite rotation is saved in the z-axis for reasons I don't really know.
            Vector2 direction = MathHelper.DegreeToVector2(rotationAngle);
            _rigidbody.velocity = direction * Speed;
        }

    }
}
