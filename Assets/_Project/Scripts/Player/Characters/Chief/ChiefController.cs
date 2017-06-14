using System;
using System.Collections.Generic;
using InControl;
using UnityEngine;
using FMOD;
using FMOD.Studio;
using _Project.Scripts.Units.Abilities;


namespace _Project.Scripts.Player.Characters.Chief
{
    [RequireComponent(typeof(AbilitySpawner))]
    class ChiefController : PlayerCharacterController
    {
        #region Audio Variables
        [FMODUnity.EventRef]
        private EventInstance _flameThrower;
        private ATTRIBUTES_3D _attribute;
        #endregion

        #region Editor Variables
        //Ability prefabs.
        [SerializeField] private ShotBeamController _minigunBulletBeam;
        [SerializeField] private ShotBeamController _flameThrowerBeam;
        [SerializeField] private Ability _shotGunShotPrefab;
        [SerializeField] private Ability _onSwitchExplosionPrefab;

        //Cooldowns.
        [SerializeField] private float _abilityAfterSwitchCooldown;
        [SerializeField] private float _abilityAfterAbilityCooldown;
        #endregion

        #region Properties

        private AbilitySpawner _abilitySpawner;
        #endregion

        #region Properties
        //Currently selected ability.
        private ChiefAbility CurrentAbility { get { return _abilities[_abilityIndex]; } }
        #endregion

        #region Internal Variables
        private ChiefState _state;          //+Current logic state.

        private ChiefAbility[] _abilities;  //List of all chief's abilities.
        private int _abilityIndex;          //Index of the currently selected ability.
        #endregion

        /// <summary>
        /// Called when object first awakes.
        /// </summary>
        protected void Awake()
        {
            _abilitySpawner = GetComponent<AbilitySpawner>();

            //init audio event instance
            _flameThrower = FMODUnity.RuntimeManager.CreateInstance("event:/Chief/Main_attack");
            _flameThrower.set3DAttributes(_attribute);

            InitializeAbilities();
            TransitionTo(new NotShootingState());
        }

        /// <summary>
        /// Called every frame.
        /// Used to update logic state.
        /// </summary>
        protected override void HandleInput()
        {
            //Update current state.
            ChiefState nextState = _state.Update(this, Device);

            //Transition if necessary.
            if (nextState != null)
                TransitionTo(nextState);
        }

        /// <summary>
        /// Transitions the state to nextState.
        /// </summary>
        /// <param name="nextState">The state to transition to.</param>
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
        /// <summary>
        /// Initialize the abilities.
        /// </summary>
        private void InitializeAbilities()
        {
            _abilities = new ChiefAbility[3];

            _abilities[0] = new ChiefAbility(_minigunBulletBeam, ChiefAbilityType.Coninuous);
            _abilities[1] = new ChiefAbility(_flameThrowerBeam, ChiefAbilityType.Coninuous, _flameThrower, "Flamethrowing");
            _abilities[2] = new ChiefAbility(_shotGunShotPrefab, ChiefAbilityType.SinglePress);
        }

        /// <summary>
        /// Changes to the next ability.
        /// </summary>
        private void ChangeAbility()
        {
            //Increment index.
            _abilityIndex++;

            //Wrap around to zero if we've reached the end of the list.
            if (_abilityIndex >= _abilities.Length)
                _abilityIndex = 0;
        }

        private enum ChiefAbilityType
        {
            Coninuous,  //Abilities that stay until the button is released.
            SinglePress //Abilities that are fired by a single press, and don't care about any further input.
        }

        /// <summary>
        /// We wrap the abilities is a class that also saves what type of shooting the ability needs.
        /// </summary>
        private class ChiefAbility
        {
            private readonly Ability _ability;          //Prefab to spawn.
            private readonly ChiefAbilityType _type;    //Type of this ability.
            private readonly EventInstance _abilitySoundfile; //corresponding audiofile
            private readonly string _parameterName;

            public bool HandlesAudio { get { return _abilitySoundfile != null; } }

            public ChiefAbility(Ability ability, ChiefAbilityType type)
            {
                _ability = ability;
                _type = type;
            }
            public ChiefAbility(Ability ability, ChiefAbilityType type, EventInstance abilitySoundfile, string parameterName)
            {
                _ability = ability;
                _type = type;
                _abilitySoundfile = abilitySoundfile;
                _parameterName = parameterName;
            }

            public void StartSound()
            {
                _abilitySoundfile.start();
            }

            public void StartIdleSound()
            {
                _abilitySoundfile.setParameterValue(_parameterName, 0);
            }

            public void StartActiveSound()
            {
                _abilitySoundfile.setParameterValue(_parameterName, 1);
            }
                

            public void StopSound()
            {
                _abilitySoundfile.stop(STOP_MODE.IMMEDIATE);
            }

