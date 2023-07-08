using System;
using UnityEngine;

namespace UHFPS.Runtime
{
    public class CustomInteractReticle : MonoBehaviour, IReticleProvider
    {
        public Reticle OverrideReticle;
        public Reticle HoldReticle;

        public bool DynamicHoldReticle;
        public ReflectionField DynamicHold;

        private void Awake()
        {
            if (DynamicHoldReticle) DynamicHold.Initialize();
        }

        public (Type, Reticle, bool) OnProvideReticle()
        {
            bool hold = DynamicHoldReticle && DynamicHold.Value;
            Reticle reticle = hold ? HoldReticle : OverrideReticle;
            return (null, reticle, hold);
        }
    }
}