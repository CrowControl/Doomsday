using UnityEngine;
using _Project.Scripts.General;
using _Project.Scripts.Player;
using _Project.Scripts.Units;

namespace _Project.Scripts.Enemies
{
    [RequireComponent(typeof(Animator))]
    class CritterController : CustomMonoBehaviour, IEnemyAttackController
    {
        #region Variables

        #region Editor
        [SerializeField] private float _distance;
        [SerializeField] private float _cooldown;
        #endregion
        
        #region Properties

        public float Distance { get { return _distance; } }
        public float Cooldown { get { return _cooldown; } }

        #endregion

        #region Components

        private Animator _animator;

        #endregion

        #region Events

        //Called when the attack is finished.
        public event CustomMonoBehaviour.BehaviourEventHandler OnFinished;

        #endregion

        #endregion

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void StartAttack(PlayerCharacterController targetPlayer)
        {
            //todo
        }
    }
}
