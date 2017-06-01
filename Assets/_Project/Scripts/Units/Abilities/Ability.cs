using UnityEngine;
using _Project.Scripts.General;
using _Project.Scripts.Player;

namespace _Project.Scripts.Units.Abilities
{
    public class Ability : CustomMonoBehaviour
    {
        public event BehaviourEventHandler OnNoLongerOccuppiesCaster;
        private bool _finished;

        public virtual void Do(ICharacterAimSource aimSource) { }

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