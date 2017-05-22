using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts
{
    internal static class MathHelper
    {
        #region Conversions
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

        #endregion

        #region Clamp
        public static float Clamp(float value, float minValue, float maxValue)
        {
            if (value < minValue) return minValue;
            if (value > maxValue) return maxValue;
            return value;
        }

        public static float Clamp(float value, float maxValue)
        {
            return Clamp(value, 0, maxValue);
        }
        #endregion

        /// <summary>
        /// Generates a layermask that makes a Raycast hit all layers that are passed.
        /// </summary>
        /// <param name="layerIndices">The indices of the layers that we want to include.</param>
        /// <returns>A layermask.</returns>
        public static int GenerateHitLayerMask(List<int> layerIndices)
        {
            if (layerIndices.Count < 1) return 0;

            int layerMask = 0;
            foreach (int layerIndex in layerIndices)
            {
                int layer = 1 << layerIndex;
                layerMask = layerMask | layer;
            }

            return layerMask;
        }
    }
}
