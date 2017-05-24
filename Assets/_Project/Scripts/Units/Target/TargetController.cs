using UnityEngine;

namespace _Project.Scripts.Units.Target
{
    [RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(HealthController))]
    public class TargetController : MonoBehaviour
    {
        #region Editor Variables
        [SerializeField] private float _speed;

        //Range bounds.
        [SerializeField] private float _minRange;
        [SerializeField] private float _maxRange;

        #endregion

        #region Components
        private Rigidbody2D _rigidbody;
        #endregion

        #region Internal Variables
        private Vector2 _direction = Vector2.right;

        private Vector2 _previousPosition;
        private float _distanceTraveled;

        private float _range;
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
        #endregion

        #region Unity Methods (Messages)

        /// <summary>
        /// Called once when the object starts up for the first time.
        /// </summary>
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            GetComponent<HealthController>().OnDeath += OnDeath;
        }

        /// <summary>
        /// Called every tick of the game. Makes the target move a specified distance before inverting direction.
        /// </summary>
        private void Update()
        {
            Vector2 position = transform.position;
            _distanceTraveled += Vector2.Distance(_previousPosition, position);

            if (_distanceTraveled > _range)
                InvertDirection();

            _previousPosition = position;
        }

        /// <summary>
        /// Called when the target collides with another object. Makes it change direction.
        /// </summary>
        /// <param name="collision">The collision that occurs.s</param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            InvertDirection();
        }
        #endregion 
        
        /// <summary>
        /// Makes this target start moving.
        /// </summary>
        public void StartMoving()
        {
            _rigidbody.velocity = _direction * _speed;
        }

        /// <summary>
        /// Makes the target change it's moving direction to the opposite direction.
        /// </summary>
        private void InvertDirection()
        {
            //Do into the opposite direction.
            _direction *= -1;
            _rigidbody.velocity = _direction * _speed;

            //Choose random distance in that direction before changing again.
            _distanceTraveled = 0;
            _range = Random.Range(_minRange, _maxRange);
        }

        /// <summary>
        /// Called when this target dies, destroys the entire object.
        /// </summary>
        private void OnDeath()
        {
            Destroy(gameObject);
        }
    }
}
