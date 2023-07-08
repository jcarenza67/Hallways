using ThunderWire.Attributes;
using UnityEngine;

namespace UHFPS.Runtime
{
    [RequireComponent(typeof(HeadBobController))]
    [InspectorHeader("Camera Human Effects")]
    public class CameraHumanEffects : PlayerComponent
    {
        [Header("References")]
        public Transform DelayEffectTransform;
        public Transform HeadBobEffectTransform;

        [Header("Delay")]
        public float DelayAmount = 0.02f;
        public float MaxDelay = 0.03f;
        public float DelayTime = 0.1f;
        public bool Flip = false;

        [Header("HeadBob")]
        public float HeadBobAmount = 1f;
        public float BreathAmount = 1f;
        public float HeadBobTime = 0.1f;

        private HeadBobController headBob;

        private Vector3 defaultDelayPos;
        private Vector3 delayVelocity;
        private Vector3 headBobVelocity;

        private void Awake()
        {
            headBob = GetComponent<HeadBobController>();
            if (DelayEffectTransform != null)
                defaultDelayPos = DelayEffectTransform.localPosition;
        }

        private void Update()
        {
            UpdateDelayEffect();
            UpdateHeadBobEffect();
        }

        private void UpdateDelayEffect()
        {
            if (DelayEffectTransform == null)
                return;

            float factorX = LookController.DeltaInput.x;
            float factorY = LookController.DeltaInput.y;

            factorX *= DelayAmount * (Flip ? -1 : 1);
            factorY *= DelayAmount * (Flip ? -1 : 1);

            factorX = Mathf.Clamp(factorX, -MaxDelay, MaxDelay);
            factorY = Mathf.Clamp(factorY, -MaxDelay, MaxDelay);

            Vector3 final = new Vector3(defaultDelayPos.x + factorX, defaultDelayPos.y + factorY, defaultDelayPos.z);
            DelayEffectTransform.localPosition = Vector3.SmoothDamp(DelayEffectTransform.localPosition, final, ref delayVelocity, DelayTime);
        }

        private void UpdateHeadBobEffect()
        {
            float blend = headBob.BreathBobBlend;
            Vector2 headBobWave = headBob.Wave * HeadBobAmount;
            Vector3 headBobFinal = new Vector3(headBobWave.x, headBobWave.y, 0f);
            Vector3 breathFinal = new Vector3(0f, headBob.Breath * BreathAmount, 0f);

            Vector3 breathOrBob = Vector3.Lerp(breathFinal, headBobFinal, blend);
            HeadBobEffectTransform.localPosition = Vector3.SmoothDamp(HeadBobEffectTransform.localPosition, breathOrBob, ref headBobVelocity, HeadBobTime);
        }
    }
}