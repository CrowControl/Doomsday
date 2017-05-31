using System.Collections.Generic;
using UnityEngine;
using _Project.Scripts.Player;

namespace _Project.Scripts.Units.Abilities
{
    [RequireComponent(typeof(Beam))]
    public class ShotBeamController : Ability
    {
        #region Editor Variables.
        [SerializeField] private List<int> _layersToHit;    //Indices of layers this beam should hit. Sorry for this.
        [SerializeField] private float _maxDistance;        //max distance of this beam.
        #endregion

        #region Components
        private Beam _beam;
        #endregion

        #region Internal Variables

        private Vector2 _sourcePosition, _targetPosition;   //The to ends of this beam.
        private ICharacterAimSource _aimSource;             //Object to get the aiming direction from.
        #endregion

        private void Awake()
        {
            InitializeComponents();
            _sourcePosition = transform.position;
        }

        /// <summary>
        /// Initializes all necessary components.
        /// </summary>
        private void InitializeComponents()
        {
            _beam = GetComponent<Beam>();
        }

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
            _beam.Activate();
        }

        #region Update
        private void Update()
        {
            //Move with the shooter.
            _sourcePosition = _aimSource.SourcePosition;
            _targetPosition = FindTargetPosition();

            _beam.UpdateTransform(_sourcePosition, _targetPosition);
        }

        /// <summary>
        /// Update the target position with raycasting.
        /// </summary>
        private Vector2 FindTargetPosition()
        {
            //Shoot a ray.
            Vector2 direction = MathHelper.DegreeToVector2(_aimSource.AimingDegree);
            int layerMask = MathHelper.GenerateHitLayerMask(_layersToHit);
            RaycastHit2D hit = Physics2D.Raycast(_sourcePosition, direction, _maxDistance, layerMask);

            //If we hit, return collided object.
            if (hit.collider != null)
                return hit.point;
            
            //calculate the position that is _maxdistance away in direction of this.position.
            direction.Normalize();
            Vector2 offset = direction * _maxDistance;
            return (Vector2) transform.position + offset;
        }
        #endregion
    }
}
