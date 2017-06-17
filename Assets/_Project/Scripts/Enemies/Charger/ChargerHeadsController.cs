
using UnityEngine;
using PowerTools;
using _Project.Scripts.General;

namespace _Project.Scripts.Enemies.Charger
{
    public class ChargerHeadsController : CustomMonoBehaviour
    {

        #region Components

        private SpriteAnimNodes _animationNodes;
        private SpriteRenderer[] _headRenderers;

        #endregion

        private void Awake()
        {
            _animationNodes = GetComponentInParent<SpriteAnimNodes>();
            _headRenderers = GetComponentsInChildren<SpriteRenderer>();
        }

        private void LateUpdate()
        {
            for (int index = 0; index < _headRenderers.Length; index++)
                _headRenderers[index].transform.position = _animationNodes.GetPosition(index);
        }
    }
}
