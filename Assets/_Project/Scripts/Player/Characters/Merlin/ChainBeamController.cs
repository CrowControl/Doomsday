using System.Collections.Generic;
using UnityEngine;
using _Project.Scripts.General;
using _Project.Scripts.Units;
using _Project.Scripts.Units.Abilities;

namespace _Project.Scripts.Player.Characters.Merlin
{
    class ChainBeamController : CustomMonoBehaviour
    {
        #region Editor Variables
        //prefabs
        [SerializeField] private Beam _beamPrefab;
        [SerializeField] private BeamEnd _beamEndPrefab;

        //Variables.
        [SerializeField] private BeamEnd _source;
        [SerializeField] private BeamEnd _target;
        #endregion

        #region`Internal Variables
        private Beam _beam;
        private readonly List<CustomMonoBehaviour> _dependentBehaviours = new List<CustomMonoBehaviour>();
        #endregion

        /// <summary>
        /// Activates the beam component.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public void Activate(GameObject source, GameObject target)
        {
            //spawn the beam.
            _beam = Instantiate(_beamPrefab, transform);
            RegisterDependentBehaviour(_beam);

            //Set the endings for this beam.
            _source = RegisterBeamEnd(source);
            _target = RegisterBeamEnd(target);
            
            //Update once to initialize the beam's transform, and then turn it's behavour on.
            Update();
            _beam.Activate();
        }

        /// <summary>
        /// Registers a gameObject as one of the ends of this beam.
        /// </summary>
        /// <param name="gameObj">The Gameobject.</param>
        /// <returns>Reference to the beamEnd.</returns>
        private BeamEnd RegisterBeamEnd(GameObject gameObj)
        {
            //Add a beam end object as child of the object.
            BeamEnd beamEnd = Instantiate(_beamEndPrefab, gameObj.transform);
            //We don't want the beam to collide with the game Object.
            _beam.IgnoreCollisionWith(gameObj);
            //We want to know if the object, and with it the beamEnd is ever destroyed.
            RegisterDependentBehaviour(beamEnd);
            return beamEnd;
        }

        /// <summary>
        /// Registers a customMonobehaviour to this one. If any is destroyed, the rest is as well.
        /// </summary>
        /// <param name="behaviour">The behaviour.</param>
        private void RegisterDependentBehaviour(CustomMonoBehaviour behaviour)
        {
            behaviour.OnDestroyed += OnDependentBehaviourDestroyed;
            _dependentBehaviours.Add(behaviour);
        }

        private void Update()
        {
            if(_source != null && _target != null)
                _beam.UpdateTransform(_source.transform.position, _target.transform.position);
        }

        /// <summary>
        /// We want to know when a dependent object is detroyed.
        /// </summary>
        private void OnDependentBehaviourDestroyed()
        {
            Destroy(gameObject);
        }

        /// <summary>
        /// Called when the object is destroyed.
        /// </summary>
        protected override void OnDestroy()
        {
            //Destroys all dependent behaviours.
            foreach (CustomMonoBehaviour behaviour in _dependentBehaviours)
            {
                if (behaviour == null) continue;

                //We unsubscribe from their destroy events because that would create a loop.
                behaviour.OnDestroyed -= OnDependentBehaviourDestroyed;
                Destroy(behaviour.gameObject);
            }

            base.OnDestroy();
        }
    }

}
