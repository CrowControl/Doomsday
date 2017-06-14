using UnityEngine;

namespace _Project.Scripts.Audio
{
    public class AbilityOneShotAudio : AbilityAudio
    {
        #region Editor

        [SerializeField] private string _onActivateEventString;
        [SerializeField] private string _onDestroyEventString;

        #endregion

        protected override void OnAbilityActivated()
        {
            PlayOneShot(_onActivateEventString);
        }

        protected override void OnAbilityDestroyed()
        {
            PlayOneShot(_onDestroyEventString);
        }


        private void PlayOneShot(string eventString)
        {
            if(!string.IsNullOrEmpty(eventString))
                FMODUnity.RuntimeManager.PlayOneShot(eventString, transform.position);
        }
    }
}
