using UnityEngine;

namespace _Project.Scripts.Player.Characters.Chief
{
    public class ChiefArmsController : ArmsController
    {
        [SerializeField] private float _aimDegreeAdjustment;

        protected override float AimingDegree
        {
            get { return base.AimingDegree + _aimDegreeAdjustment; }
        }
    }
}
