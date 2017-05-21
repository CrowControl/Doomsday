using System.Collections;
using System.Collections.Generic;
using Assets;
using InControl;
using UnityEngine;
using _Project.Scripts.Player;

public class CharacterController : MonoBehaviour, IMovementInputSource, ICharacterAimSource
{
    #region Properties
    public InputDevice Controller { get; set; }
    public Vector2 MovementVector { get; private set; }
    public float AimingDegree { get; private set; }
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
    float AimingDegree { get; }
}
