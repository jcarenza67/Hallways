using UnityEngine;
using UHFPS.Input;
using ThunderWire.Attributes;
using System;

namespace UHFPS.Runtime
{
    [InspectorHeader("Drag Rigidbody")]
    [RequireComponent(typeof(InteractController))]
    public class DragRigidbody : PlayerComponent, IReticleProvider
    {
        public enum HoldTypeEnum { Press, Hold }
        public enum DragTypeEnum { WeightedVelocity, FixedVelocity }

        public HoldTypeEnum HoldType = HoldTypeEnum.Press;
        public DragTypeEnum DragType = DragTypeEnum.WeightedVelocity;
        public GString ControlsText;

        [Header("Reticle")]
        public Reticle GrabHand;
        public Reticle HoldHand;
        public bool ShowGrabReticle = true;

        [Header("Rigidbody Settings")]
        public RigidbodyInterpolation Interpolate = RigidbodyInterpolation.Interpolate;
        public CollisionDetectionMode CollisionDetection = CollisionDetectionMode.ContinuousDynamic;
        public bool FreezeRotation = false;

        [Header("Settings")]
        public float DragStrength = 10f;
        public float ThrowStrength = 10f;
        public float RotateSpeed = 1f;
        public float ZoomSpeed = 1f;

        [Header("Features")]
        public bool HitpointOffset = true;
        public bool PlayerCollision = false;
        public bool ObjectZooming = true;
        public bool ObjectRotating = true;
        public bool ObjectThrowing = true;

        private GameManager gameManager;
        private InteractController interactController;
        private DraggableItem currentDraggable;
        private Rigidbody draggableRigidbody;
        private GameObject raycastObject;

        private Transform defParent;
        private RigidbodyInterpolation defInterpolate;
        private CollisionDetectionMode defCollisionDetection;
        private bool defFreezeRotation;
        private bool defUseGravity;
        private bool defIsKinematic;

        private Vector3 holdOffset;
        private GameObject holdPoint;
        private GameObject holdRotatePoint;
        private float holdDistance;

        private bool isDragging;
        private bool isRotating;
        private bool isThrown;
        
        private void Awake()
        {
            gameManager = GameManager.Instance;
            interactController = GetComponent<InteractController>();
        }

        private void Start()
        {
            ControlsText.SubscribeGlocMany();
        }

        private void Update()
        {
            raycastObject = interactController.RaycastObject;
            if (raycastObject != null || isDragging)
            {
                if(HoldType == HoldTypeEnum.Press)
                {
                    if (InputManager.ReadButtonOnce(GetInstanceID(), Controls.USE))
                    {
                        if (!isDragging)
                        {
                            GrabObject();
                        }
                        else
                        {
                            DropObject();
                            isDragging = false;
                        }
                    }
                }
                else
                {
                    if (InputManager.ReadButton(Controls.USE))
                    {
                        if (!isDragging && !isThrown)
                        {
                            GrabObject();
                        }
                    }
                    else if (isDragging)
                    {
                        DropObject();
                        isDragging = false;
                    }
                    else
                    {
                        isThrown = false;
                    }
                }
            }

            if (isDragging) HoldUpdate();
        }

        private void FixedUpdate()
        {
            if (isDragging) FixedHoldUpdate();
        }

        private void GrabObject()
        {
            if (raycastObject == null) return;
            if (!raycastObject.TryGetComponent(out currentDraggable)) return;
            if (!raycastObject.TryGetComponent(out draggableRigidbody)) return;

            defUseGravity = draggableRigidbody.useGravity;
            defIsKinematic = draggableRigidbody.isKinematic;

            defInterpolate = draggableRigidbody.interpolation;
            defCollisionDetection = draggableRigidbody.collisionDetectionMode;
            defFreezeRotation = draggableRigidbody.freezeRotation;

            draggableRigidbody.interpolation = Interpolate;
            draggableRigidbody.collisionDetectionMode = CollisionDetection;
            draggableRigidbody.freezeRotation = FreezeRotation;
            Physics.IgnoreCollision(raycastObject.GetComponent<Collider>(), PlayerCollider, !PlayerCollision);

            if (DragType == DragTypeEnum.FixedVelocity)
            {
                float distance = Vector3.Distance(MainCamera.transform.position, raycastObject.transform.position);
                holdDistance = Mathf.Clamp(distance, currentDraggable.ZoomDistance.RealMin, currentDraggable.ZoomDistance.RealMax);

                if (currentDraggable.KeepParent) defParent = raycastObject.transform.parent;
                raycastObject.transform.SetParent(PlayerManager.MainVirtualCamera.transform);
                draggableRigidbody.velocity = Vector3.zero;

                draggableRigidbody.useGravity = false;
                draggableRigidbody.isKinematic = false;
            }
            else
            {
                holdPoint = new GameObject("HoldPoint");
                holdPoint.transform.SetParent(VirtualCamera.transform);
                holdPoint.transform.position = VirtualCamera.transform.position + VirtualCamera.transform.forward * holdDistance;

                if (HitpointOffset)
                {
                    holdRotatePoint = new GameObject("RotatePoint");
                    holdRotatePoint.transform.SetParent(holdPoint.transform);
                    holdRotatePoint.transform.position = Vector3.zero;
                    holdRotatePoint.transform.eulerAngles = raycastObject.transform.eulerAngles;
                }
                else
                {
                    holdPoint.transform.eulerAngles = raycastObject.transform.eulerAngles;
                }

                Vector3 localHitpoint = interactController.LocalHitpoint;
                Vector3 worldHitpoint = raycastObject.transform.TransformPoint(localHitpoint);
                holdOffset = worldHitpoint - raycastObject.transform.position;

                float distance = Vector3.Distance(MainCamera.transform.position, worldHitpoint);
                holdDistance = Mathf.Clamp(distance, currentDraggable.ZoomDistance.RealMin, currentDraggable.ZoomDistance.RealMax);

                draggableRigidbody.useGravity = true;
                draggableRigidbody.isKinematic = false;
            }

            foreach (var dragStart in raycastObject.GetComponents<IOnDragStart>())
            {
                dragStart.OnDragStart();
            }

            PlayerManager.PlayerItems.IsItemsUsable = false;
            interactController.EnableInteractInfo(false);
            gameManager.ShowControlsInfo(true, ControlsText);
            isDragging = true;
        }

