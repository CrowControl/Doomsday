using UnityEngine;
using _Project.Scripts.Units;

namespace _Project.Scripts.Effects
{
    public class HealthEffect : Effect
    {
        #region Editor Variables
        [SerializeField] private float _healthChangePerTick;                //Amount of health changed each tick. Positive for a heal, negative for damage.
        [SerializeField] private float _tickDuration;                       //Duration of a tick.
        [SerializeField] private int _maxTickCount;                         //Maximum amount of ticks.  
        [SerializeField] private bool _startFirstTickImmediately = true;    //If true, the first tick happens immediately. otherwise, it will wait the tickduration.
        #endregion

        #region Components
        private HealthController _health;
        #endregion

        #region Events
        public delegate void HealthEffectEventHandler();
        public event HealthEffectEventHandler OnEffectFinished;
        #endregion

        #region Internal Variables
        private int _tickCount;     //Amount of ticks up until now.
        #endregion

        public override bool HasTargetComponent(GameObject gameObj)
        {
            return (gameObj.GetComponent<HealthController>() != null);
        }

        protected override void Apply(GameObject gameObj)
        {
            _health = gameObj.GetComponent<HealthController>();
            StartTicking();
        }
    
        /// <summary>
        /// Starts the ticking.
        /// </summary>
        private void StartTicking()
        {
            float firstTickWaitTime = _startFirstTickImmediately ? 0 : _tickDuration;
            InvokeRepeating("Apply", firstTickWaitTime, _tickDuration);
        }

        /// <summary>
        /// Applies the effect.
        /// </summary>
        private void Apply()
        {
            //Finish if we have exceeded the maximum tick count.
            _tickCount++;
            if(_tickCount > _maxTickCount)
                Destroy(gameObject);

            _health.HP += _healthChangePerTick;
        }

        private void OnDestroy()
        {
            //Cancel all invokes.
            CancelInvoke();

            //Call event.
            if (OnEffectFinished != null)
                OnEffectFinished();
        }
    }
}
