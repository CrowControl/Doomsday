using InControl;
using UnityEngine;
using _Project.Scripts.Units;

namespace _Project.Scripts.Player
{
    [RequireComponent(typeof(Rigidbody2D)), 
     RequireComponent(typeof(Animator)), 
     RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(IShooter))]
    public class PlayerController : MonoBehaviour
    {
        #region Editor Variables

        [SerializeField] private float _speed;

        #endregion

        #region Components
        private Rigidbody2D _rigidbody;
        private Animator _animator;
        private SpriteRenderer _renderer;
        private IShooter _shooter;
        #endregion

        #region Internal variables
        //input.
        private Vector2 _movementVector;
        private bool _shootButtonPressed;

        private float _aimingDegree;
        #endregion

        #region Properties
        public InputDevice Controller { get; set; }
        #endregion

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _renderer = GetComponent<SpriteRenderer>();
            _shooter = GetComponent<IShooter>();
        }

        private void Update()
        {
            ReadInput();
            UpdateAnimation();
        }

        private void FixedUpdate()
        {
            //Movement.
            UpdateMovement();

            //shooting.
            if(_shootButtonPressed)
                _shooter.Shoot(_aimingDegree);
        }

        /// <summary>
        /// Reads in the player input.
        /// </summary>
        private void ReadInput()
        {
            //movement.
            _movementVector = Controller.LeftStick.Vector;

            //Aiming. (We read in a vector and convert it to a degree for ease of use.)
            _aimingDegree = MathHelper.Vector2Degree(Controller.RightStick.Vector);

            //shooting.
            _shootButtonPressed = Controller.RightTrigger.IsPressed;
        }

        /// <summary>
        /// Updates the animation.
        /// </summary>
        private void UpdateAnimation()
        {
            //Tell the animator we're moving if we have velocity.
            _animator.SetBool("IsMoving", _rigidbody.velocity != Vector2.zero);
            //We're front facing if we're aiming to the bottom half of the screen.
            _animator.SetBool("IsFrontFacing", _aimingDegree <= 0);
            //We want to flip if we're aiming to the right side of the screen.  
            _renderer.flipX = Mathf.Abs(_aimingDegree) <= 90;
        }

        /// <summary>
        /// Updates the player movement.
        /// </summary>
        private void UpdateMovement()
        {
            _rigidbody.velocity = _movementVector * _speed;
        }
    }
}