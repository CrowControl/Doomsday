using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Units
{
    [RequireComponent(typeof(SpriteRenderer)),
     RequireComponent(typeof(CapsuleCollider2D))]
    public class CrappySpriteBeam : MonoBehaviour
    {
        #region Editor Variables.
        [SerializeField] private float _distanceTweak;
        [SerializeField] private float _lifeTime;
        [SerializeField] private List<int> _layersToHit;    //Indices of layers this beam should hit. Sorry for this.
        #endregion

        #region Components
        private SpriteRenderer _renderer;
        private CapsuleCollider2D _collider;
        #endregion

        #region Properties
        public Vector2 SourcePosition { get; set; }
        public Vector2 TargetPosition { get; set; }
        public float LifeTime { get { return _lifeTime; } }
        public int LayerMask { get { return MathHelper.GenerateHitLayerMask(_layersToHit); } }
        #endregion

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _renderer.drawMode = SpriteDrawMode.Sliced;
            _renderer.enabled = false;

            _collider = GetComponent<CapsuleCollider2D>();
            _collider.direction = CapsuleDirection2D.Horizontal;
        }


        private void Start()
        {
            _renderer.enabled = true;
        }

        private void Update()
        {
            //Set the position in the middle of the source and target.
            transform.position = Vector2.Lerp(SourcePosition, TargetPosition, 0.5f);

            //Rotate towards the target.
            float direction = MathHelper.Vector2Degree(TargetPosition - SourcePosition);
            transform.rotation = Quaternion.AngleAxis(direction, Vector3.forward);

            //Stretch the sprite accross the distance between the source and target.
            float distance = Vector2.Distance(SourcePosition, TargetPosition);
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
    }
}
