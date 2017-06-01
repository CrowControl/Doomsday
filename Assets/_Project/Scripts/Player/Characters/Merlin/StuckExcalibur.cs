using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using _Project.Scripts.General;

namespace _Project.Scripts.Player.Characters.Merlin
{
    [RequireComponent(typeof(CircleCollider2D)),    //Used as range to find other excaliburs in.
     RequireComponent(typeof(CapsuleCollider2D))]   //This is so other excaliburs can detect this one.
    public class StuckExcalibur : CustomMonoBehaviour
    {
        #region Properties
        public int OtherExcalibursInRange { get { return _otherExcalibursInRange.Count; } }
        #endregion

        #region Internal Variables
        private readonly HashSet<StuckExcalibur> _otherExcalibursInRange = new HashSet<StuckExcalibur>();
        #endregion

        private void Awake()
        {
            GetComponent<CircleCollider2D>().isTrigger = true;
            GetComponent<CapsuleCollider2D>().isTrigger = false;
        }

        #region Range
        //Watches out for excaliburs entering in range.
        private void OnTriggerEnter2D(Collider2D other)
        {
            StuckExcalibur otherExcalibur = other.GetComponent<StuckExcalibur>();
            if(otherExcalibur != null && otherExcalibur != this)
                _otherExcalibursInRange.Add(otherExcalibur);
        }

        //Watches out for excaliburs extiting range.
        private void OnTriggerExit2D(Collider2D other)
        {
            StuckExcalibur otherExcalibur = other.GetComponent<StuckExcalibur>();
            if (otherExcalibur != null)
                _otherExcalibursInRange.Remove(otherExcalibur);
        }
        #endregion

        /// <summary>
        /// Gets the nearest other excalibur.
        /// </summary>
        /// <param name="excluded">excaliburs to ignore.</param>
        /// <returns>The nearest other excalibur</returns>
        public StuckExcalibur GetNearestOtherExcalibur(List<StuckExcalibur> excluded)
        {
            //Exclude the excluded excaliburs. hehe.
            List<StuckExcalibur> otherExcaliburs = new List<StuckExcalibur>(_otherExcalibursInRange);
            if (excluded != null)
                foreach (StuckExcalibur excludedExcalibur in excluded)
                    otherExcaliburs.Remove(excludedExcalibur);

            //If we have no valid neighbouring excaliburs left, return null.
            if (otherExcaliburs.Count <= 0)
                return null;

            //Get the nearest other excalibur.
            StuckExcalibur nearestExcalibur = otherExcaliburs.First();
            float nearestDistance = float.MaxValue;
            foreach (StuckExcalibur otherExcalibur in otherExcaliburs)
            {
                float distance = Vector2.Distance(transform.position, otherExcalibur.transform.position);

                if (distance < nearestDistance)
                {
                    nearestExcalibur = otherExcalibur;
                    nearestDistance = distance;
                }
            }

            return nearestExcalibur;
        }
        public StuckExcalibur GetNearestOtherExcalibur(StuckExcalibur excluded)
        {
            return GetNearestOtherExcalibur(new List<StuckExcalibur> { excluded });
        }

        /// <summary>
        /// Gets the nearest other excalibur.
        /// </summary>
        /// <returns>The nearest other excalibur</returns>
        public StuckExcalibur GetNearestOtherExcalibur()
        {
            return GetNearestOtherExcalibur(new List<StuckExcalibur>());
        }
    }
}
