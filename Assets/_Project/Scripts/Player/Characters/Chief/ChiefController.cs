using System;
using System.Collections.Generic;
using InControl;
using UnityEngine;
using _Project.Scripts.Units.Abilities;

namespace _Project.Scripts.Player.Characters.Chief
{
    class ChiefController : PlayerCharacterController
    {
        #region Editor Variables
        //prefabs.
        [SerializeField] private ShotBeamController _minigunBulletBeam;
        [SerializeField] private ShotBeamController _flameThrowerBeam;
        [SerializeField] private Ability _shotGunShotPrefab;
        [SerializeField] private Ability _onSwitchExplosionPrefab;

        //Cooldowns.
        [SerializeField] private float _abilityAfterSwitchCooldown;
        [SerializeField] private float _abilityAfterAbilityCooldown;
        #endregion

        #region Properties
        private ChiefAbility CurrentAbility { get { return _abilities[_abilityIndex]; } }
        #endregion

        #region Internal Variables
        private ChiefState _state;

        private ChiefAbility[] _abilities;
        private int _abilityIndex;
        #endregion

        protected override void Awake()
        {
            InitializeAbilities();
            TransitionTo(new NotShootingState());

            base.Awake();
        }

        protected override void HandleInput()
        {
            ChiefState nextState = _state.Update(this, Device);
            if (nextState != null)
                TransitionTo(nextState);
        }

        private void TransitionTo(ChiefState nextState)
        {
            //Exit old state.
            if(_state != null)
                _state.Exit(this);

            //Enter new state
            _state = nextState;
            _state.Enter(this);
        }

        #region Chief ability Handling
        private void InitializeAbilities()
        {
            _abilities = new ChiefAbility[3];

            _abilities[0] = new ChiefAbility(_minigunBulletBeam, ChiefAbilityType.Coninuous);
            _abilities[1] = new ChiefAbility(_flameThrowerBeam, ChiefAbilityType.Coninuous);
            _abilities[2] = new ChiefAbility(_shotGunShotPrefab, ChiefAbilityType.SinglePress);
        }

        private void ChangeAbility()
        {
            _abilityIndex++;
            if (_abilityIndex >= _abilities.Length)
                _abilityIndex = 0;
        }

        private enum ChiefAbilityType
        {
            Coninuous,
            SinglePress
        }

        private class ChiefAbility
        {
            private readonly Ability _ability;
            private readonly ChiefAbilityType _type;

            public ChiefAbility(Ability ability, ChiefAbilityType type)
            {
                _ability = ability;
                _type = type;
            }

            public ShootingState ShootingState
            {
                get
                {
                    switch (_type)
                    {
                        case ChiefAbilityType.Coninuous:
                            return new ContinuousShootingState(_ability);
                        case ChiefAbilityType.SinglePress:
                            return new SinglePressShootingState(_ability);

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }
        #endregion

        #region State Machine
        private abstract class ChiefState
        {
            public virtual void Enter(ChiefController chief) { }

            public virtual ChiefState Update(ChiefController chief, InputDevice device)
            {
                if (device.LeftTrigger.WasPressed)
                    return new ChangeWeaponState();

                return null;
            }

            public virtual void Exit(ChiefController chief) { }

        }

        private class NotShootingState : ChiefState
        {
            public override ChiefState Update(ChiefController chief, InputDevice device)
            {
                if (device.RightTrigger.WasPressed)
                    return chief.CurrentAbility.ShootingState;

                return base.Update(chief, device);
            }
        }

        #region Shooting states

        private abstract class ShootingState : ChiefState
        {
            private readonly Ability _abilityPrefab;
            protected Ability AbilityInstance;

            protected ShootingState(Ability abilityPrefab)
            {
                _abilityPrefab = abilityPrefab;
            }

            public override void Enter(ChiefController chief)
            {
                AbilityInstance = chief.AbilitySpawner.Spawn(chief, _abilityPrefab);
            }
        }

        private class ContinuousShootingState : ShootingState
        {
            public ContinuousShootingState(Ability abilityPrefab) : base(abilityPrefab) { }

            public override ChiefState Update(ChiefController chief, InputDevice device)
            {
                if(device.RightTrigger.WasReleased)
                    return new CooldownState(chief._abilityAfterAbilityCooldown);

                return base.Update(chief, device);
            }
            public override void Exit(ChiefController chief)
            {
                Destroy(AbilityInstance.gameObject);
            }
        }

        private class SinglePressShootingState : ShootingState
        {
            public SinglePressShootingState(Ability abilityPrefab) : base(abilityPrefab) { }

            public override ChiefState Update(ChiefController chief, InputDevice device)
            {
                return new CooldownState(chief._abilityAfterAbilityCooldown);
            }
        }
        #endregion

        private class ChangeWeaponState : ChiefState
        {
            private bool _shouldTransition;

            public override void Enter(ChiefController chief)
            {
                AbilitySpawner spawner = chief.AbilitySpawner;
                spawner.OnAbilitySpawnFinished += () => _shouldTransition = true;

                spawner.Spawn(chief, chief._onSwitchExplosionPrefab);
            }

            public override ChiefState Update(ChiefController chief, InputDevice device)
            {
                return _shouldTransition ? new CooldownState(chief._abilityAfterSwitchCooldown) : null;
            }

            public override void Exit(ChiefController chief)
            {
                chief.ChangeAbility();
            }
        }

        private class CooldownState : ChiefState
        {
            private readonly float _duration;
            private float _cooldownEndTime;
            public CooldownState(float duration)
            {
                _duration = duration;
            }

            public override void Enter(ChiefController chief)
            {
                _cooldownEndTime = Time.time + _duration;
            }

            public override ChiefState Update(ChiefController chief, InputDevice device)
            {
                if(Time.time >= _cooldownEndTime)
                    return new NotShootingState();

                return null;
            }
        }

        #endregion
    }

}
