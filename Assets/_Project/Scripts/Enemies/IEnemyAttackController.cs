using _Project.Scripts.General;
using _Project.Scripts.Player;

namespace _Project.Scripts.Enemies
{
    interface IEnemyAttackController
    {
        /// <summary>
        /// Event that's called when the attack is finished.
        /// </summary>
        event CustomMonoBehaviour.BehaviourEventHandler OnFinished;

       /// <summary>
       /// Distance from which the attack can be made.
       /// </summary>
        float Distance { get; }

        /// <summary>
        /// Cooldown of the attack.
        /// </summary>
        float Cooldown { get; }

        /// <summary>
        /// Performs the attack.
        /// </summary>
        /// <param name="targetPlayer">The player the attack targets.</param>
        void StartAttack(PlayerCharacterController targetPlayer);
    }

    public class EnemyAttackController : CustomMonoBehaviour
    {
        
    }
}
