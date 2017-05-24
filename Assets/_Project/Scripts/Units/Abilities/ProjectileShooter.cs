using UnityEngine;
using _Project.Scripts.Player;

namespace _Project.Scripts.Units
{
    public class ProjectileShooter : MonoBehaviour, IAbility
    {
        #region Editor Variables
        //projectile.
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private float _projectileSpeed;
        [SerializeField] private float _projectileRange;
        #endregion

        #region Internal Variables
        private bool _onCooldown;
        #endregion

        public event AbilityEventHandler OnFinished;

        /// <summary>
        /// Shoots a projectile.
        /// </summary>
        /// <param name="aimsource">Source of aiming.</param>
        public void Do(ICharacterAimSource aimsource)
        {
            //check if we're on cooldown.
            if (_onCooldown) return;

            //Spawn a projectile.
            Projectile projectile = Instantiate(_projectilePrefab, transform.position,
                Quaternion.AngleAxis(aimsource.AimingDegree, Vector3.forward));
        
            //Make it move.
            projectile.Speed = _projectileSpeed;
            projectile.Range = _projectileRange;
            projectile.StartMoving();

            if (OnFinished != null)
                OnFinished();
        }
    }
}
