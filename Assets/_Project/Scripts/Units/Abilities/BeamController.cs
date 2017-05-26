using UnityEngine;
using _Project.Scripts.Player;

namespace _Project.Scripts.Units.Abilities
{
    class BeamController : Ability
    {
        #region Editor Variables
        [SerializeField] private CrappySpriteBeam _beam;
        #endregion

        #region Other objects
        private ICharacterAimSource _aimSource;
        #endregion

        /// <summary>
        /// Starts the beam.
        /// </summary>
        /// <param name="aimSource"></param>
        public override void Do(ICharacterAimSource aimSource)
        {
            //Set the aim source.
            _aimSource = aimSource;

            //Manually update once, because otherwise the beam spawns around the source, only then turn the renderer on.
            Update();
            _renderer.enabled = true;
        }

        private void Update()
        {
            Vector2 sourcePosition = _aimSource.SourcePosition;
            Vector2 targetPosition = FindTargetPosition();
        }

        private Vector2 FindTargetPosition()
        {
            //Shoot a ray.
            Vector2 direction = MathHelper.DegreeToVector2(_aimSource.AimingDegree);
            float maxDistance = _beam.MaxDistance;

            RaycastHit2D hit = Physics2D.Raycast(_aimSource.SourcePosition, direction, maxDistance,
                _beam.LayerMask);

            //If we hit, return collided object.
            if (hit.collider != null)
                return hit.point;

            //calculate the position that is _maxdistance away in direction of this.position.
            direction.Normalize();
            Vector2 offset = direction * maxDistance;
            return (Vector2) transform.position + offset;

        }
    }
}
