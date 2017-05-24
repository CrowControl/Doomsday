using UnityEngine;

namespace _Project.Scripts.Units
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MovementController : MonoBehaviour {

        #region Editor Variables
        [SerializeField] private float _speed;          //Speed that this character moves at.
        #endregion

        #region Components
        private Rigidbody2D _rigidbody;                 //Handles Physics.
        private IMovementInputSource _movementSource;   //Provides movement input.
        #endregion

        private void Awake ()
        {
            //Get components.
            _rigidbody = GetComponent<Rigidbody2D>();
            _movementSource = GetComponent<IMovementInputSource>();
        }
	
        // Update is called once per frame
        private void FixedUpdate ()
        {
            //Move by setting the velocity.
            _rigidbody.velocity = _movementSource.MovementVector * _speed;
        }
    }

    public interface IMovementInputSource
    {
        Vector2 MovementVector { get; } 
    }
}