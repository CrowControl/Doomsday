using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InControl;
using UnityEngine;
using _Project.Scripts.UI.Character_Selection;

namespace _Project.Scripts.Player
{
    [RequireComponent(typeof(PlayerDeviceManager))]
    public class PlayerManager : MonoBehaviour
    {
        FMOD.Studio.EventInstance gameMusic;

        #region Editor Variables
        [SerializeField] private float _maxPlayerCount;     //Maximum amount of players allowed.
        [SerializeField] private float _timeUntilOrphanDies;
        #endregion

        #region Other Objects
        private CharacterSelectUIManager _characterSelectUIManager; //The object that spawns Character select UI.
        private PlayerDeviceManager _deviceManager;
        private Camera _camera;
        #endregion

        #region Properties
        public List<PlayerCharacterController> Players { get { return _playerCharacters; } }
        #endregion

        #region Internal Variables
        private readonly List<PlayerCharacterController> _playerCharacters = new List<PlayerCharacterController>();
        private int _newPlayerIndex;

        private readonly Dictionary<PlayerCharacterController, IEnumerator> _orphans = new Dictionary<PlayerCharacterController, IEnumerator>();
        #endregion

        private void Awake()
        {
            gameMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Music_level_1");
            _camera = Camera.main;
        }

        private void Start()
        {
            gameMusic.start();

            _deviceManager = GetComponent<PlayerDeviceManager>();
            _deviceManager.OnNewDeviceInUse += RegisterNewPlayer;
            _deviceManager.OnDeviceDetached += OrphanPlayerCharacter;

            _characterSelectUIManager = FindObjectOfType<CharacterSelectUIManager>();
        }
        
        #region New Players

        private void RegisterNewPlayer(InputDevice device)
        {
            //Can't have too many players.
            if (_newPlayerIndex >= _maxPlayerCount)
                throw new TooManyPlayersException(device);

            //Spawn UI,.
            CharacterSelectUI charSelect = _characterSelectUIManager.SpawnCharacterSelectUI(_newPlayerIndex);
            
            //Set device.
            charSelect.Device = device;

            //Hook up to both succes and failure events.
            charSelect.OnCharacterSelected += OnCharacterSelected;
            charSelect.OnSelectionFailed += (c, inputDevice) => _deviceManager.UnRegisterDevice(inputDevice);
            
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

        #endregion

        #region The Orphanage
        /// <summary>
        /// Called when a controller disconnects. The connected character will live on for a short while as an orphan,
        /// but if no new controller comes along to adopt him quickly, he dies.
        /// </summary>
        /// <param name="device">The device of the playerCharacter that is orphaned.s</param>
        private void OrphanPlayerCharacter(InputDevice device)
        {
            //Find the playeCharacter with the given device.
            PlayerCharacterController player = _playerCharacters.Find(p => p.Device == device);
            if (player == null) return;

            //Remove the device reference.
            player.Device = null;

            //Newly connected devices should now be directed to the orphanage instead of creating new characters.
            if (_orphans.Count == 0)
            {
                _deviceManager.OnNewDeviceInUse -= RegisterNewPlayer;
                _deviceManager.OnNewDeviceInUse += AdoptOrphanedCharacter;
            }

            //Start the countdown till death.
            IEnumerator countdown = CountDownCoroutine(player, _timeUntilOrphanDies);
            _orphans[player] = countdown;

            StartCoroutine(countdown);
        }

        /// <summary>
        /// Let the newly connected device adopt a still living but uncontrolled character.
        /// </summary>
        /// <param name="device">the device that will adopt a character.</param>
        private void AdoptOrphanedCharacter(InputDevice device)
        {
            //Get the orphan.
            KeyValuePair<PlayerCharacterController, IEnumerator> orphan = _orphans.First();
            PlayerCharacterController characterController = orphan.Key;
            IEnumerator countdown = orphan.Value;

            //Connect to the device.
            _orphans.Remove(characterController);
            characterController.Device = device;

            //Stop the orphans death timer.
            StopCoroutine(countdown);
            OrphanCountCheck();
        }

        private IEnumerator CountDownCoroutine(PlayerCharacterController player, float duration)
        {
            yield return new WaitForSeconds(duration);

            //Remove from list.
            _orphans.Remove(player);
            OrphanCountCheck();

            //The orphan dies.
            Destroy(player.gameObject);
        }

        /// <summary>
        /// Checks if there are any orphans left. If not, redirect new devices to create new characters.
        /// </summary>
        private void OrphanCountCheck()
        {
            if (_orphans.Count != 0) return;

            _deviceManager.OnNewDeviceInUse -= AdoptOrphanedCharacter;
            _deviceManager.OnNewDeviceInUse += RegisterNewPlayer;
        }

        #endregion
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
