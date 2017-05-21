using UnityEngine;
using UnityEngine.UI;
using _Project.Scripts.Player;

namespace _Project.Scripts.UI.Character_Selection
{
    public class CharacterSelectHeadShot : MonoBehaviour
    {
        #region Editor Variables
        [SerializeField] private PlayerCharacterController _associatedCharacterPrefab;
        #endregion

        #region Components
        private Outline _outline;
        #endregion

        #region Properties
        public PlayerCharacterController AssociatedCharacterPrefab
        {
            get { return _associatedCharacterPrefab; }
        }
        #endregion

        private void Awake()
        {
            _outline = GetComponent<Outline>();
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
