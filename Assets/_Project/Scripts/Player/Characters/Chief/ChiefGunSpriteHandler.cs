using UnityEngine;
using _Project.Scripts.General;
using _Project.Scripts.Units.Spawners;

namespace _Project.Scripts.Player.Characters.Chief
{
    class ChiefGunSpriteHandler : CustomMonoBehaviour, ISpriteHandlerAssistant
    {
        public AbilitySpawner AbilitySpawner { get; private set; }

        private SpriteRenderer _renderer;

        private void Awake()
        {
            AbilitySpawner = GetComponentInChildren<AbilitySpawner>();
            _renderer = GetComponentInChildren<SpriteRenderer>();
        }

        public void SetSprite(Sprite sprite)
        {
            _renderer.sprite = sprite;
        }

        public void SetFlipX(bool flipped)
        {
            //Get local position
            Vector3 position = transform.localPosition;

            //Set local position.y to negative if we need to flip.
            float absPosY = Mathf.Abs(position.y);
            if (!flipped)
                absPosY *= -1;

            //Set updated position.
            position.y = absPosY;
            transform.localPosition = position;
        }
    }
}
