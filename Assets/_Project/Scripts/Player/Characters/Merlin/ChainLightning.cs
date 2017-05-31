using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Project.Scripts.Effects;

namespace _Project.Scripts.Player.Characters.Merlin
{
    public class ChainLightning : Effect
    {
        #region Editor Variables
        [SerializeField] private ChainBeamController _chainBeamControllerPrefab;

        [SerializeField] private float _totalDuration;
        [SerializeField] private float _timeBetweenChain;
        [SerializeField] private float _maxChainDistance;
        [SerializeField] private int _maxChainedExcaliburs;
        #endregion

        #region Internal Variables
        private readonly List<ChainBeamController> _chainBeams = new List<ChainBeamController>();
        #endregion

        public override bool IsValidTarget(GameObject gameObj)
        {
            return gameObj.GetComponentInChildren<StuckExcalibur>()      != null && //It must have a excalibur stuck in it.
                   gameObj.GetComponentInChildren<ChainBeamController>() == null;   //But can't already be beamed.
        }

        protected override void Apply(GameObject gameObj)
        {
            Invoke("Finish", _totalDuration);
            StartCoroutine(ChainThatLighting());
        }

        private IEnumerator ChainThatLighting()
        {
            List<StuckExcalibur> chainedExcaliburs = new List<StuckExcalibur>();

            StuckExcalibur currentExcalibur = transform.parent.GetComponentInChildren<StuckExcalibur>();
            StuckExcalibur previousExcalibur = currentExcalibur;

            while (true)
            {
                //Add to visited.
                chainedExcaliburs.Add(currentExcalibur);

                //Get the nearest other excalibur. If none are found, stop.
                StuckExcalibur nearest = currentExcalibur.GetNearestOtherExcalibur(previousExcalibur);
                if (nearest == null || !CloseEnoughTogether(currentExcalibur, nearest))
                    break;

                //Chain to the nearest.
                Chain(currentExcalibur, nearest);

                //If we've reached a loop or max chained, stop.
                if (chainedExcaliburs.Contains(nearest) || 
                    chainedExcaliburs.Count > _maxChainedExcaliburs)
                    break;

                //save for next iteration.
                previousExcalibur = currentExcalibur;
                currentExcalibur = nearest;

                //Wait before we chain further.
                yield return new WaitForSeconds(_timeBetweenChain);
            }
        }

        /// <summary>
        /// Spawns a chain beamController between the 2 excaliburs.
        /// </summary>
        /// <param name="from">source of lightning.</param>
        /// <param name="to">target of lightning.</param>
        private void Chain(StuckExcalibur from, StuckExcalibur to)
        {
            ChainBeamController beamController = Instantiate(_chainBeamControllerPrefab, from.transform);
            _chainBeams.Add(beamController);

            beamController.Activate(from.gameObject, to.gameObject);
        }

        private void Finish()
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            StopAllCoroutines();

            foreach (ChainBeamController beam in _chainBeams)
                if(beam != null)
                    Destroy(beam.gameObject);
            
        }

        /// <summary>
        /// Checks if 2 excaliburs are close enough together.
        /// </summary>
        /// <returns>true if close enough, false if not.</returns>
        private bool CloseEnoughTogether(StuckExcalibur excalibur1, StuckExcalibur excalibur2)
        {
            Vector2 position1 = excalibur1.transform.position;
            Vector2 position2 = excalibur2.transform.position;

            return Vector2.Distance(position1, position2) <= _maxChainDistance;
        }
    }
}
