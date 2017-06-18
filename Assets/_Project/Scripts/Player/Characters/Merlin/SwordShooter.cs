using System;
using System.Collections.Generic;
using UnityEngine;
using _Project.Scripts.Units.Abilities;
using _Project.Scripts.Units.Spawners;

namespace _Project.Scripts.Player.Characters.Merlin
{
    [RequireComponent((typeof(SpriteRenderer)))]
    public class SwordShooter : Ability 
    {
        #region Editor
        [SerializeField] private float _reloadTime;
        [SerializeField] private int _maxSwordCount = 4;
        [SerializeField] private Sprite _loadedSwordGunSprite;
        [SerializeField] private Sprite _unLoadedSwordGunSprite;
        #endregion

        #region Components

        private AbilitySpawner _abilitySpawner;
        private SpriteRenderer _renderer;

        #endregion

        #region Properties

        public bool IsLoaded { get; private set; }

        #endregion

        #region Internal
        private SwordDb _swords;
        #endregion

        private void Awake()
        {
            IsLoaded = true;
            _swords = new SwordDb(this);

            GetComponents();
        }

        /// <summary>
        /// Gets all necessary components.
        /// </summary>
        private void GetComponents()
        {
            _abilitySpawner = GetComponentInChildren<AbilitySpawner>();
            _renderer = GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// Shoots a sword.
        /// </summary>
        /// <param name="aimSource"></param>
        public override void Activate(ICharacterAimSource aimSource)
        {
            //Can't shoot if not reloaded yet.
            if(!IsLoaded)
                throw new InvalidOperationException("Excalibur gun isn't loaded.");

            //Spawn sword, add to list.
            Ability sword = _abilitySpawner.Spawn(aimSource);
            _swords.AddSword(sword);

            //Set gun to reloading.
            _renderer.sprite = _unLoadedSwordGunSprite;
            IsLoaded = false;
            Invoke("Reload", _reloadTime);
        }

        /// <summary>
        /// Reloads the gun.
        /// </summary>
        public void Reload()
        {
            _renderer.sprite = _loadedSwordGunSprite;
            IsLoaded = true;
        }

        /// <summary>
        /// Keeps track of the swords shot by this sword shooter, and removes the oldest if to much swords are in the scene.
        /// </summary>
        private class SwordDb
        {
            /// <summary>
            /// The source of the swords that this db keeps track of.
            /// </summary>
            private readonly SwordShooter _swordShooter;           
            
            /// <summary>
            /// The swords.
            /// </summary>
            private readonly List<SwordReference> _swords = new List<SwordReference>();

            public SwordDb(SwordShooter swordShooter)
            {
                _swordShooter = swordShooter;
            }

            /// <summary>
            /// Adds a new sword to the db.
            /// </summary>
            /// <param name="swordObject">The sword object that we add.</param>
            public void AddSword(Ability sword)
            {
                if (_swords.Count == _swordShooter._maxSwordCount)
                    RemoveOldestSword();

                _swords.Add(new SwordReference(sword, this));
            }

            private void RemoveOldestSword()
            {
                if (_swords.Count == 0) return;

                SwordReference sword = _swords[0];
                _swords.Remove(sword);

                sword.RemoveFromScene();
            }

            /// <summary>
            /// Keeps track of a single sword, in both it's flying state as it's stuck state.
            /// </summary>
            private class SwordReference
            {
                private readonly SwordDb _db;

                //References to the different states the sword can be.
                private readonly Projectile _flyingSword;               
                private readonly OnCollisionSpawner _stuckSwordSpawner; 
                private StuckExcalibur _stuckSword;                     

                private bool _removed;

                public SwordReference(Ability sword, SwordDb db)
                {
                    //Get the references.
                    _db = db;
                    _flyingSword = sword.GetComponent<Projectile>();
                    _stuckSwordSpawner = sword.GetComponent<OnCollisionSpawner>();

                    //Subscribe to event that change the obejct that we track.
                    _stuckSwordSpawner.OnSpawn += OnStuckSwordSpawned;
                    _flyingSword.OnDestroyed += RemoveFromScene;
                }

                /// <summary>
                /// This is called when the flying swrod rtransitions to a stuck sword. 
                /// </summary>
                /// <param name="spawnedobject">The stuck sword gameobject.</param>
                private void OnStuckSwordSpawned(GameObject spawnedobject)
                {
                    //unsubscribe from old events.
                    _stuckSwordSpawner.OnSpawn -= OnStuckSwordSpawned;
                    _flyingSword.OnDestroyed -= RemoveFromScene;

                    //Get the sword component and subscribe to its destroy-event.
                    _stuckSword = spawnedobject.GetComponent<StuckExcalibur>();
                    _stuckSword.OnDestroyed += RemoveFromScene;
                }

                public void RemoveFromScene()
                {
                    if (_removed) return;

                    //Unsubscribe from all events.
                    if (_flyingSword != null)
                        _flyingSword.OnDestroyed -= RemoveFromScene;

                    if (_stuckSwordSpawner != null)
                        _stuckSwordSpawner.OnSpawn -= OnStuckSwordSpawned;

                    if (_stuckSword != null)
                        _stuckSword.OnDestroyed -= RemoveFromScene;

                    //Destroy the object.
                    if (_flyingSword != null)
                        Destroy(_flyingSword.gameObject);

                    if (_stuckSword != null)
                        Destroy(_stuckSword.gameObject);

                    _removed = true;
                }
            }
        }
    }
}
