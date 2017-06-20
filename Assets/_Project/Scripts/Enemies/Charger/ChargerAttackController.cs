using System.Collections;
using UnityEngine;
using _Project.Scripts.General;
using _Project.Scripts.Player;
using _Project.Scripts.Units;

namespace _Project.Scripts.Enemies.Charger
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(MovementController))]
    public class ChargerAttackController : CustomMonoBehaviour, IEnemyAttackController
    {
        #region Editor
        [SerializeField] private float _chargeDistance;
        [SerializeField] private float _chargeDuration;
        [SerializeField] private float _speed;

        [SerializeField] private float _willAttackDistance;
        [SerializeField] private float _cooldown;
        #endregion

        #region Components
   
        private Animator _animator;
        private MovementController _movementController;
        #endregion

        #region Events
        public event BehaviourEventHandler OnFinished;
        #endregion

        #region Properties
        public float Distance { get { return _willAttackDistance; } }
        public float Cooldown { get { return _cooldown; } }
        #endregion

        #region Internal
        private Vector2 _chargeDirection;

        //Variables to sav old movementController state in.
        private IMovementInputSource _previousMovementSource;
        private float _previousSpeed;
        #endregion

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _movementController = GetComponent<MovementController>();
        }


        public void StartAttack(PlayerCharacterController targetPlayer)
        {
            _chargeDirection = targetPlayer.transform.position - transform.position;
            _chargeDirection.Normalize();

            _animator.SetTrigger("Attack");
        }

        private void StartCharge()
        {
            _previousMovementSource = _movementController.MovementInputSource;
            _previousSpeed = _movementController.Speed;

            _movementController.MovementInputSource = new MovementInputSource(_chargeDirection);
            _movementController.Speed = _speed;

            StartCoroutine(UpdateCharge());
        }

        private IEnumerator UpdateCharge()
        {
            //Set initial position.
            Vector2 previousPosition = transform.position;

            //Starting values of track keeping variables.
            float elapsedTime = 0;
            float traveledDistance = 0;

            while (elapsedTime < _chargeDuration && traveledDistance < _chargeDistance)
            {
                yield return null;
                //Update time tracking.
                elapsedTime += Time.deltaTime;

                //Update distance tracking.
                Vector2 position = transform.position;
                traveledDistance += Vector2.Distance(previousPosition, position);
                previousPosition = position;
            }

            _animator.SetTrigger("FinishCharge");
        }


        private void Finish()
        {
            RestoreMovementController();

            if (OnFinished != null)
                OnFinished();
        }

        private void RestoreMovementController()
        {
            _movementController.MovementInputSource = _previousMovementSource;
            _movementController.Speed = _previousSpeed;
        }
    }
}
