using UnityEngine;

namespace _Project
{
    public abstract class CustomMonoBehaviour : MonoBehaviour
    {
        public delegate void BehaviourEventHandler();
        public event BehaviourEventHandler OnDestroyed;

        protected virtual void OnDestroy()
        {
            if (OnDestroyed != null)
                OnDestroyed();
        }
    }
}
