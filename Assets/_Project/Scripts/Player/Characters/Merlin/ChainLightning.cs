using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Project.Scripts.Effects;

namespace _Project.Scripts.Player.Characters.Merlin
{
    public class ChainLightning : Effect
    {
        #region Editor Variables
        [SerializeField] private ChainBeam _chainBeamPrefab;

        [SerializeField] private float _totalDuration;
        [SerializeField] private float _timeBetweenChain;
        [SerializeField] private float _maxChainDistance;
        [SerializeField] private int _maxChainedExcaliburs;
        #endregion

        #region Internal Variables
        private readonly List<ChainBeam> _chainBeams = new List<ChainBeam>();
        #endregion

        public override bool HasTargetComponent(GameObject gameObj)
        {
            return gameObj.GetComponentInChildren<StuckExcalibur>() != null;
        }

        protected override void Apply(GameObject gameObj)
        {
            Invoke("Finish", _totalDuration);
            StartCoroutine(ChainThatLighting());
        }

        private IEnumerator ChainThatLighting()
        {
            StuckExcalibur currentExcalibur = GetComponentInChildren<StuckExcalibur>();
            StuckExcalibur previousExcalibur = currentExcalibur;
            List<StuckExcalibur> chainedExcaliburs = new List<StuckExcalibur>();

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
        /// Spawns a chain beam between the 2 excaliburs.
        /// </summary>
        /// <param name="from">source of lightning.</param>
        /// <param name="to">target of lightning.</param>
        private void Chain(StuckExcalibur from, StuckExcalibur to)
        {
            ChainBeam beam = Instantiate(_chainBeamPrefab, from.transform);
            _chainBeams.Add(beam);

            beam.Activate(from.transform, to.transform);
        }

        private void Finish()
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            StopAllCoroutines();

            foreach (ChainBeam beam in _chainBeams)
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
