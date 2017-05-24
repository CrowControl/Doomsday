using System.Collections.Generic;
using UnityEngine;
using _Project.Scripts.Player;

namespace _Project.Scripts.Units.Abilities
{
    [RequireComponent(typeof(SpriteRenderer)),
     RequireComponent(typeof(CapsuleCollider2D))]
    public class CrappySpriteBeam : Ability
    {
        #region Editor Variables.
        [SerializeField] private List<int> _layersToHit;    //Indices of layers this beam should hit. Sorry for this.
        [SerializeField] private float _maxDistance;        //max distance of this beam.

        //Only necessary for crappy stretched sprite implementation.
        [SerializeField] private float _distanceTweak;
        #endregion

        #region Components
        private SpriteRenderer _renderer;       //Component that renders the sprite.
        private CapsuleCollider2D _collider;    //Component that handles collision.
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
            _renderer = GetComponent<SpriteRenderer>();
            _renderer.drawMode = SpriteDrawMode.Sliced;

            _collider = GetComponent<CapsuleCollider2D>();
            _collider.direction = CapsuleDirection2D.Horizontal;
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
            _renderer.enabled = true;
        }

        #region Update
        private void Update()
        {
            //Move with the shooter.
            _sourcePosition = _aimSource.SourcePosition;

            UpdateTarget();
            UpdateTransform();
        }

        /// <summary>
        /// Update the target position with raycasting.
        /// </summary>
        private void UpdateTarget()
        {
            //Shoot a ray.
            Vector2 direction = MathHelper.DegreeToVector2(_aimSource.AimingDegree);
            int layerMask = MathHelper.GenerateHitLayerMask(_layersToHit);
            RaycastHit2D hit = Physics2D.Raycast(_sourcePosition, direction, _maxDistance, layerMask);

            //If we hit, return collided object.
            if (hit.collider != null)
                _targetPosition = hit.point;
            else
            {
                //calculate the position that is _maxdistance away in direction of this.position.
                direction.Normalize();
                Vector2 offset = direction * _maxDistance;
                _targetPosition = (Vector2) transform.position + offset;
            }
        }

        /// <summary>
        /// Updates the transform.
        /// </summary>
        private void UpdateTransform()
        {
            //Set the position in the middle of the source and target.
            transform.position = Vector2.Lerp(_sourcePosition, _targetPosition, 0.5f);

            //Rotate towards the target.
            float direction = MathHelper.Vector2Degree(_targetPosition - _sourcePosition);
            transform.rotation = Quaternion.AngleAxis(direction, Vector3.forward);

            //Stretch the sprite accross the distance between the source and target.
            float distance = Vector2.Distance(_sourcePosition, _targetPosition);
            UpdateSize(distance);
        }

        /// <summary>
        /// Uopdates the size in both the collider and the sprite renderer.
        /// </summary>
        /// <param name="distance"></param>
        private void UpdateSize(float distance)
        {
            Vector2 size = new Vector2(distance / _distanceTweak, _renderer.size.y);

            _renderer.size = size;
            _collider.size = size;
        }
        #endregion
    }
}
