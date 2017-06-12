using UnityEngine;
using _Project.Scripts.Units;
using _Project.Scripts.Units.Abilities;

namespace _Project.Scripts.Player.Characters.Jean
{
    public class ShieldController : Ability
    {
        #region Internal Variables
        private PlayerSpriteHandler _spawnerSprite;   //Sprite of the spawning character. (Assumes this shield is spawned as a child.)s
        #endregion

        public override void Do(ICharacterAimSource aimSource)
        {
            RotateTowardAim(aimSource);
            DisableSpriteChanges();
        }

        /// <summary>
        /// We want the sprite to make no changes in how it's perceived to move during shield ability.
        /// </summary>
        private void DisableSpriteChanges()
        {
            _spawnerSprite = GetComponentInParent<PlayerSpriteHandler>();
            _spawnerSprite.enabled = false;
        }

        /// <summary>
        /// Called when this object is destroyed.
        /// </summary>
        protected override void OnDestroy()
        {
            //We re-enable the spritehandler again.
            _spawnerSprite.enabled = true;
            base.OnDestroy();
        }
    }
}
