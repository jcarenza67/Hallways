using UnityEngine;
using UHFPS.Input;
using UHFPS.Scriptable;

namespace UHFPS.Runtime.States
{
    public class SlidingStateAsset : StrafeStateAsset
    {
        public float SlidingFriction = 2f;
        public float SpeedChange = 2f;
        public float MotionChange = 2f;
        public float SlideControlChange = 2f;
        public bool SlideControl = true;

        public override FSMPlayerState InitState(PlayerStateMachine machine, PlayerStatesGroup group)
        {
            return new SlidingPlayerState(machine, group, this);
        }

        public override string GetStateKey() => PlayerStateMachine.SLIDING_STATE;

        public override string ToString() => "Sliding";

        public class SlidingPlayerState : StrafePlayerState
        {
            protected readonly SlidingStateAsset State;

            private bool isSliding;
            private float slidingSpeed;

            private Vector3 enterMotion;
            private float motionToSlidingBlend;

            public SlidingPlayerState(PlayerStateMachine machine, PlayerStatesGroup group, StrafeStateAsset stateAsset) : base(machine, group) 
            {
                State = (SlidingStateAsset)stateAsset;
            }

            public override void OnStateEnter()
            {
                controllerState = machine.StandingState;
                slidingSpeed = machine.Motion.magnitude;
                enterMotion = machine.Motion;
                motionToSlidingBlend = 0f;
                InputManager.ResetToggledButtons();
            }

            public override void OnStateUpdate()
            {
                bool sliding = SlopeCast(out Vector3 normal, out float angle);
                isSliding = sliding && angle > strafeGroup.slopeLimit;

                Vector3 slidingForward = Vector3.ProjectOnPlane(Vector3.down, normal);
                Vector3 slidingRight = Vector3.Cross(normal, slidingForward);

                Vector3 slidingDirection = slidingForward;
                if (State.SlideControl) slidingDirection += machine.Input.x * State.SlideControlChange * slidingRight;

                slidingSpeed = Mathf.MoveTowards(slidingSpeed, State.SlidingFriction, Time.deltaTime * State.SpeedChange);
                slidingDirection = slidingDirection.normalized * slidingSpeed;

                motionToSlidingBlend = Mathf.MoveTowards(motionToSlidingBlend, 1f, Time.deltaTime * State.MotionChange);
                Vector3 finalMotion = Vector3.Lerp(enterMotion, slidingDirection, motionToSlidingBlend);

                machine.Motion = finalMotion;
                PlayerHeightUpdate();
            }

            public override Transition[] OnGetTransitions()
            {
                return new Transition[]
                {
                    Transition.To<IdleStateAsset>(() => !isSliding),
                    Transition.To<DeathStateAsset>(() => IsDead)
                };
            }
        }
    }
}