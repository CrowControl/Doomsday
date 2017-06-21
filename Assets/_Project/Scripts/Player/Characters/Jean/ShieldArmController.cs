using System.Collections;
using UnityEngine;
using _Project.Scripts.Units;
using _Project.Scripts.Units.Abilities;

namespace _Project.Scripts.Player.Characters.Jean
{
    public class ShieldArmController : Ability
    {
        #region Editor
        [SerializeField] private Sprite _shieldArmSprite;
        #endregion

        #region Properties
        /// <summary>
        /// Duration of the ability.
        /// </summary>
        public float Duration { get; set; }
        #endregion

        #region Components

        private SpriteRenderer _armRenderer;        //Renders the arm sprite.
        private SpriteRenderer _shieldRenderer;     //Renders the shield.
        private Collider2D _shieldCollider;         //Collider of the shield.
        private HealthController _shieldHealth;     //Health component of shield.

        #endregion

        private Sprite _previousSprite;
        private void Awake()
        {
            //Get renderers.
            SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>(true);
            _armRenderer = renderers[0];    //arm renderer should be first because it's located on this gameobject.
            _shieldRenderer = renderers[1];  //shield renderer should be second as it's located on a child object.

            //Get shield collider.
            _shieldCollider = GetComponentInChildren<Collider2D>(true);

            //Get shield health and subscribe to shield hits.
            _shieldHealth = GetComponentInChildren<HealthController>();
            _shieldHealth.OnHit += OnShieldHit;
        }

        /// <summary>
        /// Use shield ability.
        /// </summary>
        public override void Activate(ICharacterAimSource aimSource)
        {
            base.Activate(aimSource);
            EnableShield();
            Invoke("DisableShield", Duration);
        }

        /// <summary>
        /// Enables The shield.
        /// </summary>
        public void EnableShield()
        {

            FMODUnity.RuntimeManager.PlayOneShot("event:/Joanne/Arm_shield", transform.position);

            //Set arm sprite.
            _previousSprite = _armRenderer.sprite;
            _armRenderer.sprite = _shieldArmSprite;

            //Enable child components.
            _shieldRenderer.enabled = true;
            _shieldCollider.enabled = true;
        }

        /// <summary>
        /// Disables all shield components.
        /// </summary>
        public void DisableShield()
        {

            //Reset to the old sprite.
            _armRenderer.sprite = _previousSprite;

            //Disable the shield components.
            _shieldRenderer.enabled = false;
            _shieldCollider.enabled = false;

            //Finish ability.
            Finish();
        }

        /// <summary>
        /// Called when the shield get's hit.
        /// We negate all damage, cuz our shield is strank.
        /// </summary>
        /// <param name="damage">amount of damage the shield got hit with.</param>
        private void OnShieldHit(float damage)
        {
            _shieldHealth.HP += damage;
        }
    }
}