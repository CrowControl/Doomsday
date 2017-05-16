using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//todo add User controls
//todo add actual selection
public class CharacterSelectUI : MonoBehaviour
{
    #region Editor Variables

    [SerializeField] private float _changeFocusCooldown;
    #endregion

    #region Internal Variables
    private CharacterSelectHeadShot[] _headshots;
    private CharacterSelectHeadShot _focusedHeadShot;
    private int _focusedHeadshotIndex;
    private bool _onRefocusCooldown;
    #endregion

    #region Properties
    public InputDevice Controller { get; set; }
    #endregion

    #region Events

    public delegate void CharacterSelectEventHandler(PlayerController character);
    public event CharacterSelectEventHandler OnCharacterSelected;
    #endregion

    private void Awake()
    {
        _headshots = GetComponentsInChildren<CharacterSelectHeadShot>();
    }

    private void Start()
    {
        ChangeFocusTo(_headshots[0]);
    }

    private void Update()
    {
        if (Controller == null) return;

        UpdateFocus();
        CheckSelection();
    }

    /// <summary>
    /// Updates which headshot is currently focussed on.
    /// </summary>
    private void UpdateFocus()
    {
        //Stop if we're on cooldown or no input was registered.
        float direction = Controller.LeftStickX.Value;
        if (_onRefocusCooldown || direction == null) return;

        //Move focus.
        if (direction > 0)
            FocusMoveLeft();
        else
            FocusMoveRight();

        //Start the cooldown.
        StartCooldown();
    }

    /// <summary>
    /// Checks if the player has made it's choice. If true, applies choice.
    /// </summary>
    private void CheckSelection()
    {
        //todo
    }

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
        _focusedHeadShot.Focus();
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
