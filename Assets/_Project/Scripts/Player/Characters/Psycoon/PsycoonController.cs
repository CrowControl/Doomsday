using System.Collections.Generic;
using Assets._Project.Scripts.Player.Characters.Psycoon;
using InControl;
using UnityEngine;
using _Project.Scripts.Units;

namespace _Project.Scripts.Player.Characters.Psycoon
{
    [RequireComponent(typeof(AuraSpawner)),
     RequireComponent(typeof(CrappyBeamShooter))]
    public class PsycoonController : PlayerCharacterController
    {
        #region Editor Variables
        [SerializeField] private float _chargePerSecond;
        [SerializeField] private float _chargeCooldown;

        //attack prefabs.
        [SerializeField] private List<OnCollisionEffectApplier> _damageAuraPrefabs;
        [SerializeField] private List<OnCollisionEffectApplier> _healAuraPrefabs;
        [SerializeField] private List<CrappySpriteBeam> _damageBeamPrefabs;
        [SerializeField] private List<CrappySpriteBeam> _healBeamPrefabs;
        #endregion

        #region Components
        private AuraSpawner _auraSpawner;
        private CrappyBeamShooter _crappyBeamShooter;
        #endregion

        #region Internal Variables
        private IPsycoonState _state;
        #endregion

        private void Awake()
        {
            _auraSpawner = GetComponent<AuraSpawner>();
            _crappyBeamShooter = GetComponent<CrappyBeamShooter>();

            ChangeState(new NotChargingState());
        }

        // Update is called once per frame
        protected override void Update ()
        {
            base.Update();

            IPsycoonState nextState = _state.Update(this, Device);
            if (nextState != null)
                ChangeState(nextState);
        }

        private void ChangeState(IPsycoonState newState)
        {
            if (_state != null)
                _state.Exit(this, Device);

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

            private float _charge;                          //Percentage of charge charged.
            protected InputControl MainButton, OtherButton; //Main button and off button. Differs on child classes.
            #endregion

            #region Properties
            protected int ChargeLevel { get { return (int) Mathf.Floor(_charge / 25); } }   //We only need to know in what plateau of charge we are.
            #endregion

            public void Enter(PsycoonController psycoon, InputDevice controller)
            {
                AssignButtons(controller);
                UpdateCharge(psycoon._chargePerSecond);
            }

            public IPsycoonState Update(PsycoonController psycoon, InputDevice controller)
            {
                if (MainButton.WasReleased)
                    return ButtonReleaseTransition(psycoon);

                if (OtherButton.WasPressed)
                    return OtherButtonPressedTransition(psycoon);

                return null;
            }

            public void Exit(PsycoonController psycoon, InputDevice controller){ }

            private void UpdateCharge(float chargePerSecond)
            {
                _charge += chargePerSecond * Time.deltaTime;
                MathHelper.Clamp(_charge, 100);
            }

            #region Abstract Methods
            protected abstract void AssignButtons(InputDevice controller);
            protected abstract IPsycoonState ButtonReleaseTransition(PsycoonController psycoon);
            protected abstract IPsycoonState OtherButtonPressedTransition(PsycoonController psycoon);
            #endregion
        }


        private class ChargingHealState : ChargingState
        {
            protected override void AssignButtons(InputDevice controller)
            {
                MainButton = controller.LeftTrigger;
                OtherButton = controller.RightTrigger;
            }

            protected override IPsycoonState ButtonReleaseTransition(PsycoonController psycoon)
            {
                //release a healing aura.
                AuraSpawner auraSpawner = psycoon._auraSpawner;
                auraSpawner.AuraPrefab = psycoon._healAuraPrefabs[ChargeLevel]; ;

                return new ChargeReleaseState(auraSpawner);
            }

            protected override IPsycoonState OtherButtonPressedTransition(PsycoonController psycoon)
            {
                //Release a Healing beam.
                CrappyBeamShooter beamShooter = psycoon._crappyBeamShooter;
                beamShooter.BeamPrefab = psycoon._healBeamPrefabs[ChargeLevel];

                return new ChargeReleaseState(beamShooter);
            }
        }

        private class ChargingDamageState : ChargingState
        {
            protected override void AssignButtons(InputDevice controller)
            {
                MainButton = controller.RightTrigger;
                OtherButton = controller.LeftTrigger;
            }

            protected override IPsycoonState ButtonReleaseTransition(PsycoonController psycoon)
            {
                //Release a damage beam.
                CrappyBeamShooter beamShooter = psycoon._crappyBeamShooter;
                beamShooter.BeamPrefab = psycoon._damageBeamPrefabs[ChargeLevel];

                return new ChargeReleaseState(beamShooter);
            }

            protected override IPsycoonState OtherButtonPressedTransition(PsycoonController psycoon)
            {
                //Release a Damage Aura.
                AuraSpawner auraSpawner = psycoon._auraSpawner;
                auraSpawner.AuraPrefab = psycoon._damageAuraPrefabs[ChargeLevel];

                return new ChargeReleaseState(auraSpawner);
            }
        }
        #endregion
        
        private class ChargeReleaseState : IPsycoonState
        {
            private readonly IAbility _abilityComponent;

            private bool _shouldTransition;

            public ChargeReleaseState(IAbility abilityComponent)
            {
                _abilityComponent = abilityComponent;
            }

            public void Enter(PsycoonController psycoon, InputDevice controller)
            {
                _abilityComponent.Do(psycoon);
                _abilityComponent.OnFinished += () => _shouldTransition = true;
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
