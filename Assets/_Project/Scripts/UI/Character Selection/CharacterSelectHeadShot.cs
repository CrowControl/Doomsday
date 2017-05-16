using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectHeadShot : MonoBehaviour
{
    #region Editor Variables
    [SerializeField] private PlayerController _associatedCharacterPrefab;
    #endregion

    #region Components
    private Outline _outline;
    #endregion

    #region Properties
    public PlayerController AssociatedCharacterPrefab
    {
        get { return _associatedCharacterPrefab; }
    }
    #endregion

    private void Awake()
    {
        _outline = GetComponent<Outline>();
        UnFocus();
    }

    public void Focus()
    {
        _outline.enabled = true;
    }

    public void UnFocus()
    {
        _outline.enabled = false;
    }
}
