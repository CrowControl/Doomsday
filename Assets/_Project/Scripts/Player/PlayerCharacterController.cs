using InControl;
using UnityEngine;

namespace _Project.Scripts.Player
{
    public class PlayerCharacterController : MonoBehaviour, IMovementInputSource, ICharacterAimSource
    {
        #region Properties
        public InputDevice Controller { get; set; }         //Controller that controls this character.
        public Vector2 MovementVector { get; private set; } //Vector used for movement.
        public float AimingDegree { get; private set; }     //Degree that this character is aiming at.
        #endregion
	
        // Update is called once per frame
        protected virtual void Update ()
        {
            //Movement.
            MovementVector = Controller.LeftStick.Vector;

            //Aiming. (We read in a vector and convert it to a degree for ease of use.)
            AimingDegree = MathHelper.Vector2Degree(Controller.RightStick.Vector);
        }
    }

    public interface ICharacterAimSource
    {
        float AimingDegree { get; }     //Degree that this character is aiming at.
    }
}