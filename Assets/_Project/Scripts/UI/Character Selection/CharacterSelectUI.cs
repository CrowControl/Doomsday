using System.Linq;
using InControl;
using UnityEngine;
using _Project.Scripts.Player;

namespace _Project.Scripts.UI.Character_Selection
{
    public class CharacterSelectUI : MonoBehaviour
    {
        #region Editor Variables
        /// <summary>
        /// Cooldown on changing which of the headshots is currently focused on. 
        /// </summary>
        [SerializeField] private float _changeFocusCooldown;

        [SerializeField] private Color _focusOutlineColor;
        [SerializeField] private float _betweenHeadshotSpace;
        [SerializeField] private string _headshotResourcePath;
        #endregion

        #region Internal Variables
        private CharacterSelectHeadShot[] _headshots;       //Array of all the headshots.
        private CharacterSelectHeadShot _focusedHeadShot;   //The headshot that's currently focused on.
        private int _focusedHeadshotIndex;                  //index in the array of the focused headshot.
        private bool _onRefocusCooldown;                    //true if currently on cooldown for changing focus, flase otherwise.
        #endregion

        #region Properties
        /// <summary>
        /// Device that this UI reads it's input from.
        /// </summary>
        public InputDevice Device { get; set; }
        #endregion

        #region Events
        public delegate void CharacterSelectEventHandler(PlayerCharacterController playerCharacter, InputDevice controller);
        /// <summary>
        /// Event that is thrown when a character has been selected. Includes a reference to the character prefab in the parameters.
        /// </summary>
        public event CharacterSelectEventHandler OnCharacterSelected;
        #endregion

        private void Awake()
        {
            _headshots = Resources.LoadAll<CharacterSelectHeadShot>(_headshotResourcePath);
            InstantiateHeadshots();
        }

        private void Start()
        {
            //Set the headshot positions. Needs to happen in start so the headshots have time to initialize their components, which we need for their position.
            SetHeadshotPositions();
            //set the starting focus.
            ChangeFocusTo(0);
        }

        #region Headshot Initialization
        private void InstantiateHeadshots()
        {
            for (int i = 0; i < _headshots.Length; i++)
                _headshots[i] = Instantiate(_headshots[i], transform);
        }

        /// <summary>
        /// Sets the headshot positions.
        /// </summary>
        private void SetHeadshotPositions()
        {
            //If only 1 or less headshots, it's position is already fine.
            if (_headshots.Length <= 1) return;

            //Calculate the total width of this element.
            float totalBetweenSpace = (_headshots.Length - 1) * _betweenHeadshotSpace;
            float heatShotsWidthSum = _headshots.Sum(h => h.Width);
            float totalWidth = totalBetweenSpace + heatShotsWidthSum;

            //Set the leftmost headshot's position.

            float xPos = -totalWidth / 2 + _headshots[0].Width / 2; //Have the total width to left, but half it's own width to the right.
            Vector2 position = new Vector2(xPos, 0);
            _headshots[0].transform.localPosition = position;

            //Set the rest of the positions.
            for (int i = 1; i < _headshots.Length; i++)
            {
                position.x += _headshots[i - 1].Width / 2 + _betweenHeadshotSpace + _headshots[i].Width / 2;
                _headshots[i].transform.localPosition = position;
            }
        }
        #endregion

        #region Update
        private void Update()
        {
            if (Device == null) return;

            UpdateFocus();
            CheckSelection();
        }

        /// <summary>
        /// Updates which headshot is currently focussed on.
        /// </summary>
        private void UpdateFocus()
        {
            //Stop if we're on cooldown or no input was registered.
            float direction = Device.LeftStickX.Value;
            if (_onRefocusCooldown || direction == 0) return;

            //Move focus.
            if (direction < 0)
                FocusMoveLeft();
            else
                FocusMoveRight();

            //Start the cooldown.
            StartCooldown();
        }

        /// <summary>
        /// Check if the player has made it's choice. If true, apply choice.
        /// </summary>
        private void CheckSelection()
        {
            if (!Device.Action1.WasPressed) return;

            //Throw select event and destroy this ui.
            OnCharacterSelected(_focusedHeadShot.AssociatedCharacterPrefab, Device);
            Destroy(gameObject);
        }

        #endregion

        #region Changing focused headshot.
        /// <summary>
        /// Moves the focus one  headshot to the left.
        /// </summary>
        private void FocusMoveLeft()
        {
            //decrement index.
            _focusedHeadshotIndex--;

            //Wrap around if we're out of bounds.
            if (_focusedHeadshotIndex < 0)
                _focusedHeadshotIndex = _headshots.Length - 1;

            //Apply new index.
            ChangeFocusTo(_focusedHeadshotIndex);
        }

        /// <summary>
        /// 
        /// Moves the focus one  headshot to the right.
        /// </summary>
        private void FocusMoveRight()
        {
            //increment index.
            _focusedHeadshotIndex++;

            //Wrap around if we're out of bounds.
            if (_focusedHeadshotIndex >= _headshots.Length)
                _focusedHeadshotIndex = 0;

            //Apply the new index.
            ChangeFocusTo(_focusedHeadshotIndex);
        }

        /// <summary>
        /// Changes the currently selected headshot to the given headshot.
        /// </summary>
        /// <param name="headshotIndex">index of the headshot to be selected.</param>
        private void ChangeFocusTo(int headshotIndex)
        {
            CharacterSelectHeadShot newFocus = _headshots[headshotIndex];

            //UnFocus the current focus.
            if(_focusedHeadShot != null)
                _focusedHeadShot.UnFocus();

            //Focus the new headshot.
            _focusedHeadShot = newFocus;
            _focusedHeadShot.Focus(_focusOutlineColor);
        }
        #endregion

        #region Cooldown
        /// <summary>
        /// Starts the cooldown during which you can't move the focus.
        /// </summary>
        private void StartCooldown()
        {
            _onRefocusCooldown = true;
            Invoke("StopCooldown", _changeFocusCooldown);
        }

        /// <summary>
        /// Stops the cooldown during which you can't move the focus.
        /// </summary>
        private void StopCooldown()
        {
            _onRefocusCooldown = false;
        }

        #endregion
    }
}
