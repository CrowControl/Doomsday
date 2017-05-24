using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using _Project.Scripts.Units;

namespace _Project.Scripts.Effects
{
    [RequireComponent(typeof(Collider2D))]
    public class OnCollisionEffectApplier : MonoBehaviour
    {
        #region Editor Variables
        [SerializeField] private List<HealthEffect> _effectPrefabs;        //The effect to apply on collision.
        [SerializeField] private int _maxHitCount;                  //Maximum amount of hits we want to allow.
        [SerializeField] private float _singleTargetHitCooldown;    //Cooldown for a single target that got hit.
        #endregion

        #region Events
        public delegate void OnCollisionHealEventHandler();
        public event OnCollisionHealEventHandler OnFinished;
        #endregion

        #region Internal Variables
        /// <summary>
        /// Dictionary to keep the hit cooldowns for all targets.
        /// </summary>
        private readonly Dictionary<HealthController, float> _hitCooldowns = new Dictionary<HealthController, float>();
        #endregion

        private void Awake()
        {
            //Ensure it's a trigger Component.
            GetComponent<Collider2D>().isTrigger = true;
        }

        /// <summary>
        /// Called when the Collider component attached to this object has a collision.
        /// </summary>
        /// <param name="other">The other collider in this collision.</param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            //Get health component. If none is found or we already collided before, stop.
            HealthController health = other.GetComponent<HealthController>();
            if (health == null || !HitAllowed(health)) return;

            //if we've reached maximum hit count, finish. else, apply.
            _hitCooldowns[health] = Time.time + _singleTargetHitCooldown;
            if (_hitCooldowns.Count > _maxHitCount)
                Finish();
            else
                ApplyEffects(health);
        }

        /// <summary>
        /// Checks if a hit on the healthController should be allowed.
        /// </summary>
        /// <param name="health">The healthcontroller.</param>
        /// <returns>True if allowed, false otherwise.</returns>
        private bool HitAllowed(HealthController health)
        {
            return !_hitCooldowns.Keys.Contains(health) || _hitCooldowns[health] <= Time.time;
        }

        /// <summary>
        /// Applies the effect to the healthController.
        /// </summary>
        /// <param name="health">The healthcontroller.</param>
        private void ApplyEffects(HealthController health)
        {
            foreach (HealthEffect effect in _effectPrefabs)
                IEffect<HealthController>.ApplyEffect(effect, health);
        }

        /// <summary>
        /// Finishes.
        /// </summary>
        private void Finish()
        {
            Destroy(gameObject);
        }

        /// <summary>
        /// Called when this object is destroyed.
        /// </summary>
        private void OnDestroy()
        {
            //Cancel any outstanding invokes.
            CancelInvoke();
            
            //Call event.
            if (OnFinished != null)
                OnFinished();
        }
    }
}
