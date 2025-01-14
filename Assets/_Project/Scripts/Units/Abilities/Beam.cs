﻿using UnityEngine;
using _Project.Scripts.General;

namespace _Project.Scripts.Units.Abilities
{
    [RequireComponent(typeof(SpriteRenderer)),
     RequireComponent(typeof(CapsuleCollider2D))]
    class Beam : CustomMonoBehaviour
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

        public void Activate()
        {
            _renderer.enabled = true;
        }

        #region Update
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
            transform.localScale = new Vector2(size.x, transform.localScale.y); 
        }
        #endregion

        /// <summary>
        /// Makes the beam ignore collision with the given gameobject.
        /// </summary>
        /// <param name="gameObj">Game object to ignore.</param>
        public void IgnoreCollisionWith(GameObject gameObj)
        {
            Collider2D otherCollider = gameObj.GetComponent<Collider2D>();
            if (otherCollider != null)
                Physics2D.IgnoreCollision(_collider, otherCollider);
        }
    }
}
