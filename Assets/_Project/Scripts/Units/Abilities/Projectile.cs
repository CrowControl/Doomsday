using UnityEngine;
using _Project.Scripts.General;
using _Project.Scripts.Player;

namespace _Project.Scripts.Units.Abilities
{

    [RequireComponent(typeof(Rigidbody2D)),
     RequireComponent(typeof(MovementController))]
    class Projectile : Ability, IMovementInputSource
    {
        #region Editor Variables
        [SerializeField] private float _range;  //Maximum distance the projectile can travel.
        #endregion

        #region Internal Variables
        private Vector2 _previousPosition;      //This projectile's position from the privious update.
        private float _distanceTraveled;        //The total distance traveled by this projectile.
        #endregion

        #region Properties
        public Vector2 MovementDirection { get; private set; }
        #endregion

        #region Unity Methods (Messages)
        private void Awake()
        {
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
        public override void Activate(ICharacterAimSource aimSource)
        {
            RotateTowardAim(aimSource);

            //make the projectile move forward.
            float rotationAngle = transform.eulerAngles.z;                  //Sprite rotation is saved in the z-axis for reasons I don't really know.
            MovementDirection = MathHelper.DegreeToVector2(rotationAngle);

            Finish();
        }
    }
}
