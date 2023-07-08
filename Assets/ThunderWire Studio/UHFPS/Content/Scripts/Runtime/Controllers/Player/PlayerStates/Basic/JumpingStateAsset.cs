using UnityEngine;
using UHFPS.Scriptable;
using UHFPS.Input;

namespace UHFPS.Runtime.States
{
    public class JumpingStateAsset : StrafeStateAsset
    {
        public override FSMPlayerState InitState(PlayerStateMachine machine, PlayerStatesGroup group)
        {
            return new JumpingPlayerState(machine, group);
        }

        public override string GetStateKey() => PlayerStateMachine.JUMP_STATE;

        public override string ToString() => "Jumping";

        public class JumpingPlayerState : StrafePlayerState
        {
            public JumpingPlayerState(PlayerStateMachine machine, PlayerStatesGroup group) : base(machine, group)
            {
            }

            public override void OnStateEnter()
            {
                movementSpeed = machine.Motion.magnitude;
                machine.Motion.y = Mathf.Sqrt(machine.PlayerBasicSettings.JumpHeight * -2f * GravityForce());

                if (machine.PlayerFeatures.EnableStamina)
                {
                    float stamina = machine.Stamina.Value;
                    stamina -= machine.PlayerStamina.JumpExhaustion * 0.01f;
                    machine.Stamina.OnNext(stamina);
                }
            }

            public override Transition[] OnGetTransitions()
            {
                return new Transition[]
                {
                    Transition.To<IdleStateAsset>(() => IsGrounded),
                    Transition.To<CrouchingStateAsset>(() =>
                    {
                        if(IsGrounded)
                        {
                            if (machine.PlayerFeatures.CrouchToggle)
                            {
                                return InputManager.ReadButtonToggle("Crouch", Controls.CROUCH);
                            }

                            return InputManager.ReadButton(Controls.CROUCH);
                        }

                        return false;
                    }),
                    Transition.To<DeathStateAsset>(() => IsDead)
                };
            }
        }
    }
}