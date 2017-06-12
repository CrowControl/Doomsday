using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerTools;

[RequireComponent(typeof(SpriteAnimNodes))]
public class EyeController : MonoBehaviour
{

    #region Components
    private SpriteAnimNodes _animNodes;
    private Transform _eyeTransform;
    #endregion

    private void Awake()
    {
        _animNodes = GetComponent<SpriteAnimNodes>();
        _eyeTransform = transform.Find("Eye");
    }

    private void LateUpdate()
    {
        _eyeTransform.position = _animNodes.GetPosition(0);
    }
}
