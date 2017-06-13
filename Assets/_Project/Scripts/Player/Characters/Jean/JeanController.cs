using System;
using InControl;
using UnityEngine;
using _Project.Scripts.Units.Abilities;

namespace _Project.Scripts.Player.Characters.Jean
{
    public class JeanController : PlayerCharacterController
    {
        #region Editor Variables
        [SerializeField] private float _maxShieldTime = 4;
        [SerializeField] private float _cooldown = 0.2f;
        [SerializeField] private float _ShootingArmRotationDuringShield = -100;
        #endregion

        #region Components
        private ArmsController _arms;
        private PlayerSpriteHandler _spriteHandler;
        private AbilitySpawner _abilitySpawner;
        private ShieldArmController _shieldArm;
        #endregion

        #region Internal Variables
        private JeanState _state;
        #endregion

        protected void Awake()
        {
            GetComponents();

            //Start in idle state.
            TransitionTo(new IdleState());
        }

        private void GetComponents()
        {
            //Get components.
            _arms = GetComponent<ArmsController>();
            _spriteHandler = GetComponent<PlayerSpriteHandler>();

            //Get child components.
            _abilitySpawner = GetComponentInChildren<AbilitySpawner>();
            _shieldArm = GetComponentInChildren<ShieldArmController>();
        }

        protected override void HandleInput()
        {
            //Update state.
            JeanState newState = _state.Update(Device, this);

            //check for new state.
            if(newState != null)
                TransitionTo(newState);
        }

        /// <summary>
        /// transitions to the new state.
        /// </summary>
        /// <param name="newState">State to transition to.</param>
        private void TransitionTo(JeanState newState)
        {
            if (_state != null)
                _state.Exit(this);

            _state = newState;
            _state.Enter(this);
        }

        #region State Machine
        /// <summary>
        /// Base class for states in Jean's logic state machine.
        /// </summary>
        private abstract class JeanState
        {
            private float _enterTime;

            //Called when the state is entered.
            public virtual void Enter(JeanController jean)                                                    
            {
                _enterTime = Time.time;
            }    
            public abstract JeanState Update(InputDevice device, JeanController jean);  //Called every update.
            public virtual void Exit(JeanController jean) { }                           //Called when the state is exited.

            /// <summary>
            /// Time since this state was entered.
            /// </summary>
            protected float ElapsedTime { get { return Time.time - _enterTime; } }
        }

        /// <summary>
        /// State when Jean is not performing any abilities.
        /// </summary>
        private class IdleState : JeanState
        {
            public override JeanState Update(InputDevice device, JeanController jean)
            {
                if(device.RightTrigger.WasPressed)  //Shot pressed.
                    return new ShootingState();
                if (device.LeftTrigger.WasPressed)  //Shield button pressed.
                    return new ShieldState();

                return null;
            }
        }

        /// <summary>
        /// State when Jean is using her shield.
        /// </summary>
        private class ShieldState : JeanState
        {
            private ShieldArmController _shield;            //Reference to the shield.
            private float _enterTime;           //Time when this state was entered.
            private bool _shouldTransition;     //bool to check if we should transition.

            public override void Enter(JeanController jean)
            {
                base.Enter(jean);

                //Disable arm movement and sprite flipping.
                jean._spriteHandler.XFlippingEnabled = false;
                jean._arms.RotateShootingArmToAim = false;
                jean._arms.ShootingArmTransform.rotation = Quaternion.Euler(0, 0, jean._ShootingArmRotationDuringShield);

                //Use shield.
                _shield = jean._shieldArm;
                _shield.OnNoLongerOccuppiesCaster += OnShieldFinished;
                _shield.EnableShield();
            }

            public override JeanState Update(InputDevice device, JeanController jean)
            {
                //Start cooldown.
                if (device.LeftTrigger.WasReleased    ||    //Shield button released.
                    ElapsedTime > jean._maxShieldTime ||    //Max. shield use time reached.
                    _shouldTransition)                      //An event triggered the transition.
                    return new CooldownState(jean._cooldown);       

                return null;
            }

            public override void Exit(JeanController jean)
            {
                //Re-enable sprite flipping and arm movement.
                jean._spriteHandler.XFlippingEnabled = true;
                jean._arms.RotateShootingArmToAim = true;

                //Stop using shield.
                _shield.OnNoLongerOccuppiesCaster -= OnShieldFinished;
                _shield.DisableShield();
            }

            /// <summary>
            /// Called when the shield is destroyed.
            /// </summary>
            private void OnShieldFinished()
            {
                _shouldTransition = true;
            }
        }

        /// <summary>
        /// State when Jean is shooting a rocket.
        /// </summary>
        private class ShootingState : JeanState
        {
            private bool _shouldTransition;

            public override void Enter(JeanController jean)
            {
                base.Enter(jean);
                
                //Spawn rocket.
                jean._abilitySpawner.OnAbilitySpawnFinished += () => _shouldTransition = true;
                jean._abilitySpawner.Spawn(jean);
            }

            public override JeanState Update(InputDevice device, JeanController jean)
            {
                return _shouldTransition ? new CooldownState(jean._cooldown) : null;
            }
        }

        /// <summary>
        /// State when Jean is on cooldown after an ability was triggered.
        /// </summary>
        private class CooldownState : JeanState
        {
            private readonly float _duration;

            public CooldownState(float duration)
            {
                _duration = duration;
            }

            public override JeanState Update(InputDevice device, JeanController jean)
            {
                return ElapsedTime > _duration ? new IdleState() : null;
            }
           
        }
        #endregion
    }
}
