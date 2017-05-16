using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private static Vector2[] _playerUIPositionTranslations =
    {
        new Vector2(-1, 1),
        new Vector2(1, 1),
        new Vector2(-1, -1),
        new Vector2(1, -1)
    };

    #endregion

    public void SpawnCharacterSelectUI(int playerIndex)
    {
        //todo Spawn UI
    }
}
