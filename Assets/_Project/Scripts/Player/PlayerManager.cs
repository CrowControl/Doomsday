using System;
using System.Collections.Generic;
using InControl;
using UnityEngine;
using _Project.Scripts.UI.Character_Selection;

namespace _Project.Scripts.Player
{
    [RequireComponent(typeof(PlayerDeviceManager))]
    public class PlayerManager : MonoBehaviour
    {
        #region Editor Variables
        [SerializeField] private float _maxPlayerCount;     //Maximum amount of players allowed.
        #endregion

        #region Other Objects
        private CharacterSelectUIManager _characterSelectUIManager; //The object that spawns Character select UI.
        private Camera _camera;
        #endregion

        #region Properties
        public List<PlayerCharacterController> Players { get { return _playerCharacters; } }
        #endregion

        #region Internal Variables
        private readonly List<PlayerCharacterController> _playerCharacters = new List<PlayerCharacterController>();
        private int _newPlayerIndex;
        #endregion

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Start()
        {
            GetComponent<PlayerDeviceManager>().OnNewDeviceInUse += OnOnNewDeviceInUse;
            _characterSelectUIManager = FindObjectOfType<CharacterSelectUIManager>();
        }

        private void OnOnNewDeviceInUse(InputDevice device)
        {
            if (_newPlayerIndex >= _maxPlayerCount) throw new TooManyPlayersException(device);

            //Spawn UI, set the device and await the selection.
            CharacterSelectUI charSelect = _characterSelectUIManager.SpawnCharacterSelectUI(_newPlayerIndex);
            charSelect.OnCharacterSelected += OnCharacterSelected;
            charSelect.Device = device;

            //Set the index for the next player.
            _newPlayerIndex++;
        }

        private void OnCharacterSelected(PlayerCharacterController characterPrefab, InputDevice device)
        {
            //Spawn the character, pass the device reference.
            PlayerCharacterController playerCharacter = Instantiate(characterPrefab,  (Vector2)_camera.transform.position, Quaternion.identity);
            playerCharacter.Device = device;
            
            _playerCharacters.Add(playerCharacter);
        }
    }

    public class TooManyPlayersException : Exception
    {
        public InputDevice AssociatedDevice { get; private set; }

        public TooManyPlayersException(InputDevice device) 
            : base("Tried to add a new player when the maximum amount of players has already been reached.")
        {
            AssociatedDevice = device;
        }
    }
}
