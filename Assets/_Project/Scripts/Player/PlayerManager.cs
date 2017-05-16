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
        [SerializeField] private Vector2 _spawnPosition;
        #endregion

        #region Other Objects
        private CharacterSelectUIManager _characterSelectUIManager;
        #endregion
        
        #region Internal Variables
        private readonly List<PlayerController> _players = new List<PlayerController>();
        private int _newPlayerIndex;
        #endregion

        private void Start()
        {
            GetComponent<ControllerManager>().OnNewDeviceInUse += OnOnNewDeviceInUse;
            _characterSelectUIManager = FindObjectOfType<CharacterSelectUIManager>();
        }

        private void OnOnNewDeviceInUse(InputDevice controller)
        {
            if (_newPlayerIndex >= _maxPlayerCount) throw new TooManyPlayersException(controller);

            //Spawn UI, set the controller and await the selection.
            CharacterSelectUI charSelect = _characterSelectUIManager.SpawnCharacterSelectUI(_newPlayerIndex);
            charSelect.OnCharacterSelected += OnCharacterSelected;
            charSelect.Controller = controller;

            //Set the index for the next player.
            _newPlayerIndex++;
        }

        private void OnCharacterSelected(PlayerController characterPrefab, InputDevice controller)
        {
            //Spawn the character, pass the controller reference.
            PlayerController character = Instantiate(characterPrefab, _spawnPosition, Quaternion.identity);
            character.Controller = controller;

            _players.Add(character);
        }
    }

    public class TooManyPlayersException : Exception
    {
        public InputDevice AssociatedController { get; private set; }

        public TooManyPlayersException(InputDevice controller) 
            : base("Tried to add a new player when the maximum amount of players has already been reached.")
        {
            AssociatedController = controller;
        }
    }
}
