using System;
using System.Collections.Generic;
using InControl;
using UnityEngine;
using _Project.Scripts.UI.Character_Selection;

namespace _Project.Scripts.Player
{
    [RequireComponent(typeof(ControllerManager))]
    public class PlayerManager : MonoBehaviour
    {
        #region Editor Variables
        [SerializeField] private float _maxPlayerCount;     //Maximum amount of players allowed.
        [SerializeField] private Vector2 _spawnPosition;    //Position to spawn new players at.
        #endregion

        #region Other Objects
        private CharacterSelectUIManager _characterSelectUIManager; //The object that spawns Character select UI.
        #endregion
        
        #region Internal Variables
        private readonly List<PlayerCharacterController> _playerCharacters = new List<PlayerCharacterController>();
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

        private void OnCharacterSelected(PlayerCharacterController characterPrefab, InputDevice controller)
        {
            //Spawn the character, pass the controller reference.
            PlayerCharacterController playerCharacter = Instantiate(characterPrefab, _spawnPosition, Quaternion.identity);
            playerCharacter.Controller = controller;
            
            _playerCharacters.Add(playerCharacter);
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