        private void HoldUpdate()
        {
            if (ObjectZooming && InputManager.ReadInput(Controls.SCROLL_WHEEL, out Vector2 scroll))
            {
                holdDistance = Mathf.Clamp(holdDistance + scroll.y * ZoomSpeed * 0.001f, currentDraggable.ZoomDistance.RealMin, currentDraggable.ZoomDistance.RealMax);
            }

            if (ObjectRotating && (holdPoint != null || holdRotatePoint != null) && InputManager.ReadButton(Controls.RELOAD))
            {
                InputManager.ReadInput(Controls.POINTER_DELTA, out Vector2 delta);
                delta = delta.normalized * RotateSpeed;

                Transform rotateTransform = HitpointOffset ? holdRotatePoint.transform : holdPoint.transform;
                rotateTransform.Rotate(VirtualCamera.transform.up, delta.x, Space.World);
                rotateTransform.Rotate(VirtualCamera.transform.right, delta.y, Space.World);

                LookController.SetEnabled(false);
                isRotating = true;
            }
            else if(isRotating)
            {
                LookController.SetEnabled(true);
                isRotating = false;
            }

            if(ObjectThrowing && InputManager.ReadButtonOnce("Fire", Controls.FIRE))
            {
                ThrowObject();
                isThrown = true;
            }
        }

        private void FixedHoldUpdate()
        {
            Vector3 grabPos = VirtualCamera.transform.position + VirtualCamera.transform.forward * holdDistance;
            Vector3 currPos = currentDraggable.transform.position;

            if (HitpointOffset && holdPoint != null)
            {
                Vector3 offsetDirection = holdPoint.transform.TransformDirection(holdOffset);
                grabPos -= offsetDirection;
            }

            Vector3 targetVelocity = grabPos - currPos;

            if (DragType == DragTypeEnum.WeightedVelocity)
            {
                holdPoint.transform.position = grabPos;
                targetVelocity.Normalize();

                float massFactor = 1f / draggableRigidbody.mass;
                float distanceFactor = Mathf.Clamp01(Vector3.Distance(grabPos, currPos));
                Transform rotateTransform = HitpointOffset ? holdRotatePoint.transform : holdPoint.transform;

                draggableRigidbody.velocity = Vector3.Lerp(draggableRigidbody.velocity, distanceFactor * DragStrength * massFactor * targetVelocity, 0.3f);
                draggableRigidbody.rotation = Quaternion.Slerp(draggableRigidbody.rotation, rotateTransform.rotation, 0.3f);
                draggableRigidbody.angularVelocity = Vector3.zero;
            }
            else
            {
                draggableRigidbody.velocity = targetVelocity * DragStrength;
                draggableRigidbody.angularVelocity = Vector3.Lerp(draggableRigidbody.angularVelocity, Vector3.zero, 0.3f);
            }

            foreach (var dragUpdate in currentDraggable.GetComponents<IOnDragUpdate>())
            {
                dragUpdate.OnDragUpdate(targetVelocity);
            }

            if(Vector3.Distance(currPos, MainCamera.transform.position) > currentDraggable.MaxHoldDistance)
            {
                DropObject();
            }
        }

        private void ThrowObject()
        {
            draggableRigidbody.AddForce(10 * ThrowStrength * MainCamera.transform.forward, ForceMode.Force);
            DropObject();
        }

        private void DropObject()
        {
            if (DragType == DragTypeEnum.FixedVelocity)
            {
                if (currentDraggable.KeepParent) currentDraggable.transform.SetParent(defParent);
                else currentDraggable.transform.SetParent(null);
            }

            draggableRigidbody.useGravity = defUseGravity;
            draggableRigidbody.isKinematic = defIsKinematic;

            draggableRigidbody.interpolation = defInterpolate;
            draggableRigidbody.collisionDetectionMode = defCollisionDetection;
            draggableRigidbody.freezeRotation = defFreezeRotation;
            Physics.IgnoreCollision(currentDraggable.GetComponent<Collider>(), PlayerCollider, false);

            if (isRotating)
            {
                LookController.SetEnabled(true);
                isRotating = false;
            }

            foreach (var dragEnd in currentDraggable.GetComponents<IOnDragEnd>())
            {
                dragEnd.OnDragEnd();
            }

            Destroy(holdPoint);
            interactController.EnableInteractInfo(true);
            gameManager.ShowControlsInfo(false, string.Empty);
            PlayerManager.PlayerItems.IsItemsUsable = true;

            holdOffset = Vector3.zero;
            holdDistance = 0;

            draggableRigidbody = null;
            currentDraggable = null;
            isDragging = false;
        }

        public (Type, Reticle, bool) OnProvideReticle()
        {
            Reticle reticle = isDragging ? HoldHand : GrabHand;
            if (ShowGrabReticle) return (typeof(DraggableItem), reticle, isDragging);
            else return (null, null, false);
        }
    }
}