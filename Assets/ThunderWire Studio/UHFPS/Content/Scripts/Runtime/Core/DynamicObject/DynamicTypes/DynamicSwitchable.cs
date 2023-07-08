using System;
using UnityEngine;
using UHFPS.Tools;
using Newtonsoft.Json.Linq;

namespace UHFPS.Runtime
{
    [Serializable]
    public class DynamicSwitchable : DynamicObjectType
    {
        // limits
        [Tooltip("Limits that define the minimum/maximum angle at which the switchable can be switched.")]
        public MinMax switchLimits;
        [Tooltip("Angle at which an switchable is switched when the game is started.")]
        public float startingAngle;
        [Tooltip("Usually the axis that defines the switch direction. Most likely the Z or negative Z axis.")]
        public Axis limitsForward = Axis.Z;
        [Tooltip("Usually the axis that defines the higne joint. Most likely the Y-axis.")]
        public Axis limitsUpward = Axis.Y;

        // switchable properties
        [Tooltip("Handle parent object, usually the base object where the child is handle of switchable.")]
        public Transform rootObject;
        [Tooltip("The curve that defines the switch on speed for modifier. 0 = start to 1 = end.")]
        public AnimationCurve switchOnCurve = new(new(0, 1), new(1, 1));
        [Tooltip("The curve that defines the switch off speed for modifier. 0 = start to 1 = end.")]
        public AnimationCurve switchOffCurve = new(new(0, 1), new(1, 1));
        [Tooltip("Defines the switch speed of the switchable.")]
        public float switchSpeed = 1f;
        [Tooltip("Defines the damping of an switchable joint.")]
        public float damping = 1f;

        [Tooltip("Flip the switch direction, for example when the switchable is already switched on or the switch limits are flipped.")]
        public bool flipSwitchDirection = false;
        [Tooltip("Flip the mouse drag direction.")]
        public bool flipMouse = false;
        [Tooltip("Lock switchable when switched.")]
        public bool lockOnSwitch = true;
        [Tooltip("Show the switchable gizmos to visualize the limits.")]
        public bool showGizmos = true;

        // private
        private float currentAngle;
        private float targetAngle;
        private float mouseSmooth;

        private bool isSwitched;
        private bool isMoving;
        private bool isSwitchLocked;
        private bool isSwitchSound;

        public override bool ShowGizmos => showGizmos;

        public override bool IsOpened => isSwitched;

        public override void OnDynamicInit()
        {
            if(InteractType == DynamicObject.InteractType.Dynamic)
            {
                targetAngle = startingAngle;
                currentAngle = startingAngle;
            }
            else if(InteractType == DynamicObject.InteractType.Mouse)
            {
                currentAngle = startingAngle;
            }
        }

        public override void OnDynamicStart(PlayerManager player)
        {
            if (isSwitchLocked) return;
            if (!DynamicObject.isLocked)
            {
                if (InteractType == DynamicObject.InteractType.Dynamic && !isMoving)
                {
                    isSwitched = !isSwitched;
                    targetAngle = flipSwitchDirection
                        ? (isSwitched ? switchLimits.max : switchLimits.min)
                        : (isSwitched ? switchLimits.min : switchLimits.max);

                    if (lockOnSwitch) isSwitchLocked = true;
                }
                else if (InteractType == DynamicObject.InteractType.Animation && !Animator.IsAnyPlaying())
                {
                    if (isSwitched = !isSwitched)
                    {
                        Animator.SetTrigger(DynamicObject.useTrigger1);
                        DynamicObject.useEvent1?.Invoke(); // on event
                    }
                    else
                    {
                        Animator.SetTrigger(DynamicObject.useTrigger2);
                        DynamicObject.useEvent2?.Invoke(); // off eevent
                    }

                    if (lockOnSwitch) isSwitchLocked = true;
                }
            }
            else
            {
                TryUnlock();
            }
        }

        public override void OnDynamicOpen()
        {
            if (InteractType == DynamicObject.InteractType.Dynamic && !isMoving)
            {
                targetAngle = flipSwitchDirection
                    ? switchLimits.max : switchLimits.min;

                isSwitched = true;
                if (lockOnSwitch) isSwitchLocked = true;
            }
            else if (InteractType == DynamicObject.InteractType.Animation && !Animator.IsAnyPlaying())
            {
                Animator.SetTrigger(DynamicObject.useTrigger1);
                DynamicObject.useEvent1?.Invoke();

                isSwitched = true;
                if (lockOnSwitch) isSwitchLocked = true;
            }
        }

        public override void OnDynamicClose()
        {
            if (InteractType == DynamicObject.InteractType.Dynamic && !isMoving)
            {
                targetAngle = flipSwitchDirection
                    ? switchLimits.min : switchLimits.max;

                isSwitched = false;
                if (lockOnSwitch) isSwitchLocked = true;
            }
            else if (InteractType == DynamicObject.InteractType.Animation && !Animator.IsAnyPlaying())
            {
                Animator.SetTrigger(DynamicObject.useTrigger2);
                DynamicObject.useEvent2?.Invoke();

                isSwitched = false;
                if (lockOnSwitch) isSwitchLocked = true;
            }
        }

