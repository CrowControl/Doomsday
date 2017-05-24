using UnityEngine;
using _Project.Scripts.Player;

namespace _Project.Scripts.Units.Abilities
{

    [RequireComponent(typeof(Rigidbody2D))]
    class Projectile : Ability
    {
        #region Editor Variables
        [SerializeField] private float _speed;  //Speed the projectile travels at.
        [SerializeField] private float _range;  //Maximum distance the projectile can travel.
        #endregion

        #region Components
        private Rigidbody2D _rigidbody;         //Handles physics behaviour.
        #endregion

        #region Internal Variables
        private Vector2 _previousPosition;      //This projectile's position from the privious update.
        private float _distanceTraveled;        //The total distance traveled by this projectile.
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
            if(_distanceTraveled > _range)
                Destroy(gameObject);

            //save position for next update.
            _previousPosition = position;
        }
        #endregion

        /// <summary>
        /// Start moving.
        /// </summary>
        public override void Do(ICharacterAimSource aimSource)
        {
            transform.rotation = Quaternion.AngleAxis(aimSource.AimingDegree, Vector3.forward);

            //make the projectile move forward.
            float rotationAngle = transform.eulerAngles.z;                  //Sprite rotation is saved in the z-axis for reasons I don't really know.
            Vector2 direction = MathHelper.DegreeToVector2(rotationAngle);
            _rigidbody.velocity = direction * _speed;
        }
    }
}
