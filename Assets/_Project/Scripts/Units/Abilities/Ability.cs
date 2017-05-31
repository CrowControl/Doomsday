using UnityEngine;
using _Project.Scripts.Player;

namespace _Project.Scripts.Units.Abilities
{
    public class Ability : CustomMonoBehaviour
    {
        public virtual void Do(ICharacterAimSource aimSource) { }
    }
    
}