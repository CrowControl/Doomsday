using System;
using UnityEngine;
using _Project.Scripts.Units.Abilities;

namespace _Project.Scripts.Player.Characters.Merlin
{
    [RequireComponent((typeof(SpriteRenderer)))]
    public class SwordShooter : Ability 
    {
        #region Editor
        [SerializeField] private float _reloadTime;
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

        private void Awake()
        {
            IsLoaded = true;
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
        public override void Do(ICharacterAimSource aimSource)
        {
            if(!IsLoaded)
                throw new InvalidOperationException("Excalibur gun isn't loaded.");

            _abilitySpawner.Spawn(aimSource);
            _renderer.sprite = _unLoadedSwordGunSprite;
            IsLoaded = false;

            Invoke("Reload", _reloadTime);
        }

        public void Reload()
        {
            _renderer.sprite = _loadedSwordGunSprite;
            IsLoaded = true;
        }
    }
}
