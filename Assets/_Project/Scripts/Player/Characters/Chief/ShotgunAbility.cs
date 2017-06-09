using UnityEngine;
using _Project.Scripts.General;
using _Project.Scripts.Units.Abilities;

namespace _Project.Scripts.Player.Characters.Chief
{
    class ShotgunAbility : Ability
    {
        [SerializeField] private float _offsetDistance;

        public override void Do(ICharacterAimSource aimSource)
        {
            //Set correct rotation.
            RotateTowardAim(aimSource);

            //Adjust position.
            Vector3 direction = MathHelper.DegreeToVector2(aimSource.AimingDegree);
            Vector3 offset = direction * _offsetDistance;
            transform.position += offset;

            base.Do(aimSource);
        }
    }
}
