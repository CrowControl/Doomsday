using System.Collections.Generic;
using UnityEngine;
using _Project.Scripts.Player;

namespace _Project.Scripts.General
{
    [RequireComponent(typeof(Player.PlayerManager))]
    public class PlayerCenteredCameraController : CustomMonoBehaviour
    {
        #region Editor
        [SerializeField] private float _minSize, _maxSize;
        [SerializeField] private float _playerDistanceAtMinSize, _playerDistanceAtMaxSize;
        [SerializeField] private float _singlePlayerSize;
        #endregion

        #region Components
        private Player.PlayerManager _playerManager;
        private TransformLimitManager _cameraLimits;
        private Camera _camera;
        #endregion

        // Use this for initialization
        private void Awake()
        {
            _playerManager = GetComponent<Player.PlayerManager>();

            _camera = Camera.main;
            _cameraLimits = _camera.GetComponent<TransformLimitManager>();
        }

        // Update is called once per frame
        private void LateUpdate ()
        {
            UpdateCameraPosition();
            UpdateViewPortsize();
        }

        private void UpdateCameraPosition()
        {
            List<PlayerCharacterController> players = _playerManager.Players;
            if (players.Count == 0) return; 

            //Sum positions
            Vector3 sum = Vector3.zero;
            foreach (PlayerCharacterController player in players)
                sum = sum + player.transform.position;

            //Get center position by getting the average, and use it as camera position.
            Vector3 center = sum / players.Count;
            _camera.transform.position = new Vector3(center.x, center.y, _camera.transform.position.z);
            _cameraLimits.ApplyLimits(_camera.transform);
        }
        private void UpdateViewPortsize()
        {
            //Single player has set size.
            if (_playerManager.Players.Count <= 1)
            {
                _camera.orthographicSize = _singlePlayerSize;
                return;
            }

            //Calcualte centered size for multiple players.
            float percentage = CalculatePlayerDistancePercentage();
            _camera.orthographicSize = Mathf.Lerp(_minSize, _maxSize, percentage);
        }

        private float CalculatePlayerDistancePercentage()
        {
            //Find the furthest distance.
            float furthestPlayerDistance = 0;
            foreach (PlayerCharacterController player in _playerManager.Players)
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);
                if (distance > furthestPlayerDistance)
                    furthestPlayerDistance = distance;
            }

            //Clamp percentage to bounds.
            if (furthestPlayerDistance < _playerDistanceAtMinSize)
                return 0;
            if (furthestPlayerDistance > _playerDistanceAtMaxSize)
                return 1;

            //Calculate percentage in between bounds.
            return (furthestPlayerDistance - _playerDistanceAtMinSize) /
                   (_playerDistanceAtMaxSize - _playerDistanceAtMinSize);
        }
    }
}
