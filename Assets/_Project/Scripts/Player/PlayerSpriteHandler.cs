using System.Collections;
using UnityEngine;
using _Project.Scripts.Units;

namespace _Project.Scripts.Player
{
    [RequireComponent(typeof(Rigidbody2D)),
     RequireComponent(typeof(Animator)),
     RequireComponent(typeof(SpriteRenderer)),
     RequireComponent(typeof(ICharacterAimSource))]
    public class PlayerSpriteHandler : MonoBehaviour
    {
        #region Components
        private Rigidbody2D _rigidbody;             //Physics Component. 
        private Animator _animator;                 //Handles Animation.
        private SpriteRenderer _renderer;           //Handles rendering of the sprite.
        private ICharacterAimSource _characterAim;  //Component to get aiming input from.

        private ISpriteHandlerAssistant[] _assistants;
        #endregion

        #region Properties

        public Color Color
        {
            get { return _renderer.color; }
            set { _renderer.color = value; }
        }

        public bool XFlippingEnabled { get; set; }
        #endregion

        private void Awake()
        {
            XFlippingEnabled = true;

            GetComponents();
        }

        private void GetComponents()
        {
            //Get components.
            _rigidbody = GetComponent<Rigidbody2D>();
            _renderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            _characterAim = GetComponent<ICharacterAimSource>();
            _assistants = GetComponentsInChildren<ISpriteHandlerAssistant>();
        }

        private void Update()
        {
            //Tell the animator we're moving if we have velocity.
            _animator.SetBool("IsMoving", _rigidbody.velocity != Vector2.zero);
            
            //We want to flip the sprite if we're aiming to the right side of the screen, except when we don't xD.  
            if(XFlippingEnabled)
                SetFlipX(Mathf.Abs(_characterAim.AimingDegree) <= 90);
        }


        /// <summary>
        /// Sets the flipping of the entire character.
        /// </summary>
        /// <param name="flipped">if true, we flip.</param>
        private void SetFlipX(bool flipped)
        {
            foreach (ISpriteHandlerAssistant assistant in _assistants)
                assistant.SetFlipX(flipped);

            //Choose.
            float x = flipped ? 180 : 0;

            //Assign.
            transform.rotation = Quaternion.Euler(0, x, 0);
        }

        #region Color
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
        #endregion
    }

    public interface ISpriteHandlerAssistant
    {
        void SetFlipX(bool flipped);
    }
}
