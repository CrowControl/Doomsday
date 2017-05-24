using UnityEngine;

namespace _Project.Scripts.Effects
{
    public abstract class IEffect<T> : MonoBehaviour where T : MonoBehaviour
    {
        public abstract void SetTarget(T target);

        public static void ApplyEffect(IEffect<T> effectPrefab, T target) 
        {
            IEffect<T> effect = Instantiate(effectPrefab, target.transform);
            effect.SetTarget(target);
        }
    }
}
