using UnityEngine;
using _Project.Scripts.General;
using _Project.Scripts.Player;

namespace _Project.Scripts.Enemies
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(ICharacterAimSource))]
    class EnemySpriteHandler : CustomMonoBehaviour
    {
        #region Components
        
        private SpriteRenderer _renderer;
        private ICharacterAimSource _aimSource;

        #endregion

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _aimSource = GetComponent<ICharacterAimSource>();
        }

        private void Update()
        {
            _renderer.flipX = Mathf.Abs(_aimSource.AimingDegree) <= 90;
        }
    }
}
