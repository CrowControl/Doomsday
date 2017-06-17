using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayMusic : MonoBehaviour {

    [SerializeField] private string eventRef;
        
    private FMOD.Studio.EventInstance _eventInstance;

    private void Awake()
    {
        if (!string.IsNullOrEmpty(eventRef))
            _eventInstance = FMODUnity.RuntimeManager.CreateInstance(eventRef);

        _eventInstance.start();
    }

    private void OnDestroy()
    {
        _eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
