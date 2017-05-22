using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Project.Scripts.Player;

namespace _Project.Scripts.Units
{
    class CrappyBeamShooter : MonoBehaviour, IAbility
    {
        #region Editor Variables
        [SerializeField] private float _lifetime;       //Life time of this beam.
        [SerializeField] private float _maxDistance;    //Max distance this beam can reach.
        [SerializeField] private bool _useBeamLifeTime; //If this is true, the lifetime assigned to the beam prefab we're using is used in stead of this._lifetime.
        #endregion

        #region Events
        public event AbilityEventHandler OnFinished;
        #endregion

        #region Properties
        public CrappySpriteBeam BeamPrefab { get; set; }
        #endregion

        public void Do(ICharacterAimSource aimSource)
        {
            StartCoroutine(BeamUpdateCoRoutine(aimSource));
        }

        /// <summary>
        /// Manages the beam's transform for the duration of it's lifetime.
        /// </summary>
        /// <param name="aimSource"></param>
        /// <returns></returns>
        private IEnumerator BeamUpdateCoRoutine(ICharacterAimSource aimSource)
        {
            //Instantiate the beam.
            CrappySpriteBeam beam = Instantiate(BeamPrefab, transform);

            //Determine the duration of this beam.
            float lifetime = _useBeamLifeTime ? beam.LifeTime : this._lifetime;
            float endTime = Time.time + _lifetime;

            //The content of this wile loop is ran every Update until our time runs out. It then proceeds to the remaining code below the while loop.
            while (Time.time <= endTime)
            {
                beam.SourcePosition = transform.position;
                beam.TargetPosition = FindBeamTarget(beam, aimSource.AimingDegree);
                yield return null;
            }

            if (OnFinished != null) OnFinished();
            Destroy(beam.gameObject);
        }

        /// <summary>
        /// Finds the target point for the beam.
        /// </summary>
        /// <param name="beam">The beam.</param>
        /// <param name="directionAngle">The direction this beam is shooting.</param>
        /// <returns>The target position for the beam.</returns>
        private Vector2 FindBeamTarget(CrappySpriteBeam beam, float directionAngle)
        {
            //Shoot a ray.
            Vector2 direction = MathHelper.DegreeToVector2(directionAngle);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, _maxDistance, beam.LayerMask);

            //If we hit, return collided object.
            if (hit.collider != null)
                return hit.point;

            //calculate the position that is _maxdistance away in direction of this.position.
            direction.Normalize();
            Vector2 offset = direction * _maxDistance;
            return (Vector2)transform.position + offset;
        }
    }
}
