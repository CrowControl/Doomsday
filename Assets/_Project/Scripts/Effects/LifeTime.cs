using UnityEngine;

namespace _Project.Scripts.Effects
{
    public class LifeTime : MonoBehaviour
    {
        #region Editor Variables
    
        [SerializeField] private float _duration;

        #endregion

        #region Internal Variables
        private float _elapsedTime;
        private bool _paused;
        #endregion

        private void Start()
        {
            Restart();
        }
        public void Restart()
        {
            _elapsedTime = 0;
            _paused = false;
        }

        private void Update()
        {
            if (_paused) return;

            _elapsedTime += Time.deltaTime;
            if (_elapsedTime > _duration)
                Finish();
        }

        public void Pause()
        {
            _paused = true;
        }

        public void Resume()
        {
            _paused = false;
        }

        public void Finish()
        {
            Destroy(gameObject);
        }
    }
}
