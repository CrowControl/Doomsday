using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStoppableAudio : MonoBehaviour {

    [SerializeField] private string eventRef;

    FMOD.Studio.EventInstance beamAudio;

    private void Awake()
    {
        beamAudio = FMODUnity.RuntimeManager.CreateInstance(eventRef);
        beamAudio.set3DAttributes(new FMOD.ATTRIBUTES_3D());
        beamAudio.start();
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(beamAudio, GetComponent<Transform>(), GetComponent<Rigidbody>());

    }

    private void OnDestroy()
    {
        beamAudio.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
