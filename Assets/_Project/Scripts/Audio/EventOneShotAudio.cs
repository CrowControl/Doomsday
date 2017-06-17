using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventOneShotAudio : MonoBehaviour {

    public void OneShotAudio(string eventRef)
    {
        if (!string.IsNullOrEmpty(eventRef))
            FMODUnity.RuntimeManager.PlayOneShot(eventRef, transform.position);
    }
}
