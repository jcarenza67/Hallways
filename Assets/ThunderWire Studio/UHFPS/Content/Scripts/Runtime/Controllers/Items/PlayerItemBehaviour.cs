using UnityEngine;
using UHFPS.Tools;
using Newtonsoft.Json.Linq;

namespace UHFPS.Runtime
{
    public abstract class PlayerItemBehaviour : MonoBehaviour, ISaveableCustom
    {
        private Animator animator;
        private PlayerManager playerManager;
        private PlayerStateMachine playerStateMachine;
        private LookController lookController;
        private ExamineController examineController;
        private Vector3 wallHitVel;

        public bool EnableWallDetection = true;
        public bool EnableSway = true;

        // wall detection
        public LayerMask WallHitMask;
        public float WallHitRayDistance = 0.5f;
        public float WallHitRayRadius = 0.3f;
        public float WallHitAmount = 1f;
        public float WallHitTime = 0.2f;
        public Vector3 WallHitRayOffset;
        public bool ShowRayGizmos = true;

        // look sway
        public float LookSwayAmount = 10f;
        public float ADSLookSwayAmount = 10f;
        public float WalkSidewaySway = 10f;
        public float WalkForwardSway = 10f;
        public float MaxLookSwayAmount = 10f;
        public float SwaySpeed = 5f;

        protected bool isSwayADS = false;

        /// <summary>
        /// The pivot point of the item object that will be used for the sway effect.
        /// </summary>
        [field: SerializeField]
        public Transform SwayPivot { get; set; }

        /// <summary>
        /// The object of the item which will be enabled or disabled, usually a child object.
        /// </summary>
        [field: SerializeField]
        public GameObject ItemObject { get; set; }

        /// <summary>
        /// Animator component of the Item Object.
        /// </summary>
        public Animator Animator
        {
            get
            {
                if(animator == null)
                    animator = ItemObject.GetComponentInChildren<Animator>();

                return animator;
            }
        }

        /// <summary>
        /// PlayerManager component.
        /// </summary>
        public PlayerManager PlayerManager
        {
            get
            {
                if (playerManager == null)
                    playerManager = transform.root.GetComponent<PlayerManager>();

                return playerManager;
            }
        }

        /// <summary>
        /// PlayerStateMachine component.
        /// </summary>
        public PlayerStateMachine PlayerStateMachine
        {
            get
            {
                if (playerStateMachine == null)
                    playerStateMachine = transform.root.GetComponent<PlayerStateMachine>();

                return playerStateMachine;
            }
        }

        /// <summary>
        /// LookController component.
        /// </summary>
        public LookController LookController
        {
            get
            {
                if (lookController == null)
                    lookController = transform.GetComponentInParent<LookController>();

                return lookController;
            }
        }

        /// <summary>
        /// ExamineController component.
        /// </summary>
        public ExamineController ExamineController
        {
            get
            {
                if (examineController == null)
                    examineController = transform.GetComponentInParent<ExamineController>();

                return examineController;
            }
        }

        /// <summary>
        /// PlayerItemsManager component.
        /// </summary>
        public PlayerItemsManager PlayerItems
        {
            get => PlayerManager.PlayerItems;
        }

        /// <summary>
        /// Check if the item is interactive. False, for example when the inventory is opened, object is dragged etc.
        /// </summary>
        public bool CanInteract => PlayerItems.CanInteract;

        /// <summary>
        /// The name of the item that will be displayed in the list.
        /// </summary>
        public virtual string Name => "Item";

        /// <summary>
        /// Check whether the item can be switched.
        /// </summary>
        public virtual bool IsBusy() => false;

        /// <summary>
        /// Check whether the item is equipped.
        /// </summary>
        public virtual bool IsEquipped() => ItemObject.activeSelf;

        /// <summary>
        /// Check whether the item can be combined in inventory.
        /// </summary>
        public virtual bool CanCombine() => false;

