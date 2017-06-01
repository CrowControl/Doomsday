using System.Collections.Generic;
using InControl;
using UnityEngine;
using _Project.Scripts.General;
using _Project.Scripts.Units;
using _Project.Scripts.Units.Abilities;

namespace _Project.Scripts.Player.Characters
{
     [RequireComponent(typeof(AbilitySpawner))]
    public class PsycoonController : PlayerCharacterController
    {
        #region Editor Variables
        [SerializeField] private float _chargePerSecond;
        [SerializeField] private float _chargeCooldown;
        [SerializeField] private Color _maxChargeColor;

        //attack prefabs.
        [SerializeField] private List<Ability> _damageAuraPrefabs;
        [SerializeField] private List<Ability> _healAuraPrefabs;
        [SerializeField] private List<Ability> _damageBeamPrefabs;
        [SerializeField] private List<Ability> _healBeamPrefabs;
        #endregion

        #region Components
        private SpriteHandler _spriteHandler;
        #endregion

        #region Internal Variables
        private IPsycoonState _state;
        #endregion

        protected override void Awake()
        {
            //Get Components.
            base.Awake();
            _spriteHandler = GetComponent<SpriteHandler>();

            //Set starting state.
            TransitionTo(new NotChargingState());
        }

        protected override void HandleInput()
        {
            IPsycoonState nextState = _state.Update(this, Device);

            if (nextState != null)
                TransitionTo(nextState);
        }

        private void TransitionTo(IPsycoonState newState)
        {
            //Exit old state.
            if (_state != null)
                _state.Exit(this, Device);

            //Enter new state.
            _state = newState;
            _state.Enter(this, Device);
        }

        #region State Machine
        private interface IPsycoonState
        {
            void Enter(PsycoonController psycoon, InputDevice controller);
            IPsycoonState Update(PsycoonController psycoon, InputDevice controller);
            void Exit(PsycoonController psycoon, InputDevice controller);
        }

        private class NotChargingState : IPsycoonState
        {
            public void Enter(PsycoonController psycoon, InputDevice controller){}

            public IPsycoonState Update(PsycoonController psycoon, InputDevice controller)
            {
                if(controller.LeftTrigger.WasPressed)
                    return new ChargingHealState();
                if(controller.RightTrigger.WasPressed)
                    return new ChargingDamageState();

                return null;
            }

            public void Exit(PsycoonController psycoon, InputDevice controller){ }
        }

        #region Charging States
        /// <summary>
        /// Abstract state that defines the behaviour of charging states.
        /// </summary>
        private abstract class ChargingState : IPsycoonState
        {
            #region Internal Variables
            private Color _startingColor, _maxChargeColor;
            private SpriteHandler _spriteHandler;

            private float _charge;                          //Percentage of charge charged.
            protected InputControl MainButton, OtherButton; //Main button and off button. Differs on child classes.
            #endregion

            #region Properties
            private int ChargeLevel { get { return (int) Mathf.Floor(_charge / 25); } }   //We only need to know in what plateau of charge we are.
            protected List<Ability> ButtonReleaseAbilityPrefabs { private get; set; }
            protected List<Ability> OtherButtonPressAbilityPrefabs { private get; set; }
            #endregion

            public virtual void Enter(PsycoonController psycoon, InputDevice controller)
            {
                InitializeColorManagement(psycoon);
                AssignButtons(controller);
                AssignPrefabs(psycoon);

                UpdateCharge(psycoon._chargePerSecond);
            }

            public IPsycoonState Update(PsycoonController psycoon, InputDevice controller)
            {
                UpdateCharge(psycoon._chargePerSecond);
                UpdateColor(psycoon);

                if (MainButton.WasReleased)
                    return TransitionToChargeRelease(psycoon, ButtonReleaseAbilityPrefabs);

                if (OtherButton.WasPressed)
                    return TransitionToChargeRelease(psycoon, OtherButtonPressAbilityPrefabs);

                return null;
            }

            private IPsycoonState TransitionToChargeRelease(PsycoonController psycoon, List<Ability> abilityPrefabs)
            {
                AbilitySpawner abilitySpawner = psycoon.AbilitySpawner;
                abilitySpawner.AbilityPrefab = abilityPrefabs[ChargeLevel];

                return new ChargeReleaseState();
            }

            public void Exit(PsycoonController psycoon, InputDevice controller)
            {
                _spriteHandler.FadeToColor(_startingColor, 0.3f);
            }

            private void UpdateCharge(float chargePerSecond)
            {
                _charge += chargePerSecond * Time.deltaTime;
                _charge = MathHelper.Clamp(_charge, 100);
            }


            #region Color management
            private void InitializeColorManagement(PsycoonController psycoon)
            {
                _spriteHandler = psycoon._spriteHandler;

                _startingColor = _spriteHandler.Color;
                _maxChargeColor = psycoon._maxChargeColor;
            }

            private void UpdateColor(PsycoonController psycoon)
            {
                psycoon._spriteHandler.Color = Color.Lerp(_startingColor, _maxChargeColor, _charge / 100);
            }
            #endregion

            #region Abstract Methods
            protected abstract void AssignButtons(InputDevice controller);
            protected abstract void AssignPrefabs(PsycoonController psycoon);

            #endregion
        }

        private class ChargingHealState : ChargingState
        {
            protected override void AssignButtons(InputDevice controller)
            {
                MainButton = controller.LeftTrigger;
                OtherButton = controller.RightTrigger;
            }

            protected override void AssignPrefabs(PsycoonController psycoon)
            {
                ButtonReleaseAbilityPrefabs = psycoon._healAuraPrefabs;
                OtherButtonPressAbilityPrefabs = psycoon._healBeamPrefabs;
            }
        }

        private class ChargingDamageState : ChargingState
        {
            protected override void AssignButtons(InputDevice controller)
            {
                MainButton = controller.RightTrigger;
                OtherButton = controller.LeftTrigger;
            }

            protected override void AssignPrefabs(PsycoonController psycoon)
            {
                ButtonReleaseAbilityPrefabs = psycoon._damageBeamPrefabs;
                OtherButtonPressAbilityPrefabs = psycoon._damageAuraPrefabs;
            }
        }
        #endregion
        
        private class ChargeReleaseState : IPsycoonState
        {
            private bool _shouldTransition;

            public void Enter(PsycoonController psycoon, InputDevice controller)
            {
                psycoon.AbilitySpawner.OnFinished += () => _shouldTransition = true;
                psycoon.AbilitySpawner.Spawn(psycoon);
            }

            public IPsycoonState Update(PsycoonController psycoon, InputDevice controller)
            {
                return _shouldTransition ? new ChargeCooldownState() : null;
            }

            public void Exit(PsycoonController psycoon, InputDevice controller){ }
        }

        private class ChargeCooldownState : IPsycoonState
        {
            private float _cooldownEndTime;

            public void Enter(PsycoonController psycoon, InputDevice controller)
            {
                _cooldownEndTime = Time.time + psycoon._chargeCooldown;
            }

            public IPsycoonState Update(PsycoonController psycoon, InputDevice controller)
            {
                return Time.time >= _cooldownEndTime ? new NotChargingState() : null;
            }

            public void Exit(PsycoonController psycoon, InputDevice controller){ }
        }

        #endregion
    }
}
