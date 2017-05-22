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

        #endregion

        private void Awake()
        {
            //get components.
            _outline = GetComponent<Outline>();
            _rectTransform = GetComponent<RectTransform>();

            UnFocus();
        }

        public void Focus()
        {
            _outline.enabled = true;
        }

        public void UnFocus()
        {
            _outline.enabled = false;
        }
    }
}
