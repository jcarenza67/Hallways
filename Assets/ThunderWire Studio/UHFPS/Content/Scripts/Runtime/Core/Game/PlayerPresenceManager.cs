using System;
using System.Collections;
using UnityEngine;
using Cinemachine;
using UHFPS.Tools;

namespace UHFPS.Runtime
{
    public class PlayerPresenceManager : Singleton<PlayerPresenceManager>
    {
        public enum UnlockType 
        {
            /// <summary>
            /// Player will be unlocked at the start or after the game state is loaded.
            /// </summary>
            Automatically,

            /// <summary>
            /// Player will be unlocked after calling the <b>UnlockPlayer()</b> function.
            /// </summary>
            Manually
        }

        public UnlockType PlayerUnlockType = UnlockType.Automatically;
        public GameObject Player;

        public float WaitFadeOutTime = 0.5f;
        public float FadeOutSpeed = 3f;

        private LookController playerLook;
        private PlayerComponent[] playerComponents;
        private GameManager gameManager;
        private GameObject activeCamera;

        public bool PlayerIsUnlocked;
        public bool GameStateIsLoaded;
        public bool IsCameraSwitched;

        private bool isBackgroundFadedOut;

        private PlayerManager playerManager;
        public PlayerManager PlayerManager
        {
            get
            {
                if (playerManager == null)
                    playerManager = Player.GetComponent<PlayerManager>();

                return playerManager;
            }
        }

        private PlayerStateMachine stateMachine;
        public PlayerStateMachine StateMachine
        {
            get
            {
                if (stateMachine == null)
                    stateMachine = Player.GetComponent<PlayerStateMachine>();

                return stateMachine;
            }
        }

        /// <summary>
        /// Check if player is unlocked and the active camera is player camera.
        /// </summary>
        public bool IsUnlockedAndCamera => PlayerIsUnlocked && !IsCameraSwitched;

        public Camera PlayerCamera => PlayerManager.MainCamera;

        public CinemachineVirtualCamera PlayerVirtualCamera => PlayerManager.MainVirtualCamera;

        public GameManager GameManager => gameManager;

        private void OnEnable()
        {
            SaveGameManager.Instance.OnGameLoaded += (state) =>
            {
                if (!state) 
                    return;

                UnlockPlayer();
                GameStateIsLoaded = true;
            };
        }

        private void Awake()
        {
            gameManager = GetComponent<GameManager>();
            playerLook = Player.GetComponentInChildren<LookController>(true);
            playerComponents = Player.GetComponentsInChildren<PlayerComponent>(true);

            // keep player frozen at start
            FreezePlayer(true);
        }

        private void Start()
        {
            if (!SaveGameManager.GameWillLoad || !SaveGameManager.GameStateExist)
            {
                Vector3 rotation = Player.transform.eulerAngles;
                Player.transform.rotation = Quaternion.identity;
                playerLook.rotation.x = rotation.y;

                if(PlayerUnlockType == UnlockType.Automatically)
                    UnlockPlayer();
            }
        }

        public T Component<T>()
        {
            return Player.GetComponentInChildren<T>(true);
        }

        public T[] Components<T>()
        {
            return Player.GetComponentsInChildren<T>(true);
        }

        public void FreezePlayer(bool freeze, bool showCursor = false)
        {
            GameTools.ShowCursor(!showCursor, showCursor);

            foreach (var component in playerComponents)
            {
                component.SetEnabled(!freeze);
            }
        }

        public void FadeBackground(bool fadeOut, Action onBackgroundFade)
        {
            StartCoroutine(StartFadeBackground(fadeOut, onBackgroundFade));
        }

        IEnumerator StartFadeBackground(bool fadeOut, Action onBackgroundFade)
        {
            yield return gameManager.StartBackgroundFade(fadeOut, WaitFadeOutTime, FadeOutSpeed);
            isBackgroundFadedOut = fadeOut;
            onBackgroundFade?.Invoke();
        }

        public void UnlockPlayer()
        {
            StartCoroutine(DoUnlockPlayer());
        }

        private IEnumerator DoUnlockPlayer()
        {
            if(!isBackgroundFadedOut)
                yield return gameManager.StartBackgroundFade(true, WaitFadeOutTime, FadeOutSpeed);

            FreezePlayer(false);
            PlayerIsUnlocked = true;
        }

        public (Vector3 position, Vector2 rotation) GetPlayerTransform()
        {
            return (Player.transform.position, playerLook.rotation);
        }

        public void SetPlayerTransform(Vector3 position, Vector2 rotation)
        {
            Player.transform.SetPositionAndRotation(position, Quaternion.identity);
            playerLook.rotation = rotation;
            Physics.SyncTransforms(); // sync position change to character controller
        }

        public void SwitchActiveCamera(GameObject virtualCameraObj, float fadeSpeed, Action onBackgroundFade)
        {
            StartCoroutine(SwitchCamera(virtualCameraObj, fadeSpeed, onBackgroundFade));
            IsCameraSwitched = true;
        }

        public void SwitchToPlayerCamera(float fadeSpeed, Action onBackgroundFade)
        {
            StartCoroutine(SwitchCamera(null, fadeSpeed, onBackgroundFade));
        }

        public IEnumerator SwitchCamera(GameObject cameraObj, float fadeSpeed)
        {
            yield return gameManager.StartBackgroundFade(false, fadeSpeed: fadeSpeed);
            playerManager.MainVirtualCamera.gameObject.SetActive(cameraObj == null);
            
            if(cameraObj != null) playerManager.PlayerItems.DeactivateCurrentItem();
            else playerManager.PlayerItems.ActivatePreviouslyDeactivatedItem();

            if (activeCamera != null) activeCamera.SetActive(false);
            if (cameraObj != null) cameraObj.SetActive(cameraObj != null);
            activeCamera = cameraObj;

            yield return new WaitForEndOfFrame();
            yield return gameManager.StartBackgroundFade(true, fadeSpeed: fadeSpeed);

            IsCameraSwitched = cameraObj != null; // check if camera switched to player camera
        }

        private IEnumerator SwitchCamera(GameObject cameraObj, float fadeSpeed, Action onBackgroundFade)
        {
            yield return gameManager.StartBackgroundFade(false, fadeSpeed: fadeSpeed);
            playerManager.MainVirtualCamera.gameObject.SetActive(cameraObj == null);

            if (cameraObj != null) playerManager.PlayerItems.DeactivateCurrentItem();
            else playerManager.PlayerItems.ActivatePreviouslyDeactivatedItem();

            if (activeCamera != null) activeCamera.SetActive(false);
            if(cameraObj != null) cameraObj.SetActive(cameraObj != null);
            activeCamera = cameraObj;

            onBackgroundFade?.Invoke();

            yield return new WaitForEndOfFrame();
            yield return gameManager.StartBackgroundFade(true, fadeSpeed: fadeSpeed);

            IsCameraSwitched = cameraObj != null; // check if camera switched to player camera
        }
    }
}