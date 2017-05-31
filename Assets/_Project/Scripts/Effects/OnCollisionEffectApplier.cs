using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using _Project.Scripts.Units;

namespace _Project.Scripts.Effects
{
    [RequireComponent(typeof(Collider2D))]
    public class OnCollisionEffectApplier : CustomMonoBehaviour
    {
        #region Editor Variables
        [SerializeField] private Effect _effectPrefab;          //The effect to apply on collision.
        [SerializeField] private int _maxHitCount;                  //Maximum amount of hits we want to allow.
        [SerializeField] private float _singleTargetHitCooldown;    //Cooldown for a single target that got hit.
        [SerializeField] private bool _destroyOnMaxHitReached;
        #endregion

        #region Properties
        private bool MaxHitsreached
        {
            get { return _hits >= _maxHitCount; }
        }
        #endregion

        #region Internal Variables

        /// <summary>
        /// Dictionary to keep the hit cooldowns for all targets.
        /// </summary>
        private readonly Dictionary<GameObject, float> _hitCooldowns = new Dictionary<GameObject, float>();
        private int _hits;
        #endregion

        //private void Awake()
        //{
        //    //Ensure it's a trigger Collider.
        //    GetComponent<Collider2D>().isTrigger = true;
        //}

        #region Collision

        private void OnTriggerEnter2D(Collider2D other)
        {
            CheckCollision(other.gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            CheckCollision(collision.gameObject);
        }

        private void CheckCollision(GameObject gameObj)
        {
            if (MaxHitsreached || OnHitCooldown(gameObj))
                return;

            if (Effect.TryApplyEffect(_effectPrefab, gameObj))
            {
                StartHitCooldown(gameObj);

                _hits++;
                CheckForMaxHitReached();
            }
        }
        #endregion

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
            if (_destroyOnMaxHitReached && MaxHitsreached)
                Destroy(gameObject);
        }

        /// <summary>
        /// Called when this object is destroyed.
        /// </summary>
        protected override void OnDestroy()
        {
            //Cancel any outstanding invokes.
            CancelInvoke();
            
            base.OnDestroy();
        }
    }
}
