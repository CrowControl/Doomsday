using UnityEngine;
using _Project.Scripts.General;
using _Project.Scripts.Player;

namespace _Project.Scripts.Enemies
{
    class Direction : ICharacterAimSource
    {
        public Direction(float aimingDegree, Vector2 sourcePosition)
        {
            AimingDegree = aimingDegree;
            SourcePosition = sourcePosition;
        }

        public Direction(Vector2 aimingVector, Vector2 sourcePosition)
        {
            aimingVector.Normalize();
            AimingDegree = MathHelper.Vector2Degree(aimingVector);
            SourcePosition = sourcePosition;
        }

        public float AimingDegree { get; private set; }
        public Vector2 SourcePosition { get; private set; }
    }
}
