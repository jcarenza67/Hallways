using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UHFPS.Input;
using UHFPS.Tools;
using UnityEngine.UI;

namespace UHFPS.Runtime
{
    [RequireComponent(typeof(InteractController))]
    public class ExamineController : PlayerComponent
    {
        public sealed class ExaminedObject
        {
            public InteractableItem InteractableItem;
            public ExaminePutter.PutSettings PutSettings;
            public Vector3 HoldPosition;
            public Vector3 StartPosition;
            public Quaternion StartRotation;
            public Vector3 ControlPoint;
            public float ExamineDistance;
            public float Velocity;
            public float t;

            public GameObject GameObject => InteractableItem.gameObject;
        }

        public LayerMask FocusCullLayes;
        public Layer FocusLayer;
        public GameObject HotspotPrefab;

        public GString ControlsFormat;
        public string SpaceSeparator = "|";
        public int ControlsPadding = 3;

        public float RotateTime = 0.1f;
        public float RotateMultiplier = 3f;
        public float ZoomMultiplier = 0.1f;
        public float TimeToExamine = 2f;

        public Vector3 DropOffset;
        public Vector3 InventoryOffset;
        public bool ShowLabels = true;

        public AnimationCurve PickUpCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0));
        public float PickUpCurveMultiplier = 1f;
        public float PickUpTime = 0.2f;

        public AnimationCurve PutPositionCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0));
        public float PutPositionCurveMultiplier = 1f;
        public float PutPositionCurveTime = 0.1f;

        public AnimationCurve PutRotationCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0));
        public float PutRotationCurveMultiplier = 1f;
        public float PutRotationCurveTime = 0.1f;

        public SoundClip ExamineHintSound;

        public Vector3 DropPosition => transform.TransformPoint(DropOffset);
        public Vector3 InventoryPosition => transform.TransformPoint(InventoryOffset);

        public bool IsExamining { get; private set; }

        private GameManager gameManager;
        private InteractController interactController;

        private readonly Stack<ExaminedObject> examinedObjects = new();
        private ExaminedObject currentExamine;
        private Image examineHotspot;

        private bool isInventoryExamine;
        private bool isPointerShown;
        private bool isReadingPaper;
        private bool isHotspotPressed;

        private Vector2 examineRotate;
        private Vector2 rotateVelocity;

        private void Awake()
        {
            gameManager = GameManager.Instance;
            interactController = GetComponent<InteractController>();
        }

        private void Start()
        {
            ControlsFormat.SubscribeGlocMany();
        }

        private void Update()
        {
            if (interactController.RaycastObject != null || IsExamining)
            {
                if (InputManager.ReadButtonOnce(GetInstanceID(), Controls.EXAMINE))
                {
                    if (!IsExamining)
                    {
                        GameObject raycastObj = interactController.RaycastObject;
                        if (!raycastObj.GetComponent<ExaminePutter>())
                            StartExamine(raycastObj);
                    }
                    else
                    {
                        PopExaminedObject();
                    }
                }
            }

            if (IsExamining) ExamineHold();
        }

        public void ExamineFromInventory(GameObject obj)
        {
            isInventoryExamine = true;
            StartExamine(obj);
        }

        private void StartExamine(GameObject obj)
        {
            if (obj.TryGetComponent(out InteractableItem interactableItem))
            {
                if (interactableItem.ExamineType == InteractableItem.ExamineTypeEnum.None)
                    return;

                ExamineObject(interactableItem);
                gameManager.SetBlur(true, true);
                gameManager.FreezePlayer(true);
                gameManager.DisableAllGamePanels();

                string controls = FormatControls(interactableItem);
                gameManager.ShowControlsInfo(true, controls);
                IsExamining = true;
            }
        }

        private string FormatControls(InteractableItem interactableItem)
        {
            Regex regex = new Regex(@"(?:\(((?:[^\(\)]*|\(([^)]*)\))*?)\))");
            MatchCollection matches = regex.Matches(ControlsFormat);

            bool includeUse = interactableItem.TakeFromExamine || interactableItem.IsPaper;
            bool includeRotate = interactableItem.ExamineRotate != InteractableItem.ExamineRotateEnum.Static;
            bool includeZoom = interactableItem.UseExamineZooming;

            string result = ControlsFormat;
            List<string> usedControls = new();

            if (matches.Count > 0)
            {
                // match [0] = put back
                Match defaultMatch = matches[0];
                string putBack = defaultMatch.Groups[1].Value;
                usedControls.Add(putBack);

                // match [1] = use or rotate control
                if (includeUse)
                {
                    Match match = matches[1];
                    string group1 = match.Groups[1].Value;
                    string group2 = match.Groups[2].Value;

                    string[] group3 = group2.Split(',');
                    string newText;

                    if (interactableItem.IsPaper) newText = group1.Replace($"({group2})", group3[0]);
                    else newText = group1.Replace($"({group2})", group3[1]);

                    usedControls.Add(newText.Trim());
                }

                // match [2] = rotate control
                if (includeRotate)
                {
                    Match match = matches[2];
                    string text = match.Groups[1].Value;
                    usedControls.Add(text);
                }

                // match [3] = zoom control
                if (includeZoom)
                {
                    Match match = matches[3];
                    string text = match.Groups[1].Value;
                    usedControls.Add(text);
                }

                string spaces = new(' ', ControlsPadding);
                result = string.Join($"{spaces}{SpaceSeparator}{spaces}", usedControls);
            }

            return result;
        }

        private void ExamineObject(InteractableItem interactableItem)
        {
            if (interactableItem == null) return;
            currentExamine?.GameObject.SetLayerRecursively(interactController.InteractLayer);

            Vector3 controlOffset = Quaternion.LookRotation(MainCamera.transform.forward) * interactableItem.ControlPoint;
            Vector3 holdPosition = MainCamera.transform.position + MainCamera.transform.forward * interactableItem.ExamineDistance;

            examinedObjects.Push(currentExamine = new ExaminedObject()
            {
                InteractableItem = interactableItem,
                PutSettings = new ExaminePutter.PutSettings(interactableItem.transform, controlOffset, new ExaminePutter.PutCurve(PutPositionCurve)
                {
                    evalMultiply = PutPositionCurveMultiplier,
                    curveTime = PutPositionCurveTime
                },
                new ExaminePutter.PutCurve(PutRotationCurve)
                {
                    evalMultiply = PutRotationCurveMultiplier,
                    curveTime = PutRotationCurveTime,
                }, 
                examinedObjects.Count > 0),
                HoldPosition = holdPosition,
                StartPosition = interactableItem.transform.position,
                StartRotation = interactableItem.transform.rotation,
                ControlPoint = interactableItem.transform.position + controlOffset,
                ExamineDistance = interactableItem.ExamineDistance
            });

            if (interactableItem.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.isKinematic = true;
                rigidbody.useGravity = false;
            }

            foreach (Collider collider in interactableItem.GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(collider, PlayerCollider, true);
            }

            if (interactableItem.IsCustomExamine)
            {
                foreach (var col in interactableItem.CollidersEnable)
                {
                    col.enabled = true;
                }

                foreach (var col in interactableItem.CollidersDisable)
                {
                    col.enabled = false;
                }
            }

            if (interactableItem.ShowExamineTitle)
            {
                StopAllCoroutines();
                StartCoroutine(ExamineItemAndShowInfo(interactableItem));
            }

            if (interactableItem.ExamineType == InteractableItem.ExamineTypeEnum.CustomObject && interactableItem.ExamineHotspot.HotspotTransform != null)
            {
                // clear previous active hotspot
                if (examineHotspot != null)
                {
                    Destroy(examineHotspot.gameObject);
                    examineHotspot = null;
                }

                // add new hotspot
                GameObject hotspotGo = Instantiate(HotspotPrefab, Vector3.zero, Quaternion.identity, gameManager.ExamineHotspots);
                Image hotspotImage = hotspotGo.GetComponent<Image>();
                hotspotImage.Alpha(0f);
                examineHotspot = hotspotImage;
            }


            GameTools.PlayOneShot2D(transform.position, interactableItem.ExamineSound, "ExamineSound");
            interactableItem.gameObject.SetLayerRecursively(FocusLayer);
            interactableItem.OnExamineStartEvent?.Invoke();
            PlayerManager.PlayerItems.IsItemsUsable = false;
        }

        IEnumerator ExamineItemAndShowInfo(InteractableItem item)
        {
            bool isExamined = item.IsExamined;
            if (!isExamined)
            {
                yield return new WaitForSeconds(TimeToExamine);
                item.IsExamined = true;
            }

            string title = item.ExamineTitle;
            if (item.ExamineInventoryTitle)
            {
                Item inventoryItem = item.PickupItem.GetItem();
                title = inventoryItem.Title;
            }

            if (!isExamined)
            {
                SoundClip examineHintSound = ExamineHintSound;
                if (item.ExamineHintSound != null)
                    examineHintSound = item.ExamineHintSound;

                GameTools.PlayOneShot2D(transform.position, examineHintSound, "ExamineInfo");
            }

            gameManager.ShowExamineInfo(true, false, title);
        }

        private void PopExaminedObject()
        {
            ExaminedObject obj = examinedObjects.Pop();
            obj.InteractableItem.OnExamineEndEvent?.Invoke();

            // destroy an object if there are no other objects examined and the object is examined from the inventory
            if (examinedObjects.Count <= 0 && isInventoryExamine)
            {
                Destroy(obj.GameObject);
                isInventoryExamine = false;
            }
            // otherwise return the object to its original location
            else
            {
                obj.GameObject.AddComponent<ExaminePutter>().Put(obj.PutSettings);
            }

            // if the number of examined objects is greater than zero, peek the previous object
            if (examinedObjects.Count > 0)
            {
                currentExamine = examinedObjects.Peek();
                currentExamine.GameObject.SetLayerRecursively(FocusLayer);
            }
            // otherwise reset examined object and unlock player
            else
            {
                obj.GameObject.SetLayerRecursively(interactController.InteractLayer);
                gameManager.SetBlur(false, true);
                gameManager.FreezePlayer(false);
                gameManager.ShowPanel(GameManager.PanelType.MainPanel);
                gameManager.ShowControlsInfo(false, string.Empty);

                StopAllCoroutines();
                gameManager.ShowExamineInfo(false, true);
                PlayerManager.PlayerItems.IsItemsUsable = true;

                if (examineHotspot != null)
                {
                    var hotspot = obj.InteractableItem.ExamineHotspot;
                    if(!hotspot.KeepPressed && isHotspotPressed)
                    {
                        hotspot.HotspotAction?.Invoke();
                        isHotspotPressed = false;
                    }

                    Destroy(examineHotspot.gameObject);
                    examineHotspot = null;
                }

                IsExamining = false;
                currentExamine = null;
            }

            // if it's a custom examine, enable/disable custom colliders
            if (obj.InteractableItem.IsCustomExamine)
            {
                foreach (var col in obj.InteractableItem.CollidersEnable)
                {
                    col.enabled = false;
                }

                foreach (var col in obj.InteractableItem.CollidersDisable)
                {
                    col.enabled = true;
                }
            }

            // disable pointer
            if (isPointerShown) gameManager.HidePointer();
            gameManager.ShowPaperInfo(false, true);
            isReadingPaper = false;
            isPointerShown = false;
        }

        private void ExamineHold()
        {
            InteractableItem currentItem = currentExamine.InteractableItem;

            // hold position
            foreach (var obj in examinedObjects)
            {
                Vector3 holdPos = MainCamera.transform.position + MainCamera.transform.forward * obj.ExamineDistance;
                obj.HoldPosition = Vector3.Lerp(obj.HoldPosition, holdPos, Time.deltaTime * 5);
                float speedMultiplier = PickUpCurve.Evaluate(obj.t) * PickUpCurveMultiplier;
                obj.t = Mathf.SmoothDamp(obj.t, 1f, ref obj.Velocity, PickUpTime + speedMultiplier);
                obj.InteractableItem.transform.position = VectorE.QuadraticBezier(obj.StartPosition, obj.HoldPosition, obj.ControlPoint, obj.t);
            }

            // paper reading
            if (currentItem.IsPaper && !string.IsNullOrEmpty(currentItem.PaperText))
            {
                if (InputManager.ReadButtonOnce(GetInstanceID(), Controls.USE))
                {
                    isReadingPaper = !isReadingPaper;
                    gameManager.ShowPaperInfo(isReadingPaper, false, currentItem.PaperText);
                }
            }

            // rotation
            if (currentItem.UseFaceRotation && currentExamine.t <= 0.99f)
            {
                Vector3 faceRotation = currentItem.FaceRotation;
                Quaternion faceRotationQ = Quaternion.LookRotation(MainCamera.transform.forward) * Quaternion.Euler(faceRotation);
                currentItem.transform.rotation = Quaternion.Slerp(currentExamine.StartRotation, faceRotationQ, currentExamine.t);
            }
            else if (!isPointerShown && !isReadingPaper && InputManager.ReadButton(Controls.FIRE))
            {
                Vector2 rotateValue = InputManager.ReadInput<Vector2>(Controls.LOOK) * RotateMultiplier;
                examineRotate = Vector2.SmoothDamp(examineRotate, rotateValue, ref rotateVelocity, RotateTime);

                switch (currentItem.ExamineRotate)
                {
                    case InteractableItem.ExamineRotateEnum.Horizontal:
                        currentItem.transform.Rotate(MainCamera.transform.up, -examineRotate.x, Space.World);
                        break;
                    case InteractableItem.ExamineRotateEnum.Vertical:
                        currentItem.transform.Rotate(MainCamera.transform.right, examineRotate.y, Space.World);
                        break;
                    case InteractableItem.ExamineRotateEnum.Both:
                        currentItem.transform.Rotate(MainCamera.transform.up, -examineRotate.x, Space.World);
                        currentItem.transform.Rotate(MainCamera.transform.right, examineRotate.y, Space.World);
                        break;
                }
            }

            // examine zooming
            if (!isReadingPaper && currentItem.UseExamineZooming)
            {
                Vector2 scroll = InputManager.ReadInput<Vector2>(Controls.SCROLL_WHEEL);
                float nextZoom = currentExamine.ExamineDistance + scroll.normalized.y * ZoomMultiplier;
                currentExamine.ExamineDistance = Mathf.Clamp(nextZoom, currentItem.ExamineZoomLimits.RealMin, currentItem.ExamineZoomLimits.RealMax);
            }

            // pointer
            if (!isReadingPaper && currentItem.AllowCursorExamine && InputManager.ReadButtonOnce(GetInstanceID(), Controls.SHOW_CURSOR))
            {
                isPointerShown = !isPointerShown;

                if (isPointerShown)
                {
                    gameManager.ShowPointer(FocusCullLayes, FocusLayer, (hit, _) =>
                    {
                        if (hit.collider.gameObject.TryGetComponent(out InteractableItem interactableItem))
                        {
                            ExamineObject(interactableItem);
                            gameManager.HidePointer();
                            isPointerShown = false;
                        }
                    });
                }
                else
                {
                    gameManager.HidePointer();
                }
            }

            // examine hotspots
            if(currentItem.ExamineType == InteractableItem.ExamineTypeEnum.CustomObject 
                && examineHotspot != null 
                && currentItem.ExamineHotspot.HotspotTransform != null 
                && currentExamine.t > 0.99f)
            {
                var hotspot = currentItem.ExamineHotspot;
                Vector3 mainCamera = MainCamera.transform.position;
                Vector3 hotspotPos = currentItem.ExamineHotspot.HotspotTransform.position;

                Vector3 screenPointPos = MainCamera.WorldToScreenPoint(hotspotPos);
                examineHotspot.transform.position = screenPointPos;

                Vector3 direction = hotspotPos - mainCamera;
                direction -= direction.normalized * 0.01f;

                float alpha = examineHotspot.color.a;
                {
                    if (!Physics.Raycast(mainCamera, direction, direction.magnitude, FocusCullLayes, QueryTriggerInteraction.Ignore) && currentItem.ExamineHotspot.Enabled)
                    {
                        alpha = Mathf.MoveTowards(alpha, 1f, Time.deltaTime * 10f);
                        if (InputManager.ReadButtonOnce(this, Controls.USE))
                        {
                            hotspot.HotspotAction?.Invoke();
                            if(!hotspot.KeepPressed)
                                isHotspotPressed = !isHotspotPressed;
                        }
                    }
                    else
                    {
                        alpha = Mathf.MoveTowards(alpha, 0f, Time.deltaTime * 10f);
                    }
                }
                examineHotspot.Alpha(alpha);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(InventoryPosition, 0.01f);
            if(ShowLabels) GizmosE.DrawCenteredLabel(InventoryPosition, "Inventory Position");

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(DropPosition, 0.01f);
            if (ShowLabels) GizmosE.DrawCenteredLabel(DropPosition, "Drop Position");
        }
    }
}