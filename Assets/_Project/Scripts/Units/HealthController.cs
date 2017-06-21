using UnityEngine;
using _Project.Scripts.General;

namespace _Project.Scripts.Units
{
    public class HealthController : CustomMonoBehaviour
    {
        #region Editor Variables
        [SerializeField] private float _startingHP;
        [SerializeField] private float _maxHP;

        [SerializeField] private bool _destroyOnDeath;
        #endregion

        #region Properties
        public float HP
        {
            get { return _hp; }
            set
            {
                //If the new value is lower, use the GetHit method.
                if (value < _hp)
                {
                    GetHit(_hp - value);
                    return;
                }

                //Assign it but keep it bellow max.
                _hp = value < _maxHP ? value : _maxHP; 
            }
        }
        #endregion

        #region Events
        public delegate void HitEventHandler(float damage);
        public delegate void DeathEventHandler();

        public event HitEventHandler OnHit;
        public event DeathEventHandler OnDeath;
        #endregion

        #region Internal Variables
        private float _hp;
        private bool _isDead;
        #endregion

        private void Awake()
        {
            HP = _startingHP;

            OnDeath += () => _isDead = true;

            if (_destroyOnDeath)
                OnDeath += Destroy;
        }

        public void GetHit(float damage)
        {
            //Apply damage.
            _hp -= damage;

            //Trigger death event if health reaches zero.
            if (_hp < 0 && OnDeath != null)
                OnDeath();

            //Trigger hit event.
            if (OnHit != null && !_isDead)
                OnHit(damage);
        }

        private void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