        private void Update()
        {
            if (EnableWallDetection)
            {
                Vector3 forward = PlayerItems.transform.forward;
                Vector3 origin = PlayerItems.transform.TransformPoint(WallHitRayOffset);

                if (Physics.SphereCast(origin, WallHitRayRadius, forward, out RaycastHit hit, WallHitRayDistance, WallHitMask))
                    OnItemBlocked(hit.distance, true);
                else
                    OnItemBlocked(0f, false);
            }

            if (EnableSway && SwayPivot != null)
            {
                float magnitude = PlayerStateMachine.PlayerCollider.velocity.magnitude;
                float inputX = PlayerStateMachine.Input.x * -1;
                float inputY = PlayerStateMachine.Input.y;

                float vertical = LookController.DeltaInput.y * -1;
                float horizontal = LookController.DeltaInput.x;

                vertical *= isSwayADS ? ADSLookSwayAmount : LookSwayAmount;
                horizontal *= isSwayADS ? ADSLookSwayAmount : LookSwayAmount;

                vertical = Mathf.Clamp(vertical, -MaxLookSwayAmount, MaxLookSwayAmount);
                horizontal = Mathf.Clamp(horizontal, -MaxLookSwayAmount, MaxLookSwayAmount);

                magnitude = Mathf.Clamp(magnitude, -1, 1);
                inputY = Mathf.Clamp01(inputY);

                float sideway = inputX * magnitude * WalkSidewaySway * (isSwayADS ? 0 : 1);
                float forward = inputY * magnitude * WalkForwardSway * (isSwayADS ? 0 : 1);

                Vector3 final = new Vector3(vertical + forward, horizontal, sideway);
                SwayPivot.localRotation = Quaternion.Slerp(SwayPivot.localRotation, Quaternion.Euler(final), Time.deltaTime * SwaySpeed);
            }

            OnUpdate();
        }

        /// <summary>
        /// Will be called when the ray going from the camera hits the wall to prevent the player item from being clipped.
        /// </summary>
        public virtual void OnItemBlocked(float hitDistance, bool blocked)
        {
            float value = GameTools.Remap(0f, WallHitRayDistance, 0f, 1f, hitDistance);
            Vector3 backward = Vector3.back * WallHitAmount;
            Vector3 result = Vector3.Lerp(backward, Vector3.zero, blocked ? value : 1f);
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, result, ref wallHitVel, WallHitTime);
        }

        /// <summary>
        /// Will be called every frame like the classic Update() function. 
        /// </summary>
        public virtual void OnUpdate() { }

        /// <summary>
        /// Will be called when a combinable item is combined with this inventory item.
        /// </summary>
        public virtual void OnItemCombine(InventoryItem combineItem) { }

        /// <summary>
        /// Will be called when PlayerItemsManager selects an item.
        /// </summary>
        public abstract void OnItemSelect();

        /// <summary>
        /// Will be called when PlayerItemsManager deselects an item.
        /// </summary>
        public abstract void OnItemDeselect();

        /// <summary>
        /// Will be called when PlayerItemsManager activates an item.
        /// </summary>
        public abstract void OnItemActivate();

        /// <summary>
        /// Will be called when PlayerItemsManager deactivates an item.
        /// </summary>
        public abstract void OnItemDeactivate();

        public virtual void OnDrawGizmosSelected()
        {
            bool selected = false;

#if UNITY_EDITOR
            selected = UnityEditor.Selection.activeGameObject == gameObject;
#endif

            if (ShowRayGizmos && EnableWallDetection && selected)
            {
                Vector3 forward = PlayerItems.transform.forward;
                Vector3 origin = PlayerItems.transform.TransformPoint(WallHitRayOffset);
                Vector3 p2 = origin + forward * WallHitRayDistance;

                Gizmos.color = Color.yellow;
                GizmosE.DrawWireCapsule(origin, p2, WallHitRayRadius);
            }
        }

        public virtual StorableCollection OnCustomSave()
        {
            return new();
        }

        public virtual void OnCustomLoad(JToken data) { }
    }
}