        public override void OnDynamicUpdate()
        {
            float t = 0;

            if (InteractType == DynamicObject.InteractType.Dynamic)
            {
                t = Mathf.InverseLerp(switchLimits.min, switchLimits.max, currentAngle);
                isMoving = t > 0 && t < 1;

                float modifier = isSwitched ? switchOnCurve.Evaluate(t) : switchOffCurve.Evaluate(1 - t);
                currentAngle = Mathf.MoveTowards(currentAngle, targetAngle, Time.deltaTime * switchSpeed * 10 * modifier);

                Vector3 axis = Quaternion.AngleAxis(currentAngle, rootObject.Direction(limitsUpward)) * rootObject.Direction(limitsForward);
                Target.rotation = Quaternion.LookRotation(axis);
            }
            else if(InteractType == DynamicObject.InteractType.Mouse)
            {
                mouseSmooth = Mathf.MoveTowards(mouseSmooth, targetAngle, Time.deltaTime * (targetAngle != 0 ? switchSpeed : damping));
                currentAngle = Mathf.Clamp(currentAngle + mouseSmooth, switchLimits.RealMin, switchLimits.RealMax);

                Vector3 axis = Quaternion.AngleAxis(currentAngle, rootObject.Direction(limitsUpward)) * rootObject.Direction(limitsForward);
                Target.rotation = Quaternion.LookRotation(axis);

                t = Mathf.InverseLerp(switchLimits.min, switchLimits.max, currentAngle);
                if(t > 0.99f && !isSwitched) isSwitched = true;
                else if(t < 0.01f && isSwitched) isSwitched = false;
            }

            if(InteractType != DynamicObject.InteractType.Animation)
            {
                if (isSwitched && !isSwitchSound && t > 0.99f)
                {
                    DynamicObject.useEvent1?.Invoke(); // on event
                    DynamicObject.PlaySound(DynamicSoundType.Open);
                    isSwitchSound = true;
                }
                else if (!isSwitched && isSwitchSound && t < 0.01f)
                {
                    DynamicObject.useEvent2?.Invoke(); // off event
                    DynamicObject.PlaySound(DynamicSoundType.Close);
                    isSwitchSound = false;
                }
            }

            // value change event
            DynamicObject.onValueChange?.Invoke(t);
        }

        public override void OnDynamicHold(Vector2 mouseDelta)
        {
            if (InteractType == DynamicObject.InteractType.Mouse && !isSwitchLocked)
            {
                mouseDelta.x = 0;
                float mouseInput = Mathf.Clamp(mouseDelta.y, -1, 1) * (flipMouse ? 1 : -1);
                targetAngle = mouseDelta.magnitude > 0 ? mouseInput : 0;
            }

            IsHolding = true;
        }

        public override void OnDynamicEnd()
        {
            if (InteractType == DynamicObject.InteractType.Mouse && !isSwitchLocked)
            {
                targetAngle = 0;
            }

            IsHolding = false;
        }

        public override void OnDrawGizmos()
        {
            if (DynamicObject == null || rootObject == null || InteractType == DynamicObject.InteractType.Animation) return;

            Vector3 forward = rootObject.Direction(limitsForward);
            Vector3 upward = rootObject.Direction(limitsUpward);
            HandlesDrawing.DrawLimits(rootObject.position, switchLimits, forward, upward, true, radius: 0.25f);

            Vector3 from = Quaternion.AngleAxis(switchLimits.min - switchLimits.min, upward) * forward;
            Quaternion angleRotation = Quaternion.AngleAxis(startingAngle, upward);

            Gizmos.color = Color.red;
            Gizmos.DrawRay(rootObject.position, angleRotation * from * 0.25f);
        }

        public override StorableCollection OnSave()
        {
            StorableCollection saveableBuffer = new StorableCollection();

            if (InteractType != DynamicObject.InteractType.Animation)
            {
                saveableBuffer.Add("rotation", Target.eulerAngles.ToSaveable());
                saveableBuffer.Add("angle", currentAngle);
                saveableBuffer.Add(nameof(isSwitchSound), isSwitchSound);
                saveableBuffer.Add(nameof(isSwitchLocked), isSwitchLocked);
            }

            saveableBuffer.Add(nameof(isSwitched), isSwitched);
            return saveableBuffer;
        }

        public override void OnLoad(JToken token)
        {
            if (InteractType != DynamicObject.InteractType.Animation)
            {
                Target.eulerAngles = token["rotation"].ToObject<Vector3>();
                currentAngle = (float)token["angle"];
                isSwitchSound = (bool)token[nameof(isSwitchSound)];
                isSwitchLocked = (bool)token[nameof(isSwitchLocked)];
            }

            isSwitched = (bool)token[nameof(isSwitched)];
        }
    }
}