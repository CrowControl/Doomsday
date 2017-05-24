using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using _Project.Scripts.Effects;
using _Project.Scripts.Units;
using _Project.Scripts.Units.Abilities;

namespace Assets._Project.Scripts.Effects
{
    class ExplosionEffect : IEffect<HealthController>
    {
        #region Editor Variables
        [SerializeField] private Ability _ExplosionPrefab;
        #endregion

        public override void SetTarget(HealthController target)
        {
            
        }
    }
}
