using UnityEngine;

namespace _Project.Scripts.UI.Character_Selection
{
    [RequireComponent(typeof(RectTransform))]
    public class CharacterSelectUIManager : MonoBehaviour
    {
        #region Editor Variables
        [SerializeField] private CharacterSelectUI _characterSelectUIPrefab;
        [SerializeField] private Vector2 _characterselectUIPosition;
        #endregion

        #region Internal Variables
        /// <summary>
        /// Used to translate the position of the Character select UI, setting it to the appropriate place for each player.
        /// </summary>
        private static readonly Vector2[] PlayerUIPositionTranslations =
        {
            new Vector2(-1, 1),
            new Vector2(1, 1),
            new Vector2(-1, -1),
            new Vector2(1, -1)
        };

        #endregion

        /// <summary>
        /// Spawns a Character selection UI element.
        /// </summary>
        /// <param name="playerIndex">Index of the player that controls the input for this UI element.</param>
        /// <returns></returns>
        public CharacterSelectUI SpawnCharacterSelectUI(int playerIndex)
        {
            //Get the right position for this UI (UI's for different players go into different corners of the screen.
            Vector2 position = Vector2.Scale(_characterselectUIPosition, PlayerUIPositionTranslations[playerIndex]);

            //Spawn and set position.
            CharacterSelectUI charSelect = Instantiate(_characterSelectUIPrefab, transform);
            charSelect.transform.localPosition = position;

            return charSelect;
        }
    }
}
