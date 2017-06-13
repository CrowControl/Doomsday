using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerTools;
using _Project.Scripts.Player;

public class ArmsController : MonoBehaviour
{
    #region Components
    private SpriteAnimNodes _animationNodes;
    private ICharacterAimSource _aimSource;

    //transforms.
    private Transform _shootingArmTransform;
    private Transform _otherArmTransform;

    //Renderers
    private SpriteRenderer _shootingArmRenderer;
    private SpriteRenderer _otherArmRenderer;
    #endregion

    private void Awake()
    {
        _animationNodes = GetComponent<SpriteAnimNodes>();
        _aimSource = GetComponent<ICharacterAimSource>();

        //Get arm transforms.
        _shootingArmTransform = transform.Find("Shooting Arm");
        _otherArmTransform = transform.Find("Other Arm");

        //Get arm renderers.
        _shootingArmRenderer = _shootingArmTransform.GetComponent<SpriteRenderer>();
        _otherArmRenderer = _otherArmTransform.GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        _otherArmTransform.position = _animationNodes.GetPosition(0);
        _shootingArmTransform.position = _animationNodes.GetPosition(1);

        UpdateShootingArmRotation();
    }

    private void UpdateShootingArmRotation()
    {
        float degree = _aimSource.AimingDegree;
        _shootingArmTransform.rotation = Quaternion.AngleAxis(degree, Vector3.forward);
        
        //The shooting arm sprite needs to be flipped if we're aiming at the right side of the screen. 
        //If your wondering why, try commenting the line below.
        _shootingArmRenderer.flipY = Mathf.Abs(degree) < 90;
    }
}
