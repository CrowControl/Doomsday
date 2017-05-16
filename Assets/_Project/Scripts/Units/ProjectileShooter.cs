using System.Collections;
using System.Collections.Generic;
using Assets._Project.Scripts.Shooting;
using UnityEngine;
using _Project.Scripts.Shooting;

public class ProjectileShooter : MonoBehaviour, IShooter
{
    #region Editor Variables
    //projectile.
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _projectileRange;

    //Cooldown
    [SerializeField] private float _cooldown;
    #endregion

    #region Internal Variables
    private bool _onCooldown;
    #endregion

    /// <summary>
    /// Shoots a projectile.
    /// </summary>
    /// <param name="direction"></param>
    public void Shoot(float directionAngle)
    {
        //check if we're on cooldown.
        if (_onCooldown) return;

        //Spawn a projectile.
        Projectile projectile = Instantiate(_projectilePrefab, transform.position,
            Quaternion.AngleAxis(directionAngle, Vector3.forward));
        
        //Make it move.
        projectile.Speed = _projectileSpeed;
        projectile.Range = _projectileRange;
        projectile.StartMoving();

        //start the cooldown.
        StartCooldown();
    }

    /// <summary>
    /// Starts the shooting cooldown.
    /// </summary>
    private void StartCooldown()
    {
        _onCooldown = true;
        Invoke("FinishCooldown", _cooldown);
    }

    /// <summary>
    /// Finishes the cooldown.
    /// </summary>
    private void FinishCooldown()
    {
        _onCooldown = false;
    }
}
