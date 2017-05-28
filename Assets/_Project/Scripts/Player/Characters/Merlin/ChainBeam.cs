using UnityEngine;
using _Project.Scripts.Units.Abilities;

namespace _Project.Scripts.Player.Characters.Merlin
{
    [RequireComponent(typeof(Beam))]
    class ChainBeam : MonoBehaviour
    {
        #region Components
        private Beam _beam;
        #endregion

        #region Internal Variables
        private Transform _sourceTransform, _targetTransform;
        #endregion

        private void Awake()
        {
            _beam = GetComponent<Beam>();
        }

        public void Activate(Transform source, Transform target)
        {
            _sourceTransform = source;
            _targetTransform = target;
            
            Update();
            //todo: Make beam ignore source and target enemies.

            _beam.Activate();

        }

        private void Update()
        {
            _beam.UpdateTransform(_sourceTransform.position, _targetTransform.position);
        }
    }
}
