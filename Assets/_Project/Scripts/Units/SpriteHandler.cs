using System.Collections;
using UnityEngine;

namespace _Project.Scripts.Player
{
    [RequireComponent(typeof(Rigidbody2D)),
     RequireComponent(typeof(Animator)),
     RequireComponent(typeof(SpriteRenderer)),
     RequireComponent(typeof(ICharacterAimSource))]
    public class SpriteHandler : MonoBehaviour
    {
        #region Components
        private Rigidbody2D _rigidbody;             //Physics Component. 
        private Animator _animator;                 //Handles Animation.
        private SpriteRenderer _renderer;           //Handles rendering of the sprite.
        private ICharacterAimSource _characterAim;  //Component to get aiming input from.
        #endregion

        #region Properties

        public Color Color
        {
            get { return _renderer.color; }
            set { _renderer.color = value; }
        }
        #endregion

        private void Awake()
        {
            //Get components.
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _renderer = GetComponent<SpriteRenderer>();
            _characterAim = GetComponent<ICharacterAimSource>();
        }

        private void Update()
        {
            //Tell the animator we're moving if we have velocity.
            _animator.SetBool("IsMoving", _rigidbody.velocity != Vector2.zero);

            float aimingDegree = _characterAim.AimingDegree;

            //We're front facing if we're aiming to the bottom half of the screen.
            _animator.SetBool("IsFrontFacing", aimingDegree <= 0);
            //We want to flip the sprite if we're aiming to the right side of the screen.  
            _renderer.flipX = Mathf.Abs(aimingDegree) <= 90;
        }

        public void FadeToColor(Color color, float fadeDuration)
        {
            StartCoroutine(FadeToColorRoutine(color, fadeDuration));
        }

        private IEnumerator FadeToColorRoutine(Color color, float fadeDuration)
        {
            Color startingColor = Color;

            float elapsedTime = 0;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                Color = Color.Lerp(startingColor, color, elapsedTime / fadeDuration);
                yield return null;
            }

            Color = color;
        }
    }
}
