using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOneShot : MonoBehaviour {

    [SerializeField] private string eventRef;

    private void Awake()
    {
        if (!string.IsNullOrEmpty(eventRef))
            FMODUnity.RuntimeManager.PlayOneShot(eventRef, transform.position);
    }
}
