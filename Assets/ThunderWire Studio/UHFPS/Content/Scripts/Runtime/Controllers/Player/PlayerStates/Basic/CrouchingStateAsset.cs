using UnityEngine;
using UHFPS.Scriptable;
using UHFPS.Input;

namespace UHFPS.Runtime.States
{
    public class CrouchingStateAsset : StrafeStateAsset
    {
        public override FSMPlayerState InitState(PlayerStateMachine machine, PlayerStatesGroup group)
        {
            return new CrouchingPlayerState(machine, group);
        }

        public override string GetStateKey() => PlayerStateMachine.CROUCH_STATE;

        public override string ToString() => "Crouching";

        public class CrouchingPlayerState : StrafePlayerState
        {
            public CrouchingPlayerState(PlayerStateMachine machine, PlayerStatesGroup group) : base(machine, group)
            {
            }

            public override void OnStateEnter()
            {
                movementSpeed = machine.PlayerBasicSettings.CrouchSpeed;
                controllerState = machine.CrouchingState;
            }

            public override Transition[] OnGetTransitions()
            {
                return new Transition[]
                {
                    Transition.To<IdleStateAsset>(() =>
                    {
                        if (machine.PlayerFeatures.CrouchToggle)
                        {
                            if(InputManager.ReadButtonOnce("CrouchState", Controls.JUMP))
                            {
                                InputManager.ResetToggledButtons();
                                return !CheckStandObstacle();
                            }

                            if(!InputManager.ReadButtonToggle("Crouch", Controls.CROUCH))
                                return !CheckStandObstacle();

                            return false;
                        }

                        if(!InputManager.ReadButton(Controls.CROUCH))
                        {
                            InputManager.ResetToggledButtons();
                            return !CheckStandObstacle();
                        }

                        return false;
                    }),
                    Transition.To<SlidingStateAsset>(() =>
                    {
                        if(SlopeCast(out _, out float angle))
                            return angle > strafeGroup.slopeLimit;

                        return false;
                    }),
                    Transition.To<DeathStateAsset>(() => IsDead)
                };
            }

            private bool CheckStandObstacle()
            {
                float height = machine.StandingState.ControllerHeight + 0.1f;
                float radius = controller.radius;
                Vector3 origin = machine.ControllerFeet;
                Ray ray = new(origin, Vector3.up);

                return Physics.SphereCast(ray, radius, out _, height, machine.SurfaceMask);
            }
        }
    }
}