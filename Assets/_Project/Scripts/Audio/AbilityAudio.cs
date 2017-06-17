using UnityEngine;
using _Project.Scripts.General;
using _Project.Scripts.Units.Abilities;

namespace _Project.Scripts.Audio
{
    [RequireComponent(typeof(Ability))]
    public abstract class AbilityAudio : CustomMonoBehaviour 
    {
        protected virtual void Awake()
        {
            Ability ability = GetComponent<Ability>();

            ability.OnActivated += OnAbilityActivated;
            ability.OnDestroyed += OnAbilityDestroyed;
        }

        protected abstract void OnAbilityActivated();

        protected abstract void OnAbilityDestroyed();

    }
}
