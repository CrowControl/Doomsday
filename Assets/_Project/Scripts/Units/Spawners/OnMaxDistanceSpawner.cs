using UnityEngine;

namespace _Project.Scripts.Units.Spawners
{
    public class OnMaxDistanceSpawner : Spawner
    {
        #region Editor
        [SerializeField] private float _maxDistance;
        #endregion

        #region Internal
        private Vector2 _previousPosition;
        private float _distanceTraveled;
        #endregion

        private void Awake()
        {
            //Set initial position.
            _previousPosition = transform.position;
        }

        private void Update()
        {
            //get position.
            Vector2 position = transform.position;

            //update distance traveled.
            _distanceTraveled += Vector2.Distance(_previousPosition, position);
            if (_distanceTraveled >= _maxDistance)
                Spawn();

            //save for next update.
            _previousPosition = position;
        }
    }
}
