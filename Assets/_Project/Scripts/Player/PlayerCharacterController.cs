using InControl;
using UnityEngine;
using _Project.Scripts.Units;

namespace _Project.Scripts.Player
{
    public class PlayerCharacterController : CustomMonoBehaviour, IMovementInputSource, ICharacterAimSource
    {
        #region Properties
        public InputDevice Device { get; set; }         //Device that controls this character.
        public Vector2 MovementVector { get; private set; } //Vector used for movement.
        public float AimingDegree { get; private set; }     //Degree that this character is aiming at.
        public Vector2 SourcePosition { get { return transform.position; } } 
        #endregion
	
        // Update is called once per frame
        protected virtual void Update ()
        {
            //Movement.
            MovementVector = Device.LeftStick.Vector;

            //Aiming. (We read in a vector and convert it to a degree for ease of use.)
            AimingDegree = MathHelper.Vector2Degree(Device.RightStick.Vector);
        }
    }

    public interface ICharacterAimSource
    {
        float AimingDegree { get; }     //Degree that this character is aiming at.
        Vector2 SourcePosition { get; }
    }
}