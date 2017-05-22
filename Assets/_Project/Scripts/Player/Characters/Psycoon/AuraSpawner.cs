using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using _Project.Scripts.Player;
using _Project.Scripts.Player.Characters.Psycoon;
using _Project.Scripts.Units;

namespace Assets._Project.Scripts.Player.Characters.Psycoon
{
    class AuraSpawner : MonoBehaviour, IAbility
    {
        #region Properties
        public OnCollisionEffectApplier AuraPrefab { get; set; }
        #endregion

        public event AbilityEventHandler OnFinished;

        public void Do(ICharacterAimSource aimSource)
        {
            OnCollisionEffectApplier aura = Instantiate(AuraPrefab, transform);
            aura.OnFinished += OnAuraFinished;
        }

        private void OnAuraFinished()
        {
            if(OnFinished != null)
                OnFinished();
        }
    }
}
