using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    static class MathHelper
    {
        /// <summary>
        /// Converts a directional vector to an angle between -180 and 180 degrees.
        /// </summary>
        /// <param name="directionVector">The vector to convert.</param>
        /// <returns>The angle.</returns>
        public static float Vector2Degree(Vector2 directionVector)
        {
            return Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg;
        }

        #region Taken from http://answers.unity3d.com/questions/823090/equivalent-of-degree-to-vector2-in-unity.html
        public static Vector2 RadianToVector2(float radian)
        {
            return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
        }
        public static Vector2 RadianToVector2(float radian, float length)
        {
            return RadianToVector2(radian) * length;
        }
        public static Vector2 DegreeToVector2(float degree)
        {
            return RadianToVector2(degree * Mathf.Deg2Rad);
        }
        public static Vector2 DegreeToVector2(float degree, float length)
        {
            return RadianToVector2(degree * Mathf.Deg2Rad) * length;
        }
        #endregion
    }
}
