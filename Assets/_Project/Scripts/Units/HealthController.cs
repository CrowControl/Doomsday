using UnityEngine;

namespace _Project.Scripts.Units
{
    class HealthController : MonoBehaviour
    {
        #region Editor Variables
        [SerializeField] private float _hp;
        #endregion

        #region Properties
        public float Hp
        {
            get { return _hp; }
            set { _hp = value; }
        }
        #endregion

        #region Events
        public delegate void HitEventHandler(float damage);
        public delegate void DeathEventHandler();

        public event HitEventHandler OnHit;
        public event DeathEventHandler OnDeath;
        #endregion

        public void GetHit(float damage)
        {
            //Apply damage.
            _hp -= damage;

            //Trigger hit event.
            if (OnHit != null)
                OnHit(damage);

            //Trigger death event if health reaches zero.
            if (_hp < 0 && OnDeath != null)
                OnDeath();
        }
    }
}
