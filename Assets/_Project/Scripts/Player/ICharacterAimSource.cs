using UnityEngine;

namespace _Project.Scripts.Player
{


    public interface ICharacterAimSource
    {
        float AimingDegree { get; }     //Degree that this character is aiming at.
        Vector2 SourcePosition { get; }
    }

    public class ProxyAimSource : ICharacterAimSource
    {
        private readonly ICharacterAimSource _aimSource;
        private readonly Transform _transform;

        public ProxyAimSource(ICharacterAimSource aimSource, Transform transform)
        {
            _aimSource = aimSource;
            _transform = transform;
        }

        public float AimingDegree { get { return _aimSource.AimingDegree; } }
        public Vector2 SourcePosition { get { return _transform.position; } }
    }
}
