using System.Collections.Generic;
using UnityEngine;
using _Project.Scripts.General;
using _Project.Scripts.Player;

namespace _Project.Scripts.Enemies
{
    [RequireComponent(typeof(Collider2D))]
    class EnemyRange : CustomMonoBehaviour
    {
        #region Variables

        #region Public

        #region Properties

        public PlayerCharacterController NearestPlayer { get; private set; }

        #endregion

        #region Events

        public delegate void RangeEventHandler(PlayerCharacterController player);
        public event RangeEventHandler OnFirstPlayerEnteredRange;   //Called when a player enters the range when there was none in it.
        public event RangeEventHandler OnLastPlayerExitedRange;     //Called when a player exits the range, after which no other players are in range.
        public event RangeEventHandler OnNewNearestPlayer;           //Called when which player is closest in range changes.

        #endregion

        #endregion

        #region Components

        private Collider2D _collider;

        #endregion

        #region Internal Variables

        private readonly List<PlayerCharacterController> _playersInRange = new List<PlayerCharacterController>();

        #endregion

        #endregion

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _collider.isTrigger = true;
        }

        private void Update()
        {
            UpdateNearestPlayer();
        }

        /// <summary>
        /// Updates our nearest player property.
        /// </summary>
        private void UpdateNearestPlayer()
        {
            PlayerCharacterController nearestPlayer = null;
            float nearestPlayerDistance = int.MaxValue;

            //Get the nearest player.
            foreach (PlayerCharacterController player in _playersInRange)
            {
                float distance = Vector2.Distance(transform.position, player.transform.position);
                if (distance < nearestPlayerDistance)
                {
                    nearestPlayer = player;
                    nearestPlayerDistance = distance;
                }
            }

            //Throw event if it's a new one.
            if (nearestPlayer != NearestPlayer && OnNewNearestPlayer != null)
                OnNewNearestPlayer(nearestPlayer);

            //Update property.
            NearestPlayer = nearestPlayer;
        }

        #region Collision Event Methods

        /// <summary>
        /// Called when a collider enters our range.
        /// </summary>
        private void OnTriggerEnter2D(Collider2D other)
        {
            //Get player component, stop if none is found.
            PlayerCharacterController player = other.gameObject.GetComponent<PlayerCharacterController>();
            if (player == null) return;

            //Add to list, throw event if it's the first.
            _playersInRange.Add(player);
            if (_playersInRange.Count == 1 && OnFirstPlayerEnteredRange != null)
                OnFirstPlayerEnteredRange(player);
        }

        /// <summary>
        /// Called when a collider exits our range.
        /// </summary>
        private void OnTriggerExit2D(Collider2D other)
        {
            //Get player component, stop if none is found.
            PlayerCharacterController player = other.gameObject.GetComponent<PlayerCharacterController>();
            if (player == null) return;

            
            //Check if it's the last.
            bool isLastPLayer = _playersInRange.Count == 1 &&   //One player left.
                                _playersInRange[0] == player;   //Its this one.
            //throw event if its the last.
            if (isLastPLayer && OnLastPlayerExitedRange != null)
                OnLastPlayerExitedRange(player);

            //remove from list.
            _playersInRange.Remove(player);
        }
        #endregion
    }
}
