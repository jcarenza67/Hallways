using System;
using UnityEngine;
using UHFPS.Tools;
using Newtonsoft.Json.Linq;

namespace UHFPS.Runtime
{
    [Serializable]
    public class DynamicOpenable : DynamicObjectType
    {
        // limits
        [Tooltip("Limits that define the minimum/maximum angle at which the openable can be opened.")]
        public MinMax openLimits;
        [Tooltip("Angle at which an openable is opened when the game is started.")]
        public float startingAngle;
        [Tooltip("Usually the axis that defines the open direction. Most likely the Z or negative Z axis.")]
        public Axis limitsForward = Axis.Z;
        [Tooltip("Usually the axis that defines the higne joint. Most likely the Y-axis.")]
        public Axis limitsUpward = Axis.Y;

        // openable properties
        [Tooltip("The curve that defines the opening speed for modifier. 0 = start to 1 = end.")]
        public AnimationCurve openCurve = new(new(0, 1), new(1, 1));
        [Tooltip("The curve that defines the closing speed for modifier. 0 = start to 1 = end.")]
        public AnimationCurve closeCurve = new(new(0, 1), new(1, 1));
        [Tooltip("Usually the axis that determines the forward direction of the frame. The direction is used to determine in which direction the door should open. Usually the same axis as the limits forward axis.")]
        public Axis openableForward = Axis.Z;
        [Tooltip("Usually the axis that defines the hinge joint. It will help to define where the top is when the openable is flipping at an angle below 0.")]
        public Axis openableUp = Axis.Y;

        [Tooltip("Defines the open/close speed of the openable.")]
        public float openSpeed = 1f;
        [Tooltip("Defines the damping of an openable joint.")]
        public float damper = 1f;
        [Tooltip("Defines the minimum volume at which the open/close motion sound will be played.")]
        public float dragSoundPlay = 0.2f;

        [Tooltip("Flip the open direction, for example when the openable is already opened or the open limits are flipped.")]
        public bool flipOpenDirection = false;
        [Tooltip("Flip the forward direction, for example when the openable gizmo is pointing in the wrong direction.")]
        public bool flipForwardDirection = false;
        [Tooltip("Use the upward direction to determine where the openable up is pointing.")]
        public bool useUpwardDirection = false;
        [Tooltip("Defines when the openable can be opened on both sides.")]
        public bool bothSidesOpen = false;
        [Tooltip("Allows to use drag sounds.")]
        public bool dragSounds = false;
        [Tooltip("Play sound when the openable is closed.")]
        public bool playCloseSound = true;
        [Tooltip("Flip the mouse drag direction.")]
        public bool flipMouse = false;
        [Tooltip("Flip the openable min/max limits.")]
        public bool flipValue = false;
        [Tooltip("Show the openable gizmos to visualize the limits.")]
        public bool showGizmos = true;

        // sounds
        public SoundClip dragSound;

        // private
        private float currentAngle;
        private float targetAngle;
        private float openAngle;
        private float prevAngle;

        private bool isOpened;
        private bool isMoving;
        private bool isOpenSound;
        private bool isCloseSound;
        private bool disableSounds;

        private Vector3 limitsFwd;
        private Vector3 limitsUpwd;
        private Vector3 openableFwd;

        public override bool ShowGizmos => showGizmos;

        public override bool IsOpened => isOpened;

        public override void OnDynamicInit()
        {
            limitsFwd = Target.Direction(limitsForward);
            limitsUpwd = Target.Direction(limitsUpward);
            openableFwd = Target.Direction(openableForward);

            if (InteractType == DynamicObject.InteractType.Mouse && Joint != null)
            {
                // configure joint limits
                JointLimits limits = Joint.limits;
                limits.min = openLimits.min;
                limits.max = openLimits.max;
                Joint.limits = limits;

                // configure joint spring
                JointSpring spring = Joint.spring;
                spring.damper = damper;
                Joint.spring = spring;

                // configure joint motor
                JointMotor motor = Joint.motor;
                motor.force = 1f;
                Joint.motor = motor;

                // enable/disable joint features
                Joint.useSpring = true;
                Joint.useLimits = true;
                Joint.useMotor = false;

                // configure joint axis and rigidbody
                Joint.axis = openableUp.Convert();
                Rigidbody.isKinematic = false;
                Rigidbody.useGravity = true;
            }

            if(InteractType != DynamicObject.InteractType.Animation)
            {
                SetOpenableAngle(startingAngle);

                targetAngle = startingAngle;
                currentAngle = startingAngle;
                openAngle = startingAngle;

                float mid = Mathf.Lerp(openLimits.min, openLimits.max, 0.5f);
                disableSounds = Mathf.Abs(startingAngle) > Mathf.Abs(mid);
                isOpenSound = disableSounds;
            }
        }

