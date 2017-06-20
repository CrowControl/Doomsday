using System.Collections.Generic;
using System.Linq;
using Assets._Project.Scripts.Units.Abilities;
using UnityEngine;
using _Project.Scripts.General;
using _Project.Scripts.Units;

namespace _Project.Scripts.Effects
{
    public class OnCollisionEffectApplier : CustomMonoBehaviour
    {
        #region Editor Variables
        [SerializeField] private Effect _effectPrefab;          //The effect to apply on collision.
        [SerializeField] private bool _useMaxHitCount = true;
        [SerializeField] private int _maxHitCount;                  //Maximum amount of hits we want to allow.
        [SerializeField] private float _singleTargetHitCooldown;    //Cooldown for a single target that got hit.
        [SerializeField] private bool _destroyOnMaxHitReached;
        #endregion

        #region Components
        private List<ChildCollider> _childColliders;
        #endregion

        #region Properties
        private bool MaxHitsreached
        {
            get { return _useMaxHitCount && _hits >= _maxHitCount; }
        }
        #endregion

        #region Internal Variables

        /// <summary>
        /// Dictionary to keep the hit cooldowns for all targets.
        /// </summary>
        private readonly Dictionary<GameObject, float> _hitCooldowns = new Dictionary<GameObject, float>();
        private int _hits;
        #endregion

        #region Child colliders
        private void Awake()
        {
            _childColliders = GetComponentsInChildren<ChildCollider>().ToList();
            foreach (ChildCollider child in _childColliders)
                child.OnCollisionEnter += CheckCollision;
        }

        public void RegisterChildCollider(ChildCollider child)
        {
            _childColliders.Add(child);
            child.OnCollisionEnter += CheckCollision;
        }

        public void UnRegisterChildCollider(ChildCollider child)
        {
            _childColliders.Remove(child);
            child.OnCollisionEnter -= CheckCollision;
        }
        #endregion

        #region Collision

        private void OnTriggerEnter2D(Collider2D other)
        {
            CheckCollision(other.gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            CheckCollision(collision.gameObject);
        }

        private void OnParticleCollision(GameObject other)
        {
            CheckCollision(other);
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

            //Stop checking child colliders.
            foreach (ChildCollider child in _childColliders)
                child.OnCollisionEnter -= CheckCollision;

            base.OnDestroy();
        }
    }
}
