using UnityEngine;

namespace _Project.Scripts.General
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
