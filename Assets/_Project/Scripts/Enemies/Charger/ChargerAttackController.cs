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

        #region Components
   
        private Animator _animator;
        #endregion

        #region Events
        public event BehaviourEventHandler OnFinished;
        #endregion

        #region Properties
        public float Distance { get { return _willAttackDistance; } }
        public float Cooldown { get { return _cooldown; } }
        #endregion

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void StartAttack(PlayerCharacterController targetPlayer)
        {
            Vector3 direction = transform.position - targetPlayer.transform.position;
            direction.Normalize();

            _animator.SetTrigger("Attack");
        }

        private void StartCharge()
        {
            //todo
        }

        private void Finish()
        {
            //todo

            if (OnFinished != null)
                OnFinished();
        }
    }
}
