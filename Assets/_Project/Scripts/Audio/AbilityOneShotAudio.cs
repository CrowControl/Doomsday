using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Project.Scripts.Units.Abilities;

[RequireComponent(typeof(Ability))]
public class AbilityOneShotAudio : MonoBehaviour
{
    #region Editor

    [SerializeField] private string _onActivateEventString;
    [SerializeField] private string _onDestroyEventString;

    #endregion

    private void Awake()
    {
        Ability ability = GetComponent<Ability>();

        ability.OnActivated += OnAbilityActivated;
        ability.OnDestroyed += OnAbilityDestroyed;
    }


    private void OnAbilityActivated()
    {
        PlayOneShot(_onActivateEventString);
    }

    private void OnAbilityDestroyed()
    {
        PlayOneShot(_onDestroyEventString);
    }


    private void PlayOneShot(string eventString)
    {
        if(!string.IsNullOrEmpty(eventString))
            FMODUnity.RuntimeManager.PlayOneShot(eventString, transform.position);
    }
}
