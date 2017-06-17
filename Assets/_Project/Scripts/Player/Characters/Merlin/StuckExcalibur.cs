using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using _Project.Scripts.General;

namespace _Project.Scripts.Player.Characters.Merlin
{
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class StuckExcalibur : CustomMonoBehaviour
    {
        /// <summary>
        /// Gets the nearest other excalibur.
        /// </summary>
        /// <param name="excluded">excaliburs to ignore.</param>
        /// <returns>The nearest other excalibur</returns>
        public StuckExcalibur GetNearestOtherExcalibur(List<StuckExcalibur> excluded, float range)
        {
            //Exclude the excluded excaliburs. hehe.
            List<StuckExcalibur> otherExcaliburs = FindExcalibursInRange(range);
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

        public StuckExcalibur GetNearestOtherExcalibur(StuckExcalibur excluded, float range)
        {
            return GetNearestOtherExcalibur(new List<StuckExcalibur> { excluded }, range);
        }

        /// <summary>
        /// Gets the nearest other excalibur.
        /// </summary>
        /// <returns>The nearest other excalibur</returns>
        public StuckExcalibur GetNearestOtherExcalibur(float range)
        {
            return GetNearestOtherExcalibur(new List<StuckExcalibur>(), range);
        }

        private List<StuckExcalibur> FindExcalibursInRange(float range)
        {
            List<StuckExcalibur> excalibursInRange = new List<StuckExcalibur>();// = new List<StuckExcalibur>(_otherExcalibursInRange);

            Collider2D[] collidersInRange = Physics2D.OverlapCircleAll(transform.position, range, 1 << 15);
            foreach (Collider2D collider in collidersInRange)
            {
                StuckExcalibur excalibur = collider.gameObject.GetComponent<StuckExcalibur>();
                if (excalibur != null)
                    excalibursInRange.Add(excalibur);
            }

            excalibursInRange.Remove(this);
            return excalibursInRange;
        }
    }
}
