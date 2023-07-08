using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace UHFPS.Runtime
{
    public class FloatingIconModule : ManagerModule
    {
        public sealed class FloatingIconData
        {
            public FloatingIcon floatingIcon;
            public Transform iconTranform;
            public GameObject targetObject;
            public Vector3 lastPosition;
            public bool wasDisabled;

            public void UpdateLastPosition()
            {
                if(targetObject != null)
                    lastPosition = targetObject.transform.position;
            }
        }

        public override string ToString() => "FloatingIcon";

        public GameObject FloatingIconPrefab;

        [Header("Settings")]
        public LayerMask CullLayers;
        public float DistanceShow = 4;
        public float DistanceHide = 4;
        public float FadeInTime = 0.2f;
        public float FadeOutTime = 0.05f;

        private readonly List<FloatingIconData> uiFloatingIcons = new List<FloatingIconData>();
        private List<GameObject> worldFloatingIcons = new List<GameObject>();

        public override void OnAwake()
        {
            worldFloatingIcons = (from interactable in FindObjectsOfType<InteractableItem>()
                                 where interactable.ShowFloatingIcon
                                 select interactable.gameObject).ToList();

            var customIcons = FindObjectsOfType<FloatingIconObject>().Select(x => x.gameObject);
            worldFloatingIcons.AddRange(customIcons);
        }

        /// <summary>
        /// Add object to floating icons list.
        /// </summary>
        public void AddFloatingIcon(GameObject gameObject)
        {
            worldFloatingIcons.Add(gameObject);
        }

        /// <summary>
        /// Remove object from floating icons list.
        /// </summary>
        public void RemoveFloatingIcon(GameObject gameObject)
        {
            worldFloatingIcons.Remove(gameObject);
        }

        public override void OnUpdate()
        {
            for (int i = 0; i < worldFloatingIcons.Count; i++)
            {
                GameObject obj = worldFloatingIcons[i];

                if(obj == null)
                {
                    worldFloatingIcons.RemoveAt(i);
                    continue;
                }

                if (Vector3.Distance(PlayerPresence.PlayerCamera.transform.position, obj.transform.position) <= DistanceShow)
                {
                    if (!uiFloatingIcons.Any(x => x.targetObject == obj) && VisibleByCamera(obj) && IsIconUpdatable(obj))
                    {
                        Vector3 screenPoint = PlayerPresence.PlayerCamera.WorldToScreenPoint(obj.transform.position);
                        GameObject floatingIconObj = Instantiate(FloatingIconPrefab, screenPoint, Quaternion.identity, GameManager.FloatingIcons);
                        FloatingIcon icon = floatingIconObj.AddComponent<FloatingIcon>();

                        uiFloatingIcons.Add(new FloatingIconData()
                        {
                            floatingIcon = icon,
                            iconTranform = floatingIconObj.transform,
                            targetObject = obj,
                            lastPosition = obj.transform.position
                        });

                        icon.FadeIn(FadeInTime);
                    }
                }
            }

            for (int i = 0; i < uiFloatingIcons.Count; i++)
            {
                FloatingIconData item = uiFloatingIcons[i];
 
                if (item.iconTranform == null)
                {
                    uiFloatingIcons.RemoveAt(i);
                    continue;
                }

                if (IsIconUpdatable(item.targetObject))
                {
                    // update last object position
                    item.UpdateLastPosition();

                    // update distance
                    float distance = Vector3.Distance(PlayerPresence.PlayerCamera.transform.position, item.lastPosition);

                    // set point position
                    Vector3 screenPoint = PlayerPresence.PlayerCamera.WorldToScreenPoint(item.lastPosition);
                    item.iconTranform.position = screenPoint;

                    if (item.targetObject == null)
                    {
                        // destroy the floating icon if the target object is removed
                        Destroy(item.iconTranform.gameObject);
                        uiFloatingIcons.RemoveAt(i);
                    }
                    else if (distance > DistanceHide)
                    {
                        // destroy and remove the item if it is out of distance
                        item.floatingIcon.FadeOut(FadeOutTime);
                    }
                    else if (!VisibleByCamera(item.targetObject))
                    {
                        // disable an item if it is behind an object
                        item.iconTranform.gameObject.SetActive(false);
                        item.wasDisabled = true;
                    }
                    else if (item.wasDisabled)
                    {
                        // enable an object if it is visible when it has been disabled
                        item.floatingIcon.FadeIn(FadeInTime);
                        item.iconTranform.gameObject.SetActive(true);
                        item.wasDisabled = false;
                    }
                }
                else
                {
                    // destroy the floating icon if the target object is disabled
                    Destroy(item.iconTranform.gameObject);
                    uiFloatingIcons.RemoveAt(i);
                }
            }
        }

        private bool VisibleByCamera(GameObject obj)
        {
            if (obj != null)
            {
                bool linecastResult = Physics.Linecast(PlayerPresence.PlayerCamera.transform.position, obj.transform.position, out RaycastHit hit, CullLayers);

                if (!linecastResult || linecastResult && hit.collider.gameObject == obj)
                {
                    Vector3 screenPoint = PlayerPresence.PlayerCamera.WorldToViewportPoint(obj.transform.position);
                    return screenPoint.x >= 0 && screenPoint.x <= 1 && screenPoint.y >= 0 && screenPoint.y <= 1 && screenPoint.z > 0;
                }
            }

            return false;
        }

        private bool IsIconUpdatable(GameObject targetObj)
        {
            return targetObj != null && (targetObj.activeSelf || targetObj.activeSelf && targetObj.TryGetComponent(out Renderer renderer) && renderer.enabled);
        }
    }
}