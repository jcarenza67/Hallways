using UnityEngine;
using UHFPS.Scriptable;

namespace UHFPS.Runtime.States
{
    public abstract class StrafeStateAsset : PlayerStateAsset
    {
        public override FSMPlayerState InitState(PlayerStateMachine machine, PlayerStatesGroup group)
        {
            return new StrafePlayerState(machine, group);
        }

        public class StrafePlayerState : FSMPlayerState
        {
            protected StrafeMovementGroup strafeGroup;
            protected float movementSpeed;

            public StrafePlayerState(PlayerStateMachine machine, PlayerStatesGroup group) : base(machine)
            {
                strafeGroup = (StrafeMovementGroup)group;
            }

            public override void OnStateUpdate()
            {
                Vector3 wishDir = new Vector3(machine.Input.x, 0, machine.Input.y);
                wishDir = cameraLook.RotationX * wishDir;

                if (machine.IsGrounded)
                {
                    Friction(ref machine.Motion);
                    Accelerate(ref machine.Motion, wishDir, movementSpeed);
                    machine.Motion.y = -machine.PlayerControllerSettings.AntiBumpFactor;
                }
                else
                {
                    AirAccelerate(ref machine.Motion, wishDir, movementSpeed);
                }

                ApplyGravity(ref machine.Motion);
                PlayerHeightUpdate();
            }

            protected void Accelerate(ref Vector3 velocity, Vector3 wishDir, float wishSpeed)
            {
                // see if we are changing direction.
                float currentSpeed = Vector3.Dot(velocity, wishDir);

                // see how much to add.
                float addSpeed = wishSpeed - currentSpeed;

                // if not going to add any speed, done.
                if (addSpeed <= 0) return;

                // determine amount of accleration.
                float accelSpeed = strafeGroup.groundAcceleration * wishSpeed * Time.deltaTime;

                // cap at addspeed.
                accelSpeed = Mathf.Min(accelSpeed, addSpeed);

                // adjust velocity.
                velocity += wishDir * accelSpeed;
            }

            protected void AirAccelerate(ref Vector3 velocity, Vector3 wishDir, float wishSpeed)
            {
                float wishspd = wishSpeed;

                // cap speed.
                wishspd = Mathf.Min(wishspd, strafeGroup.airAccelerationCap);

                // see if we are changing direction.
                float currentSpeed = Vector3.Dot(velocity, wishDir);

                // see how much to add.
                float addSpeed = wishspd - currentSpeed;

                // if not going to add any speed, done.
                if (addSpeed <= 0) return;

                // determine amount of accleration.
                float accelSpeed = strafeGroup.airAcceleration * wishSpeed * Time.deltaTime;

                // cap at addspeed.
                accelSpeed = Mathf.Min(accelSpeed, addSpeed);

                // adjust velocity.
                velocity += wishDir * accelSpeed;
            }

            protected void Friction(ref Vector3 velocity)
            {
                float speed = velocity.magnitude;

                if (speed != 0)
                {
                    float drop = speed * strafeGroup.friction * Time.deltaTime;
                    velocity *= Mathf.Max(speed - drop, 0) / speed;
                }
            }

            protected bool SlopeCast(out Vector3 normal, out float angle)
            {
                if (Physics.SphereCast(CenterPosition, controller.radius, Vector3.down, out RaycastHit hit, strafeGroup.slideRayLength, strafeGroup.slidingMask))
                {
                    normal = hit.normal;
                    angle = Vector3.Angle(hit.normal, Vector3.up);
                    return true;
                }

                normal = Vector3.zero;
                angle = 0f;
                return false;
            }
        }
    }
}