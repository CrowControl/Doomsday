using System;
using FMOD;
using UnityEngine;
using FMOD.Studio;

namespace _Project.Scripts.Audio
{
    class AbilityStoppableAudio : AbilityAudio
    {
        #region editor
        [SerializeField] private string _eventInstancePath;
        [SerializeField] private STOP_MODE _stopMode;
        #endregion

        private EventInstance _eventInstance;

        protected override void Awake()
        {
            base.Awake();

            _eventInstance = FMODUnity.RuntimeManager.CreateInstance(_eventInstancePath);
            _eventInstance.set3DAttributes(new ATTRIBUTES_3D());
        }

        protected override void OnAbilityActivated()
        {
            _eventInstance.start();
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(_eventInstance,
                GetComponent<Transform>(), GetComponent<Rigidbody>());
        }

        protected override void OnAbilityDestroyed()
        {
            _eventInstance.stop(_stopMode);
        }
    }
}
