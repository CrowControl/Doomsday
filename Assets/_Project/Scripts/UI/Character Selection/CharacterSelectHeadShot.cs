using UnityEngine;
using UnityEngine.UI;
using _Project.Scripts.Player;

namespace _Project.Scripts.UI.Character_Selection
{
    [RequireComponent(typeof(Outline)), 
     RequireComponent(typeof(RectTransform))]
    public class CharacterSelectHeadShot : MonoBehaviour
    {
        #region Editor Variables
        [SerializeField] private PlayerCharacterController _associatedCharacterPrefab;
        #endregion

        #region Components
        private Outline _outline;
        private RectTransform _rectTransform;
        #endregion

        #region Properties
        public PlayerCharacterController AssociatedCharacterPrefab
        {
            get { return _associatedCharacterPrefab; }
        }

        public float Width { get { return _rectTransform.rect.width; } }

        public bool IsFocus { get; private set; }

        #endregion

        #region Internal
        private Color _previousOutLineColor;
        #endregion

        private void Awake()
        {
            //get components.
            _outline = GetComponent<Outline>();
            _rectTransform = GetComponent<RectTransform>();

            UnFocus();
        }

        public void Focus(Color focusOutlineColor)
        {
            if (IsFocus)
            {
                Debug.LogError("Tried to focus a headshot that was already focussed.");
                return;
            }

            _previousOutLineColor = _outline.effectColor;
            _outline.effectColor = focusOutlineColor;
            IsFocus = true;
        }

        public void UnFocus()
        {
            if (!IsFocus) return;

            _outline.effectColor = _previousOutLineColor;
            IsFocus = false;
        }
    }
}
