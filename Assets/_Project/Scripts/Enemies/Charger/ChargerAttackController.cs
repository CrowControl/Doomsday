using UnityEngine;
using _Project.Scripts.General;
using _Project.Scripts.Player;

namespace _Project.Scripts.Enemies.Charger
{
    public class ChargerAttackController : CustomMonoBehaviour, IEnemyAttackController
    {
        #region Editor

        [SerializeField] private float _distance;
        [SerializeField] private float _cooldown;
        #endregion

        public event BehaviourEventHandler OnFinished;
        public float Distance { get { return _distance; } }
        public float Cooldown { get { return _cooldown; } }
        public void StartAttack(PlayerCharacterController targetPlayer)
        {
            throw new System.NotImplementedException();
        }
    }
}