        public override void OnDynamicStart(PlayerManager player)
        {
            if (DynamicObject.isLocked)
            {
                TryUnlock();
                return;
            }

            if (InteractType == DynamicObject.InteractType.Dynamic)
            {
                if (isMoving) 
                    return;

                if (bothSidesOpen)
                {
                    float lookDirection = Vector3.Dot(openableFwd, player.MainCamera.transform.forward);
                    prevAngle = openAngle;
                    openAngle = targetAngle = (isOpened = !isOpened)
                        ? flipOpenDirection
                            ? (lookDirection > 0 ? openLimits.max : openLimits.min)
                            : (lookDirection > 0 ? openLimits.min : openLimits.max)
                        : 0;
                }
                else
                {
                    prevAngle = openAngle;
                    openAngle = targetAngle = flipOpenDirection
                        ? (isOpened ? openLimits.max : openLimits.min)
                        : (isOpened ? openLimits.min : openLimits.max);
                    isOpened = !isOpened;
                }
            }
            else if (InteractType == DynamicObject.InteractType.Animation && !Animator.IsAnyPlaying())
            {
                if (isOpened = !isOpened)
                {
                    if (bothSidesOpen)
                    {
                        float lookDirection = Vector3.Dot(openableFwd, player.MainCamera.transform.forward);
                        Animator.SetBool(DynamicObject.useTrigger3, Mathf.RoundToInt(lookDirection) > 0);
                    }

                    Animator.SetTrigger(DynamicObject.useTrigger1);
                    DynamicObject.PlaySound(DynamicSoundType.Open);
                    DynamicObject.useEvent1?.Invoke();  // open event
                }
                else
                {
                    Animator.SetTrigger(DynamicObject.useTrigger2);
                    DynamicObject.useEvent2?.Invoke(); // close event
                    isCloseSound = true;
                }
            }

            if (disableSounds) disableSounds = false;
        }

        public override void OnDynamicOpen()
        {
            if (InteractType == DynamicObject.InteractType.Dynamic)
            {
                if (isMoving)
                    return;

                prevAngle = openAngle;
                openAngle = targetAngle = flipOpenDirection
                    ? openLimits.min : openLimits.max;
                isOpened = true;
            }
            else if (InteractType == DynamicObject.InteractType.Animation && !Animator.IsAnyPlaying())
            {
                Animator.SetTrigger(DynamicObject.useTrigger1);
                DynamicObject.PlaySound(DynamicSoundType.Open);
                DynamicObject.useEvent1?.Invoke();
                isOpened = true;
            }
        }

        public override void OnDynamicClose()
        {
            if (InteractType == DynamicObject.InteractType.Dynamic)
            {
                if (isMoving)
                    return;

                prevAngle = openAngle;
                openAngle = targetAngle = flipOpenDirection
                    ? openLimits.max : openLimits.min;
                isOpened = false;
            }
            else if (InteractType == DynamicObject.InteractType.Animation && !Animator.IsAnyPlaying())
            {
                Animator.SetTrigger(DynamicObject.useTrigger2);
                DynamicObject.useEvent2?.Invoke();
                isCloseSound = true;
                isOpened = false;
            }
        }

        public override void OnDynamicUpdate()
        {
            float t = 0;

            if (InteractType == DynamicObject.InteractType.Dynamic)
            {
                t = Mathf.InverseLerp(prevAngle, openAngle, currentAngle);
                DynamicObject.onValueChange?.Invoke(t);
                isMoving = t > 0 && t < 1;

                float modifier = isOpened ? openCurve.Evaluate(t) : closeCurve.Evaluate(t);
                currentAngle = Mathf.MoveTowards(currentAngle, targetAngle, Time.deltaTime * openSpeed * 10 * modifier);
                SetOpenableAngle(currentAngle);

                if (!disableSounds)
                {
                    if (isOpened && !isOpenSound && t > 0.02f)
                    {
                        DynamicObject.PlaySound(DynamicSoundType.Open);
                        DynamicObject.useEvent1?.Invoke(); // open event
                        isOpenSound = true;
                    }
                    else if (!isOpened && isOpenSound && t > 0.95f)
                    {
                        DynamicObject.PlaySound(DynamicSoundType.Close);
                        DynamicObject.useEvent2?.Invoke(); // close event
                        isOpenSound = false;
                    }
                }
            }
            else if(InteractType == DynamicObject.InteractType.Mouse)
            {
                Vector3 minDir = Quaternion.AngleAxis(openLimits.min, limitsUpwd) * limitsFwd;
                Vector3 maxDir = Quaternion.AngleAxis(openLimits.max, limitsUpwd) * limitsFwd;

                Vector3 newMin = flipValue ? maxDir : minDir;
                Vector3 newMax = flipValue ? minDir : maxDir;

                Vector3 forward = Target.Direction(openableForward);
                t = VectorE.InverseLerp(newMin, newMax, forward);
                DynamicObject.onValueChange?.Invoke(t);

                if (!disableSounds)
                {
                    if (!isOpened && t > 0.02f)
                    {
                        DynamicObject.PlaySound(DynamicSoundType.Open);
                        DynamicObject.useEvent1?.Invoke(); // open event
                        isOpened = true;
                    }
                    else if (isOpened && t < 0.01f)
                    {
                        DynamicObject.PlaySound(DynamicSoundType.Close);
                        DynamicObject.useEvent2?.Invoke(); // close event
                        isOpened = false;
                    }
                }

                if (dragSounds)
                {
                    float angle = Target.localEulerAngles.Component(openableUp).FixAngle(openLimits.min, openLimits.max);
                    float volumeMag = Mathf.Clamp01(Rigidbody.velocity.magnitude);

                    if (volumeMag > dragSoundPlay && ((Vector2)openLimits).InRange(angle))
                    {
                        AudioSource.SetSoundClip(dragSound, volumeMag, true);
                    }
                    else
                    {
                        if (AudioSource.volume > 0.01f)
                        {
                            AudioSource.volume = Mathf.MoveTowards(AudioSource.volume, 0f, Time.deltaTime);
                        }
                        else
                        {
                            AudioSource.volume = 0f;
                            AudioSource.Stop();
                        }
                    }
                }
            }

            if(InteractType != DynamicObject.InteractType.Animation)
            {
                // value change event
                DynamicObject.onValueChange?.Invoke(t);
            }
            else if(playCloseSound && !isOpened && isCloseSound && !Animator.IsAnyPlaying())
            {
                DynamicObject.PlaySound(DynamicSoundType.Close);
                isCloseSound = false;
            }
        }

