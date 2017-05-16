using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectHeadShot : MonoBehaviour
{
    private Outline _outline;
    private void Awake()
    {
        _outline = GetComponent<Outline>();
        Unselect();
    }

    public void Select()
    {
        _outline.enabled = true;
    }

    public void Unselect()
    {
        _outline.enabled = false;
    }
}
