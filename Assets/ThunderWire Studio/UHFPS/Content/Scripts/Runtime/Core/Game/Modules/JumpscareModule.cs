using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UHFPS.Tools;

namespace UHFPS.Runtime
{
    public class JumpscareModule : ManagerModule
    {
        public struct JumpscareParams
        {
            public float amplitudeGain;
            public float frequencyGain;
            public float wobbleTime;
            public float sanityDuration;
        }

        public override string ToString() => "Jumpscare";

        [Header("Wobble")]
        public float WobbleGain = 1f;
        public float WobbleLoss = 0.5f;

        [Header("Sanity")]
        public float SanityGain = 1f;
        public float SanityLoss = 0.5f;

        private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
        private Image sanityOverlay;

        private float scareWeight;
        private float wobbleTimer;
        private float sanityTimer;

        public override void OnAwake()
        {
            sanityOverlay = GameManager.SanityOverlay;
            cinemachineBasicMultiChannelPerlin = PlayerPresence.PlayerVirtualCamera.
                GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        public void ApplyJumpscareEffect(JumpscareParams jumpscareParams)
        {
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = jumpscareParams.amplitudeGain;
            cinemachineBasicMultiChannelPerlin.m_FrequencyGain = jumpscareParams.frequencyGain;

            wobbleTimer = jumpscareParams.wobbleTime;
            sanityTimer = jumpscareParams.sanityDuration;
        }

        public void ApplyJumpscareEffect(JumpscareTrigger jumpscareTrigger)
        {
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = jumpscareTrigger.WobbleAmplitudeGain;
            cinemachineBasicMultiChannelPerlin.m_FrequencyGain = jumpscareTrigger.WobbleFrequencyGain;

            wobbleTimer = jumpscareTrigger.WobbleDuration;
            sanityTimer = jumpscareTrigger.SanityDuration;
        }

        public override void OnUpdate()
        {
            float amplitude = cinemachineBasicMultiChannelPerlin.m_AmplitudeGain;

            if (wobbleTimer > 0)
            {
                wobbleTimer -= Time.deltaTime;
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.MoveTowards(amplitude, 0f, Time.deltaTime * WobbleGain);
            }
            else if(amplitude > 0f)
            {
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.MoveTowards(amplitude, 0f, Time.deltaTime * WobbleLoss);
            }

            if(sanityTimer > 0)
            {
                sanityTimer -= Time.deltaTime;
                scareWeight = Mathf.MoveTowards(scareWeight, 1f, Time.deltaTime * SanityGain);
            }
            else if(scareWeight > 0f)
            {
                scareWeight = Mathf.MoveTowards(scareWeight, 0f, Time.deltaTime * SanityLoss);
            }

            sanityOverlay.Alpha(1f * scareWeight);
        }
    }
}