using UnityEngine;
using _Project.Scripts.General;
using _Project.Scripts.Player;

namespace _Project.Scripts.Enemies.Charger
{
    public class ChargerAttackController : CustomMonoBehaviour, IEnemyAttackController
    {
        #region Editor

        [SerializeField] private float _chargeDistance;
        [SerializeField] private float _speed;

        [SerializeField] private float _willAttackDistance;
        [SerializeField] private float _cooldown;
        #endregion

        #region Events
        public event BehaviourEventHandler OnFinished;
        #endregion

        #region Properties
        public float Distance { get { return _willAttackDistance; } }
        public float Cooldown { get { return _cooldown; } }
        #endregion

        public void StartAttack(PlayerCharacterController targetPlayer)
        {
            Vector3 direction = transform.position - targetPlayer.transform.position;
            direction.Normalize();
        }
    }
}
