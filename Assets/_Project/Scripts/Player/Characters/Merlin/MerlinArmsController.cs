using UnityEngine;

namespace _Project.Scripts.Player.Characters.Merlin
{
    class MerlinArmsController : ArmsController
    {
        protected override Arm InititializeArm(Transform armTransform, bool rotateToAim)
        {
            return new MerlinArm(armTransform, rotateToAim);
        }

        private class MerlinArm : Arm
        {
            public MerlinArm(Transform transform, bool canRotate = true) 
                : base(transform, canRotate){ }

            protected override void FlipArm(bool flip)
            {
                foreach (SpriteRenderer renderer in _renderers)
                {
                    renderer.flipY = !flip;
                    renderer.flipX = true;
                }
            }
        }
    }
}
