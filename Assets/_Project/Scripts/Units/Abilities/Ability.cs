using UnityEngine;
using _Project.Scripts.General;
using _Project.Scripts.Player;

namespace _Project.Scripts.Units.Abilities
{
    public class Ability : CustomMonoBehaviour
    {
        #region Editor
        [SerializeField] private bool _spawnAsChild;
        #endregion

        #region Events
        public event BehaviourEventHandler OnNoLongerOccuppiesCaster, OnActivated;
        #endregion

        #region Properties
        public bool SpawnAsChild { get { return _spawnAsChild; } }
        #endregion

        #region Internal
        private bool _finished;
        #endregion

        public virtual void Activate(ICharacterAimSource aimSource)
        {
            if (OnActivated != null)
                OnActivated();
        }

        protected void RotateTowardAim(ICharacterAimSource aimSource)
        {
            transform.rotation = Quaternion.AngleAxis(aimSource.AimingDegree, Vector3.forward);
        }

        protected void Finish()
        {
            if (_finished) return;

            if (OnNoLongerOccuppiesCaster != null)
                OnNoLongerOccuppiesCaster();

            _finished = true;
        }

        protected override void OnDestroy()
        {
            Finish();
            base.OnDestroy();
        }
    }
    
}