            /// <summary>
            /// The shooting state to transition to when we want to shoot this ability.
            /// </summary>
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
                            throw new ArgumentOutOfRangeException();    //Shouldn't be possible.
                    }
                }
            }
        }
        #endregion

        #region State Machine
        /// <summary>
        /// Base state for chief's state machine.
        /// </summary>
        private abstract class ChiefState
        {
            public virtual void Enter(ChiefController chief) {}

            public virtual ChiefState Update(ChiefController chief, InputDevice device)
            {
                //In most states, we want to check if weapon press is called.
                if (device.LeftTrigger.WasPressed)
                    return new ChangeWeaponState();

                return null;
            }

            public virtual void Exit(ChiefController chief) { }

        }

        /// <summary>
        /// State for when we're not shooting.
        /// </summary>
        private class NotShootingState : ChiefState
        {

            public override void Enter(ChiefController chief)
            {
                base.Enter(chief);

                ChiefAbility ability = chief.CurrentAbility;
                if(ability.HandlesAudio)
                    ability.StartIdleSound();
            }

            public override ChiefState Update(ChiefController chief, InputDevice device)
            {
                //Check for shot button pressed.
                if (device.RightTrigger.WasPressed)
                {
                    return chief.CurrentAbility.ShootingState;
                }

                //Else check for ability-switch.
                return base.Update(chief, device);
            }
        }

        #region Shooting states

        /// <summary>
        /// Base state for when Chief shoots.
        /// </summary>
        private abstract class ShootingState : ChiefState
        {
            private readonly Ability _abilityPrefab;    //Prefab of the ability to shoot.
            protected Ability AbilityInstance;          //Reference to the instance of the ability once it's spawned.
            protected bool AbilityFinished;             //Set to true when the ability finishes.
            /// <summary>
            /// The ability prefab needs to be passed when we Instantiate this state.
            /// </summary>
            /// <param name="abilityPrefab"></param>
            protected ShootingState(Ability abilityPrefab)
            {
                _abilityPrefab = abilityPrefab;
            }

            //Spawn the ability when the state is entered.
            public override void Enter(ChiefController chief)
            {
                ChiefAbility ability = chief.CurrentAbility;
                if (ability.HandlesAudio)
                    ability.StartActiveSound();

                chief._abilitySpawner.OnAbilitySpawnFinished += OnAbilityFinished;
                AbilityInstance = chief._abilitySpawner.Spawn(chief, _abilityPrefab);
            }

            public override void Exit(ChiefController chief)
            {
                ChiefAbility ability = chief.CurrentAbility;
                if (ability.HandlesAudio)
                    ability.StartIdleSound();

                chief._abilitySpawner.OnAbilitySpawnFinished -= OnAbilityFinished;
                base.Exit(chief);
            }

            private void OnAbilityFinished()
            {
                AbilityFinished = true;
            }
        }

        /// <summary>
        /// State for when we're shooting an ability that stays active until the button is released.
        /// </summary>
        private class ContinuousShootingState : ShootingState
        {
            public ContinuousShootingState(Ability abilityPrefab) : base(abilityPrefab) { }

            public override ChiefState Update(ChiefController chief, InputDevice device)
            {
                //Check for butoon released.
                if (device.RightTrigger.WasReleased)
                {
                    return new CooldownState(chief._abilityAfterAbilityCooldown);
                }

                //Changing weapon is still possible.
                return base.Update(chief, device);
            }
            public override void Exit(ChiefController chief)
            {
                base.Exit(chief);
                //Destroys the ability if it still exists.
                if(AbilityInstance != null)
                    Destroy(AbilityInstance.gameObject);
            }
        }

        /// <summary>
        /// State for when we're shooting an ability that just needs a quick press.
        /// </summary>
        private class SinglePressShootingState : ShootingState
        {
            //Base state spawns the ability.
            public SinglePressShootingState(Ability abilityPrefab) : base(abilityPrefab) { }

            //Start cooldown as soon as the ability finishes.
            public override ChiefState Update(ChiefController chief, InputDevice device)
            {
                return AbilityFinished ? new CooldownState(chief._abilityAfterAbilityCooldown) : null;
            }
        }
        #endregion

        /// <summary>
        /// State for changing weapons.
        /// </summary>
        private class ChangeWeaponState : ChiefState
        {
            private bool _shouldTransition;

            //Spawn the switch eplxosion.
            public override void Enter(ChiefController chief)
            {
                ChiefAbility ability = chief.CurrentAbility;
                if (ability.HandlesAudio)
                    ability.StopSound();

                //***************** oneshot voor wapenverandering *************8
                AbilitySpawner spawner = chief._abilitySpawner;
                spawner.OnAbilitySpawnFinished += () => _shouldTransition = true;

                spawner.Spawn(chief, chief._onSwitchExplosionPrefab);
            }

            //Wait until explosion finishes.
            public override ChiefState Update(ChiefController chief, InputDevice device)
            {
                return _shouldTransition ? new CooldownState(chief._abilityAfterSwitchCooldown) : null;
            }

            //Change to the next ability.
            public override void Exit(ChiefController chief)
            {
                chief.ChangeAbility();

                ChiefAbility ability = chief.CurrentAbility;
                if (ability.HandlesAudio)
                    ability.StartSound();
            }
        }

        /// <summary>
        /// State for when chief is on cooldown after using an ability.
        /// </summary>
        private class CooldownState : ChiefState
        {
            private readonly float _duration;   //Length of the cooldown.
            private float _cooldownEndTime;     //Time that the cooldown ends.

            public CooldownState(float duration)
            {
                _duration = duration;
            }

            //Determine end time.
            public override void Enter(ChiefController chief)
            {
                _cooldownEndTime = Time.time + _duration;
            }

            //wait until time runs out.
            public override ChiefState Update(ChiefController chief, InputDevice device)
            {
                return Time.time >= _cooldownEndTime ? new NotShootingState() : null;
            }
        }

        #endregion
    }

}
