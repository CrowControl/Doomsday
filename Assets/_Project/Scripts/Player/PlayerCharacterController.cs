using InControl;
using UnityEngine;
using _Project.Scripts.General;
using _Project.Scripts.Units;
using _Project.Scripts.Units.Abilities;

namespace _Project.Scripts.Player
{
    public abstract class PlayerCharacterController : CustomMonoBehaviour, IMovementInputSource, ICharacterAimSource
    {
        #region Properties
        public InputDevice Device { get; set; }         //Device that controls this character.

        #region IMovementInputSource
        public Vector2 MovementDirection { get; private set; } //Vector used for movement.
        #endregion

        #region ICharacterAimSource
        public float AimingDegree { get; private set; }     //Degree that this character is aiming at.
        public Vector2 SourcePosition { get { return transform.position; } }
        #endregion

        #endregion
	
        // Update is called once per frame
        private void Update ()
        {
            //Movement.
            MovementDirection = Device.LeftStick.Vector;

            //Aiming. (We read in a vector and convert it to a degree for ease of use.)
            AimingDegree = MathHelper.Vector2Degree(Device.RightStick.Vector);

            //Let the implementing class handle input.
            HandleInput();
        }

        /// <summary>
        /// Let the character class handle input.
        /// </summary>
        protected abstract void HandleInput();
    }
}