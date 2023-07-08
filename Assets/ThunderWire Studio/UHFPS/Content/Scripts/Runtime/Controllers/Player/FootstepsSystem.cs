using UnityEngine;
using UHFPS.Scriptable;
using UHFPS.Tools;
using static UHFPS.Scriptable.SurfaceDetailsAsset;

namespace UHFPS.Runtime
{
    [RequireComponent(typeof(AudioSource))]
    public class FootstepsSystem : PlayerComponent
    {
        public enum FootstepStyleEnum { Timed, HeadBob, Animation }

        public SurfaceDetailsAsset SurfaceDetails;
        public FootstepStyleEnum FootstepStyle;
        public SurfaceDetectionEnum SurfaceDetection;
        public LayerMask FootstepsMask;

        public float StepPlayerVelocity = 0.1f;
        public float JumpStepAirTime = 0.1f;

        public float WalkStepTime = 1f;
        public float RunStepTime = 1f;
        public float LandStepTime = 1f;
        [Range(-1f, 1f)]
        public float HeadBobStepWave = -0.9f;

        [Range(0, 1)] public float WalkingVolume = 1f;
        [Range(0, 1)] public float RunningVolume = 1f;
        [Range(0, 1)] public float LandVolume = 1f;

        private AudioSource audioSource;
        private Collider surfaceUnder;

        private bool isWalking;
        private bool isRunning;

        private int lastStep;
        private int lastLandStep;

        private float stepTime;
        private bool waveStep;

        private float airTime;
        private bool wasInAir;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            surfaceUnder = FootstepsMask.CompareLayer(hit.gameObject.layer)
                ? hit.collider : null;
        }

        private void Update()
        {
            if(stepTime > 0f) stepTime -= Time.deltaTime;

            if (PlayerStateMachine.IsGrounded)
            {
                if (surfaceUnder != null)
                {
                    SurfaceDetails? surfaceDetails = SurfaceDetails.GetSurfaceDetails(surfaceUnder.gameObject, transform.position, SurfaceDetection);
                    if (FootstepStyle != FootstepStyleEnum.Animation && surfaceDetails.HasValue)
                        EvaluateFootsteps(surfaceDetails.Value);
                }
            }
            else
            {
                airTime += Time.deltaTime;
                wasInAir = true;
            }
        }

        private void EvaluateFootsteps(SurfaceDetails surfaceDetails)
        {
            float playerVelocity = PlayerCollider.velocity.magnitude;
            bool isCrouching = PlayerStateMachine.IsCurrent(PlayerStateMachine.CROUCH_STATE);
            isWalking = PlayerStateMachine.IsCurrent(PlayerStateMachine.WALK_STATE);
            isRunning = PlayerStateMachine.IsCurrent(PlayerStateMachine.RUN_STATE);

            if (isCrouching) return;

            if (FootstepStyle == FootstepStyleEnum.Timed)
            {
                if(wasInAir)
                {
                    if(airTime >= LandStepTime) 
                        PlayFootstep(surfaceDetails, true);

                    airTime = 0;
                    wasInAir = false;
                }
                else if(playerVelocity > StepPlayerVelocity && stepTime <= 0)
                {
                    PlayFootstep(surfaceDetails, false);
                    stepTime = isWalking ? WalkStepTime : isRunning ? RunStepTime : 0;
                }
            }
            else if (FootstepStyle == FootstepStyleEnum.HeadBob)
            {
                if (wasInAir)
                {
                    if (airTime >= LandStepTime)
                        PlayFootstep(surfaceDetails, true);

                    airTime = 0;
                    wasInAir = false;
                }
                else if (playerVelocity > StepPlayerVelocity)
                {
                    float yWave = PlayerManager.HeadBobController.Wave.y;
                    if(yWave < HeadBobStepWave && !waveStep)
                    {
                        PlayFootstep(surfaceDetails, false);
                        waveStep = true;
                    }
                    else if (yWave > HeadBobStepWave && waveStep)
                    {
                        waveStep = false;
                    }
                }
            }
        }

        private void PlayFootstep(SurfaceDetails surfaceDetails, bool isLand)
        {
            var footsteps = surfaceDetails.FootstepProperties;
            var multipliers = surfaceDetails.MultiplierProperties;

            if (!isLand && footsteps.SurfaceFootsteps.Length > 0)
            {
                lastStep = GameTools.RandomUnique(0, footsteps.SurfaceFootsteps.Length, lastStep);
                AudioClip footstep = footsteps.SurfaceFootsteps[lastStep];

                float multiplier = multipliers.FootstepsMultiplier;
                float volumeScale = (isWalking ? WalkingVolume : isRunning ? RunningVolume : 0f) * multiplier;

                audioSource.PlayOneShot(footstep, volumeScale);
            }
            else if (footsteps.SurfaceLandSteps.Length > 0)
            {
                lastLandStep = GameTools.RandomUnique(0, footsteps.SurfaceLandSteps.Length, lastLandStep);
                AudioClip landStep = footsteps.SurfaceLandSteps[lastLandStep];

                float multiplier = multipliers.LandStepsMultiplier;
                float volumeScale = LandVolume * multiplier;

                audioSource.PlayOneShot(landStep, volumeScale);
            }
        }

        public void PlayFootstep(bool runningStep)
        {
            if (surfaceUnder == null)
                return;

            SurfaceDetails? surfaceDetails = SurfaceDetails.GetSurfaceDetails(surfaceUnder.gameObject, transform.position, SurfaceDetection);
            if (surfaceDetails.HasValue)
            {
                isWalking = !runningStep;
                isRunning = runningStep;
                PlayFootstep(surfaceDetails.Value, false);
            }
        }

        public void PlayLandSteps()
        {
            if (surfaceUnder == null)
                return;

            SurfaceDetails? surfaceDetails = SurfaceDetails.GetSurfaceDetails(surfaceUnder.gameObject, transform.position, SurfaceDetection);
            if (surfaceDetails.HasValue)
            {
                PlayFootstep(surfaceDetails.Value, true);
            }
        }
    }
}