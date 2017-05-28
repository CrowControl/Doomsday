using UnityEngine;

namespace _Project.Scripts.Units.Abilities
{
    [RequireComponent(typeof(SpriteRenderer)),
     RequireComponent(typeof(CapsuleCollider2D))]
    class Beam : MonoBehaviour
    {
        #region Editor Variables.
        [SerializeField] private float _distanceTweak;
        #endregion

        #region Components
        private SpriteRenderer _renderer;       //Component that renders the sprite.
        private CapsuleCollider2D _collider;    //Component that handles collision.
        #endregion

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _renderer.drawMode = SpriteDrawMode.Sliced;
            _renderer.enabled = false;

            _collider = GetComponent<CapsuleCollider2D>();
            _collider.direction = CapsuleDirection2D.Horizontal;
        }

        public void ActivateRendering()
        {
            _renderer.enabled = true;
        }

        /// <summary>
        /// Updates the transform.
        /// </summary>
        public void UpdateTransform(Vector2 source, Vector2 target)
        {
            //Set the position in the middle of the source and target.
            transform.position = Vector2.Lerp(source, target, 0.5f);

            //Rotate towards the target.
            float direction = MathHelper.Vector2Degree(target - source);
            transform.rotation = Quaternion.AngleAxis(direction, Vector3.forward);

            //Stretch the sprite accross the distance between the source and target.
            float distance = Vector2.Distance(source, target);
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
