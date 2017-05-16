
using Assets;
using Assets._Project.Scripts.Shooting;
using InControl;
using UnityEngine;

//todo refactor the mouse-keyboard input to a seperate component.
//todo implement InControl input management.
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
        UpdateAimingDirection();
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
        //movement input.
        _movementVector = new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")
        );


        //shooting.
        _shootButtonPressed = Input.GetMouseButton(0);
    }

    /// <summary>
    /// Update the viewing direction.
    /// </summary>
    private void UpdateAimingDirection()
    {
        //Calculate the vector to the mouse position from this character.
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 distance = mousePosition - (Vector2)transform.position;

        //Calculate the angle to the mouse position.
        _aimingDegree = MathHelper.Vector2Degree(distance);
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