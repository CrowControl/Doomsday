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
        [SerializeField] private Effect _effectPrefab;          //The effect to apply on collision.
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
        private readonly Dictionary<GameObject, float> _hitCooldowns = new Dictionary<GameObject, float>();
        private int _hits;
        #endregion

        private void Awake()
        {
            //Ensure it's a trigger Collider.
            GetComponent<Collider2D>().isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            GameObject otherObject = other.gameObject;
            if (OnHitCooldown(otherObject))
                return;

            if (Effect.TryApplyEffect(_effectPrefab, otherObject))
            {
                StartHitCooldown(otherObject);

                _hits++;
                CheckForMaxHitReached();
            }
        }

        #region Cooldowns
        private bool OnHitCooldown(GameObject obj)
        {
            return _hitCooldowns.Keys.Contains(obj) && _hitCooldowns[obj] > Time.time;
        }

        private void StartHitCooldown(GameObject obj)
        {
            _hitCooldowns[obj] = Time.time + _singleTargetHitCooldown;
        }
        #endregion

        private void CheckForMaxHitReached()
        {
            if (_hits > _maxHitCount)
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