        private void SetOpenableAngle(float angle)
        {
            Vector3 upward = Target.Direction(openableUp);
            int flipForward = flipForwardDirection ? -1 : 1;

            Vector3 axis = Quaternion.AngleAxis(angle, limitsUpwd) * limitsFwd * flipForward;
            if (useUpwardDirection) Target.rotation = Quaternion.LookRotation(axis, upward);
            else Target.rotation = Quaternion.LookRotation(axis);
        }

        public override void OnDynamicHold(Vector2 mouseDelta)
        {
            if (InteractType == DynamicObject.InteractType.Mouse && Joint != null)
            {
                mouseDelta.y = 0;
                if (mouseDelta.magnitude > 0)
                {
                    Joint.useMotor = true;
                    JointMotor motor = Joint.motor;
                    motor.targetVelocity = mouseDelta.x * openSpeed * 10 * (flipMouse ? -1 : 1);
                    Joint.motor = motor;
                }
                else
                {
                    Joint.useMotor = false;
                    JointMotor motor = Joint.motor;
                    motor.targetVelocity = 0f;
                    Joint.motor = motor;
                }
            }

            IsHolding = true;
        }

        public override void OnDynamicEnd()
        {
            if (InteractType == DynamicObject.InteractType.Mouse && Joint != null)
            {
                Joint.useMotor = false;
                JointMotor motor = Joint.motor;
                motor.targetVelocity = 0f;
                Joint.motor = motor;
            }

            IsHolding = false;
        }

        public override void OnDrawGizmos()
        {
            if (DynamicObject == null || Target == null || InteractType == DynamicObject.InteractType.Animation) return;

            Vector3 forward = Application.isPlaying ? limitsFwd : Target.Direction(limitsForward);
            Vector3 upward = Application.isPlaying ? limitsUpwd : Target.Direction(limitsUpward);
            forward = Quaternion.Euler(0, -90, 0) * forward;
            float radius = 0.3f;

            HandlesDrawing.DrawLimits(DynamicObject.transform.position, openLimits, forward, upward, true, flipOpenDirection, radius);

            Vector3 startingDir = Quaternion.AngleAxis(startingAngle, upward) * forward;
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(Target.position, startingDir * radius);
        }

        public override StorableCollection OnSave()
        {
            StorableCollection saveableBuffer = new StorableCollection();
            saveableBuffer.Add("rotation", Target.eulerAngles.ToSaveable());

            if (InteractType != DynamicObject.InteractType.Animation)
            {
                saveableBuffer.Add("targetAngle", targetAngle);
                saveableBuffer.Add("currentAngle", currentAngle);
                saveableBuffer.Add("openAngle", openAngle);
                saveableBuffer.Add("isOpenSound", isOpenSound);
                saveableBuffer.Add("disableSounds", disableSounds);
                saveableBuffer.Add("isOpened", isOpened);
            }

            return saveableBuffer;
        }

        public override void OnLoad(JToken token)
        {
            Target.eulerAngles = token["rotation"].ToObject<Vector3>();

            if (InteractType != DynamicObject.InteractType.Animation)
            {
                targetAngle = (float)token["targetAngle"];
                currentAngle = (float)token["currentAngle"];
                openAngle = (float)token["openAngle"];
                isOpenSound = (bool)token["isOpenSound"];
                disableSounds = (bool)token["disableSounds"];
                isOpened = (bool)token["isOpened"];
            }
        }
    }
}