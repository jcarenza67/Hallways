using UnityEngine;
using UnityEngine.Events;
using UHFPS.Tools;

namespace UHFPS.Runtime
{
    public class LockpickInteract : MonoBehaviour, IDynamicUnlock, IInteractStart
    {
        public GameObject LockpickModel;

        [Range(-90f, 90f)]
        public float UnlockAngle;
        public bool RandomUnlockAngle;
        public bool IsDynamicUnlockComponent;

        public Vector3 LockpickRotation;
        public float LockpickDistance;
        public GString ControlsFormat;

        public ItemGuid BobbyPinItem;
        public MinMax BobbyPinLimits;

        public float BobbyPinUnlockDistance = 0.1f;
        public float BobbyPinLifetime = 2;
        public bool UnbreakableBobbyPin;

        [Range(0f, 90f)]
        public float KeyholeMaxTestRange = 20;
        public float KeyholeUnlockTarget = 0.1f;

        public UnityEvent OnUnlock;

        private Camera MainCamera => playerPresence.PlayerCamera;
        private PlayerPresenceManager playerPresence;
        private DynamicObject dynamicObject;
        private bool isUnlocked;

        private void Awake()
        {
            playerPresence = PlayerPresenceManager.Instance;
            if(RandomUnlockAngle) UnlockAngle = Mathf.Floor(GameTools.Random(BobbyPinLimits));
        }

        private void Start()
        {
            ControlsFormat.SubscribeGlocMany();
        }

        public void InteractStart()
        {
            if (IsDynamicUnlockComponent || isUnlocked) return;
            AttemptToUnlock();
        }

        public void OnTryUnlock(DynamicObject dynamicObject)
        {
            if (!IsDynamicUnlockComponent || isUnlocked) return;
            this.dynamicObject = dynamicObject;
            AttemptToUnlock();
        }

        public void AttemptToUnlock()
        {
            Vector3 holdPosition = MainCamera.transform.position + MainCamera.transform.forward * LockpickDistance;
            Quaternion faceRotation = Quaternion.LookRotation(MainCamera.transform.forward) * Quaternion.Euler(LockpickRotation);
            GameObject lockpickObj = Instantiate(LockpickModel, holdPosition, faceRotation, playerPresence.PlayerManager.MainVirtualCamera.transform);
            LockpickComponent lockpickComponent = lockpickObj.GetComponent<LockpickComponent>();

            playerPresence.FreezePlayer(true);
            playerPresence.GameManager.LockInput(true);
            playerPresence.GameManager.SetBlur(true, true);
            playerPresence.GameManager.DisableAllGamePanels();
            playerPresence.GameManager.ShowControlsInfo(true, ControlsFormat);
            lockpickComponent.SetLockpick(this);
        }

        public void Unlock()
        {
            if (isUnlocked) 
                return;

            if (IsDynamicUnlockComponent && dynamicObject != null) 
                dynamicObject.TryUnlockResult(true);
            else OnUnlock?.Invoke();

            isUnlocked = true;
        }
    }
}