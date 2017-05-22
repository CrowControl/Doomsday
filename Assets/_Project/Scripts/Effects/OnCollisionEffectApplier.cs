using System.Collections.Generic;
using UnityEngine;
using _Project.Scripts.Effects;
using _Project.Scripts.Units;

namespace _Project.Scripts.Player.Characters.Psycoon
{
    [RequireComponent(typeof(Collider2D))]
    public class OnCollisionEffectApplier : MonoBehaviour
    {
        #region Editor Variables
        [SerializeField] private HealthEffect _effectPrefab;
        [SerializeField] private int _maxHitCount;
        [SerializeField] private float _lifeTime;
        #endregion

        #region Events
        public delegate void OnCollisionHealEventHandler();
        public event OnCollisionHealEventHandler OnFinished;
        #endregion

        #region Internal Variables
        private readonly List<HealthController> _hits = new List<HealthController>();
        #endregion

        private void Awake()
        {
            //Ensure it's a trigger Component.
            GetComponent<Collider2D>().isTrigger = true;

            //Set the lifetime.
            Invoke("Finish", _lifeTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            //Get health component. If none is found or we already collided before, stop.
            HealthController health = other.GetComponent<HealthController>();
            if (health == null || _hits.Contains(health)) return;

            //if we've reached maximum hit count, finish. else, apply.
            _hits.Add(health);
            if (_hits.Count > _maxHitCount)
                Finish();
            else
                Apply(health);
        }

        private void Apply(HealthController health)
        {
            HealthEffect effect = Instantiate(_effectPrefab, health.transform);
            effect.SetTarget(health);
        }

        /// <summary>
        /// Finishes up this healing.
        /// </summary>
        private void Finish()
        {
            CancelInvoke();

            if (OnFinished != null)
                OnFinished();

            Destroy(gameObject);
        }
    }
}
