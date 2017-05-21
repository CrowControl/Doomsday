using UnityEngine;

namespace _Project.Scripts.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MovementController : MonoBehaviour {

        #region Editor Variables
        [SerializeField] private float _speed;
        #endregion

        #region Components
        private Rigidbody2D _rigidbody;                   //Handles Physics.
        private IMovementInputSource _movementSource;   //Provides movement input.
        #endregion


        private void Awake ()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _movementSource = GetComponent<IMovementInputSource>();
        }
	
        // Update is called once per frame
        private void FixedUpdate ()
        {
            _rigidbody.velocity = _movementSource.MovementVector * _speed;
        }
    }

    public interface IMovementInputSource
    {
        Vector2 MovementVector { get; }
    }
}