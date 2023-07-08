using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UHFPS.Scriptable
{
    [CreateAssetMenu(fileName = "StrafeMovementGroup", menuName = "UHFPS/Player/Strafe Movement Group")]
    public class StrafeMovementGroup : PlayerStatesGroup
    {
        public float friction = 6f;
        public float groundAcceleration = 3f;
        public float airAcceleration = 1f;
        public float airAccelerationCap = 0.8f;

        [Header("Sliding")]
        public LayerMask slidingMask;
        public float slideRayLength = 1f;
        public float slopeLimit = 45f;
    }
}