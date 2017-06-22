using UnityEngine;

namespace _Project.Scripts.General
{
    [RequireComponent(typeof(Collider2D))]
    public class TransformLimit : CustomMonoBehaviour
    {
        #region Editor

        [SerializeField] private TransformLimitType _type;
        [SerializeField] private float _value;

        #endregion

        #region Properties

        public TransformLimitType Type {get { return _type; }}
        public float Value {get { return _value; }}

        #endregion

        #region Collision Events

        private void OnTriggerEnter2D(Collider2D other)
        {
            TransformLimitManager manager = other.GetComponent<TransformLimitManager>();
            if (manager == null)
            {
                Debug.Log("A TransformLimit collider registered a collision-enter with an object that didn't have a limitManager.");
                return;
            }

            manager.AddCameraLimit(this);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            TransformLimitManager manager = other.GetComponent<TransformLimitManager>();
            if (manager == null)
            {
                Debug.Log("A TransformLimit collider registered a collision-exit with an object that didn't have a limitManager.");
                return;
            }

            manager.RemoveCameraLimit(this);
        }

        #endregion

        public enum TransformLimitType
        {
            LowX, 
            HighX,
            LowY,
            HighY
        }
    }
}
