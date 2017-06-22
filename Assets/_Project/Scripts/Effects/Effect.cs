using System;
using UnityEngine;

namespace _Project.Scripts.Effects
{
    public abstract class Effect : MonoBehaviour
    {
        /// <summary>
        /// Tries to apply the given effect to the given game object. Succeeds if the game object has the component that the effect targets.
        /// </summary>
        /// <param name="effectPrefab">Effect prefab to apply.</param>
        /// <param name="gameObj">Game object to apply the effect to.</param>
        /// <returns>true if the effect was succesfully applied, false otherwise.</returns>
        public static bool TryApplyEffect(Effect effectPrefab, GameObject gameObj)
        {
            //Each effect has a target component. If the game object does not have this component, we do not continue.
            if (!effectPrefab.IsValidTarget(gameObj))
                return false;

            //Instantiate the prefab.
            Effect instantiatedEffect = Instantiate(effectPrefab, gameObj.transform);
            instantiatedEffect.Apply(gameObj);
            return true;
        }

        public abstract bool IsValidTarget(GameObject gameObj);
        protected abstract void Apply(GameObject gameObj);
    }
}
