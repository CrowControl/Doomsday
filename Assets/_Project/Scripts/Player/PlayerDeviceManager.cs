using System.Collections.Generic;
using InControl;
using UnityEngine;

namespace _Project.Scripts.Player
{
    public class PlayerDeviceManager : MonoBehaviour
    {
        #region Variables
        #region Internal Variables
        //The devices currently being used.
        private readonly List<InputDevice> _devicesInUse = new List<InputDevice>();
        #endregion

        #region Events
        public delegate void DeviceEventHandler(InputDevice device);
        /// <summary>
        /// Event that triggers when a new device is registered.
        /// </summary>
        public event DeviceEventHandler OnNewDeviceInUse;
        public event DeviceEventHandler OnDeviceDetached;
        #endregion
        #endregion

        #region Unity Methods (Messages)

        private void Start()
        {
            InputManager.OnDeviceDetached += UnRegisterDevice;
        }

        private void Update()
        {
            CheckForNewDevice();
        }

        #endregion

        #region Checks
        /// <summary>
        /// Checks if there is a new input-device active, and calls OnNewDeviceInUse if true.
        /// </summary>
        private void CheckForNewDevice()
        {
            InputDevice device = InputManager.ActiveDevice;
            if (WasJoinButtonPressed(device) && !IsInUse(device))
            {
                if (OnNewDeviceInUse == null) return;

                try
                {
                    OnNewDeviceInUse(device);
                    _devicesInUse.Add(device);
                }
                catch (TooManyPlayersException exception)
                {
                    _devicesInUse.Remove(exception.AssociatedDevice);
                }
            }
        }

        /// <summary>
        /// Checks if the device has any of the join buttons pressed.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>True if a join-button was pressed. false otherwise.</returns>
        private bool WasJoinButtonPressed(InputDevice device)
        {
            return device.AnyButton.WasPressed;
        }

        /// <summary>
        /// Checks if the device is current;y in use.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>True if the device is in use, false otherwise.</returns>
        private bool IsInUse(InputDevice device)
        {
            return _devicesInUse.Contains(device);
        }
        #endregion

        #region Event Methods

        /// <summary>
        /// Called when a device is detached.
        /// </summary>
        /// <param name="device"></param>
        public void UnRegisterDevice(InputDevice device)
        {
            if (OnDeviceDetached != null)
                OnDeviceDetached(device);

            _devicesInUse.Remove(device);
        }

        #endregion
    }
}
