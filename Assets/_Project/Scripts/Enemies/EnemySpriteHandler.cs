using UnityEngine;
using _Project.Scripts.General;
using _Project.Scripts.Player;
using _Project.Scripts.Units;

namespace _Project.Scripts.Enemies
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(IMovementInputSource))]
    class EnemySpriteHandler : CustomMonoBehaviour
    {
        #region Components
        
        private SpriteRenderer _renderer;
        private IMovementInputSource _movement;

        #endregion

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _movement = GetComponent<IMovementInputSource>();
        }

        private void Update()
        {
            float degree = MathHelper.Vector2Degree(_movement.MovementDirection);
            _renderer.flipX = Mathf.Abs(degree) <= 90;
        }
    }
}
