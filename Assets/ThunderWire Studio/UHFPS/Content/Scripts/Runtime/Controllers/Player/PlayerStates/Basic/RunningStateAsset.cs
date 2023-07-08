using UHFPS.Scriptable;
using UHFPS.Input;
using UnityEngine;

namespace UHFPS.Runtime.States
{
    public class RunningStateAsset : StrafeStateAsset
    {
        public override FSMPlayerState InitState(PlayerStateMachine machine, PlayerStatesGroup group)
        {
            return new RunningPlayerState(machine, group);
        }

        public override string GetStateKey() => PlayerStateMachine.RUN_STATE;

        public override string ToString() => "Running";

        public class RunningPlayerState : StrafePlayerState
        {
            public RunningPlayerState(PlayerStateMachine machine, PlayerStatesGroup group) : base(machine, group)
            {
            }

            public override void OnStateEnter()
            {
                movementSpeed = machine.PlayerBasicSettings.RunSpeed;
                controllerState = machine.StandingState;
            }

            public override void OnStateUpdate()
            {
                base.OnStateUpdate();

                // fix backwards running speed
                bool runSpeed = machine.Input.y > 0 || machine.Input.y > 0 && machine.Input.x > 0;
                movementSpeed = runSpeed ? machine.PlayerBasicSettings.RunSpeed
                    : machine.PlayerBasicSettings.RunSpeed * 0.5f;

                if (StaminaEnabled)
                {
                    float stamina = machine.Stamina.Value;
                    float exhaustionSpeed = runSpeed ? machine.PlayerStamina.RunExhaustionSpeed : machine.PlayerStamina.RunExhaustionSpeed * 0.5f;
                    stamina = Mathf.MoveTowards(stamina, 0f, Time.deltaTime * exhaustionSpeed);
                    machine.Stamina.OnNext(stamina);
                }
            }

            public override Transition[] OnGetTransitions()
            {
                return new Transition[]
                {
                    Transition.To<IdleStateAsset>(() =>
                    {
                        return InputMagnitude <= 0;
                    }),
                    Transition.To<JumpingStateAsset>(() =>
                    {
                        bool jumpPressed = InputManager.ReadButtonOnce("Jump", Controls.JUMP);
                        return jumpPressed && (!StaminaEnabled || machine.Stamina.Value > 0f);
                    }),
                    Transition.To<WalkingStateAsset>(() =>
                    {
                        if(InputMagnitude > 0)
                        {
                            if (machine.PlayerFeatures.RunToggle)
                            {
                                bool runToggle = !InputManager.ReadButtonToggle("Run", Controls.SPRINT);
                                return runToggle || (StaminaEnabled && machine.Stamina.Value <= 0f);
                            }

                            bool runUnPressed = !InputManager.ReadButton(Controls.SPRINT);
                            return runUnPressed || (StaminaEnabled && machine.Stamina.Value <= 0f);
                        }

                        return false;
                    }),
                    Transition.To<CrouchingStateAsset>(() =>
                    {
                        if (machine.PlayerFeatures.CrouchToggle)
                        {
                            return InputManager.ReadButtonToggle("Crouch", Controls.CROUCH);
                        }

                        return InputManager.ReadButton(Controls.CROUCH);
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
        }
    }
}