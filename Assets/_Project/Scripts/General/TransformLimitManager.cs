using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Scripts.General
{
    public class TransformLimitManager : CustomMonoBehaviour
    {
        #region Internal

        private readonly TransformLimitController _lowXLimits = new TransformLimitController(TransformLimit.TransformLimitType.LowX);
        private readonly TransformLimitController _highXLimits = new TransformLimitController(TransformLimit.TransformLimitType.HighX);
        private readonly TransformLimitController _lowYLimits = new TransformLimitController(TransformLimit.TransformLimitType.LowY);
        private readonly TransformLimitController _highYLimits = new TransformLimitController(TransformLimit.TransformLimitType.HighY);

        #endregion

        public void ApplyLimits(Transform target)
        {
            //Get current value.
            Vector3 position = target.position;

            //Apply x limits.
            float x = _lowXLimits.Apply(position.x);
            x = _highXLimits.Apply(x);

            //Apply y limits.
            float y = _lowYLimits.Apply(position.y);
            y = _highYLimits.Apply(y);

            //Set new value.
            target.position = new Vector3(x, y, position.z);
        }

        /// <summary>
        /// Adds a camera limit.
        /// </summary>
        /// <param name="limit">The limit.</param>
        public void AddCameraLimit(TransformLimit limit)
        {
            TransformLimitController controller = GetCameraController(limit.Type);
            controller.AddLimit(limit.Value);
        }

        /// <summary>
        /// Removes a camera limit.
        /// </summary>
        /// <param name="limit">The limit.</param>
        public void RemoveCameraLimit(TransformLimit limit)
        {
            TransformLimitController controller = GetCameraController(limit.Type);
            controller.RemoveLimit(limit.Value);
        }

        /// <summary>
        /// Gets the limitController corresponding to the given type.
        /// </summary>
        /// <param name="type">the type.</param>
        /// <returns>The controller.</returns>
        private TransformLimitController GetCameraController(TransformLimit.TransformLimitType type)
        {
            switch (type)
            {
                case TransformLimit.TransformLimitType.LowX:
                    return _lowXLimits;

                case TransformLimit.TransformLimitType.HighX:
                    return _highXLimits;
                    
                case TransformLimit.TransformLimitType.LowY:
                    return _lowYLimits;

                default:
                    return _highYLimits;
            }
        }

        /// <summary>
        /// Controls either the upper or lower limit of a single axis.
        /// </summary>
        private class TransformLimitController
        {
            private readonly List<float> _limits = new List<float>();

            private float _activeLimit;
            private bool IsActive { get { return _limits.Count > 0; } }

            private delegate float LimitDelegate(List<float> limits);
            private delegate float ApplyDelegate(float value, float limit);

            private readonly LimitDelegate _chooseActiveLimit;
            private readonly ApplyDelegate _applyInternal;

            public TransformLimitController(TransformLimit.TransformLimitType type)
            {
                if (type == TransformLimit.TransformLimitType.LowX || type == TransformLimit.TransformLimitType.LowY)
                {
                    _chooseActiveLimit = (limits) => limits.Min();
                    _applyInternal = MathHelper.MinClamp;
                }
                else
                {
                    _chooseActiveLimit = (limits) => limits.Max();
                    _applyInternal = MathHelper.Clamp;
                }
            }

            /// <summary>
            /// Apply the limits to the given value.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <returns>The limited value.</returns>
            public float Apply(float value)
            {
                return IsActive ? _applyInternal(value, _activeLimit) : value;
            }

            /// <summary>
            /// Adds a limit to this controller.
            /// </summary>
            /// <param name="limitValue"></param>
            public void AddLimit(float limitValue)
            {
                _limits.Add(limitValue);
                UpdateActiveLimit();
            }

            public void RemoveLimit(float limitValue)
            {
                //Remove.
                _limits.Remove(limitValue);
                
                //If value was the active limit, choose new.
                if(_activeLimit == limitValue)
                    UpdateActiveLimit();
            }

            private void UpdateActiveLimit()
            {
                _activeLimit = _chooseActiveLimit(_limits);
            }
        }
    }
}
