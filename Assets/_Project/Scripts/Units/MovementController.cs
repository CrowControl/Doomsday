using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Scripts.Units
{
    [RequireComponent(typeof(Rigidbody2D)),
     RequireComponent(typeof(IMovementInputSource))]
    public class MovementController : MonoBehaviour
    {
        #region Editor Variables
        [SerializeField] private float _speed;          //Speed that this character moves at.
        #endregion

        #region Components
        private Rigidbody2D _rigidbody;                 //Handles Physics.
        private IMovementInputSource _movementSource;   //Provides movement input.
        #endregion

        #region Internal Variables
        private readonly List<float> _speedModifiers = new List<float>();
        private bool _stunned;
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
            
            _rigidbody.velocity = _movementSource.MovementDirection * DetermineSpeed();
        }

        #region Speed moification
        private float DetermineSpeed()
        {
            return _stunned ? 0 : _speedModifiers.Aggregate(_speed, (current, modifier) => current * modifier);
        }

        public void AddSpeedModifier(float modifier, float duration)
        {
            StartCoroutine(modifier <= 0 ? 
                StunModifierCoroutine(duration) : 
                ModifierCoroutine(modifier, duration));
        }

        private IEnumerator StunModifierCoroutine(float duration)
        {
            _stunned = true;
            yield return new WaitForSeconds(duration);
            _stunned = false;
        }

        private IEnumerator ModifierCoroutine(float modifier, float duration)
        {
            _speedModifiers.Add(modifier);
            yield return new WaitForSeconds(duration);
            _speedModifiers.Remove(modifier);
        }
        #endregion
    }

    public interface IMovementInputSource
    {
        Vector2 MovementDirection { get; } 
    }
}