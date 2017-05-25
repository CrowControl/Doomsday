using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D)),
 RequireComponent(typeof(CapsuleCollider2D))]
public class StuckExcalibur : MonoBehaviour
{
    #region Internal Variables
    private List<StuckExcalibur> _otherExcalibursInRange = new List<StuckExcalibur>();
    #endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        StuckExcalibur otherExcalibur = other.GetComponentInChildren<StuckExcalibur>();
        if(otherExcalibur != null)
            _otherExcalibursInRange.Add(otherExcalibur);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        StuckExcalibur otherExcalibur = other.GetComponentInChildren<StuckExcalibur>();
        if (otherExcalibur != null)
            _otherExcalibursInRange.Remove(otherExcalibur);
    }

    public StuckExcalibur GetNearestOtherExcalibur(List<StuckExcalibur> excluded)
    {
        //Exclude the excluded excaliburs. hehe.
        List<StuckExcalibur> otherExcaliburs = new List<StuckExcalibur>(_otherExcalibursInRange);
        if (excluded != null)
            foreach (StuckExcalibur excludedExcalibur in excluded)
                otherExcaliburs.Remove(excludedExcalibur);

        StuckExcalibur nearestExcalibur = otherExcaliburs.First();
        float nearestDistance = float.MaxValue;

        //Get the nearest distance.
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
}
