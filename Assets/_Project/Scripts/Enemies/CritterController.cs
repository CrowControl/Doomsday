using UnityEngine;
using _Project.Scripts.Player;
using _Project.Scripts.Units;

namespace _Project.Scripts.Enemies
{
    [RequireComponent(typeof(MovementController))]
    [RequireComponent(typeof(Animator))]
    class CritterController : EnemyController
    {
        public override event BehaviourEventHandler OnAttackFinished;

        protected override void MoveTowardsTarget(Transform targetTransform)
        {
            throw new System.NotImplementedException();
        }

        protected override void AttackPlayer(PlayerCharacterController player)
        {
            throw new System.NotImplementedException();
        }
    }
}
