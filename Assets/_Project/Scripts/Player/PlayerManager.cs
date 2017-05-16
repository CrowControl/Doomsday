using System;
using System.Collections.Generic;
using InControl;
using UnityEngine;

namespace _Project.Scripts
{
    [RequireComponent(typeof(ControllerManager))]
    public class PlayerManager : MonoBehaviour
    {
        #region Editor Variables
        [SerializeField] private float _maxPlayerCount;
        #endregion

        #region Internal Variables
        private readonly List<PlayerController> _players = new List<PlayerController>();
        #endregion

        private void Start()
        {
            GetComponent<ControllerManager>().OnNewDeviceInUse += OnOnNewDeviceInUse;
        }

        private void OnOnNewDeviceInUse(InputDevice controller)
        {
            //todo spawn UI
            //todo subscribe to char selection event
            //todo spawn selected character
        }
    }
}
