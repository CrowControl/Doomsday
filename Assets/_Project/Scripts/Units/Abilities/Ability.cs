using UnityEngine;
using _Project.Scripts.Player;

namespace _Project.Scripts.Units.Abilities
{
    public class Ability : MonoBehaviour
    {
        public event AbilityEventHandler OnFinished;
        public virtual void Do(ICharacterAimSource aimSource) { }

        private void OnDestroy()
        {
            if (OnFinished != null)
                OnFinished();
        }
    }

    public delegate void AbilityEventHandler();
}