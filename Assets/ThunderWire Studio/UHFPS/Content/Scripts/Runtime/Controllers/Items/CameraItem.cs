using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UHFPS.Input;
using UHFPS.Tools;
using Newtonsoft.Json.Linq;

namespace UHFPS.Runtime
{
    public class CameraItem : PlayerItemBehaviour
    {
        public ItemGuid BatteryInventoryItem;
        public Light CameraLight;
        public Volume CameraVolume;
        public AudioSource CameraAudio;

        public ushort BatteryLife = 320;
        public Percentage BatteryPercentage = 100;
        public Percentage BatteryLowPercent = 20;
        public float LightIntensity = 1f;
        public Color BatteryFullColor = Color.white;
        public Color BatteryLowColor = Color.red;

        public float LightZoomRange = 24;
        public float CameraZoomFOV = 30f;
        public float CameraZoomSpeed = 5f;

        public string CameraShow = "CameraShow";
        public string CameraHide = "CameraHide";
        public string CameraReload = "CameraReload";
        public float CameraShowFade = 0.2f;
        public float CameraHideFade = 0.2f;

        public SoundClip CameraEquip;
        public SoundClip CameraUnequip;
        public SoundClip CameraZoomIn;
        public SoundClip CameraZoomOut;

        private GameManager gameManager;
        private ObjectiveManager objectiveManager;

        private GameObject cameraOverlay;
        private Slider cameraZoom;
        private Image batteryIcon;
        private Image batteryFill;
        private TMPro.TMP_Text recordingText;

        private CustomStopwatch stopwatch = new();

        private bool isEquipped;
        private bool isBusy;

        private float defaultLightRange;
        private float defaultZoom;
        private float currentZoom;

        public float batteryEnergy;
        private float currentBattery;
        private Color batteryColor;

        public override string Name => "Camera";

        private void Awake()
        {
            gameManager = GameManager.Instance;
            objectiveManager = ObjectiveManager.Instance;

            var behaviours = gameManager.GraphicReferences.Value["Camera"];
            cameraOverlay = behaviours[0].gameObject;
            cameraZoom = (Slider)behaviours[1];
            batteryIcon = (Image)behaviours[2];
            batteryFill = (Image)behaviours[3];
            recordingText = (TMPro.TMP_Text)behaviours[4];

            defaultZoom = PlayerManager.MainVirtualCamera.m_Lens.FieldOfView;
            defaultLightRange = CameraLight.range;
            currentZoom = defaultZoom;

            if (!SaveGameManager.GameWillLoad)
            {
                currentBattery = BatteryPercentage.From(BatteryLife);
                UpdateBattery();

                batteryColor = batteryEnergy > BatteryLowPercent.Ratio()
                    ? BatteryFullColor : BatteryLowColor;

                batteryIcon.color = batteryColor;
                batteryFill.color = batteryColor;
            }
        }

        public override void OnUpdate()
        {
            if (!isEquipped) return;

            // camera zoom
            float zoomT = Mathf.InverseLerp(defaultZoom, CameraZoomFOV, currentZoom);
            if (InputManager.ReadButton(Controls.ADS))
            {
                currentZoom = Mathf.MoveTowards(currentZoom, CameraZoomFOV, Time.deltaTime * CameraZoomSpeed * 10);
                if (!isBusy)
                {
                    if (zoomT < 1) CameraAudio.SetSoundClip(CameraZoomIn, play: true);
                    else CameraAudio.Stop();
                }
            }
            else
            {
                currentZoom = Mathf.MoveTowards(currentZoom, defaultZoom, Time.deltaTime * CameraZoomSpeed * 10);
                if (!isBusy)
                {
                    if (zoomT > 0) CameraAudio.SetSoundClip(CameraZoomOut, play: true);
                    else CameraAudio.Stop();
                }
            }

            PlayerManager.MainVirtualCamera.m_Lens.FieldOfView = currentZoom;
            CameraLight.range = Mathf.Lerp(defaultLightRange, LightZoomRange, zoomT);
            cameraZoom.value = zoomT;

            if (isBusy) return;

            UpdateRecordingTime();

            // battery life
            currentBattery = currentBattery > 0 ? currentBattery -= Time.deltaTime : 0;
            UpdateBattery();

            // battery icon
            batteryColor = batteryEnergy > BatteryLowPercent.Ratio()
                ? Color.Lerp(batteryColor, BatteryFullColor, Time.deltaTime * 10)
                : Color.Lerp(batteryColor, BatteryLowColor, Time.deltaTime * 10);

            batteryIcon.color = batteryColor;
            batteryFill.color = batteryColor;
        }

        private void UpdateRecordingTime()
        {
            TimeSpan timeSpan = stopwatch.Elapsed;
            recordingText.text = timeSpan.ToString(@"hh\:mm\:ss\:ff");
        }

        private void UpdateBattery()
        {
            batteryEnergy = Mathf.InverseLerp(0, BatteryLife, currentBattery);
            batteryFill.fillAmount = batteryEnergy;
            CameraLight.intensity = Mathf.Lerp(0, LightIntensity, batteryEnergy);
        }

        public override bool IsBusy() => isBusy;

        public override bool IsEquipped() => ItemObject.activeSelf || isEquipped;

        public override bool CanCombine() => isEquipped && !isBusy;

        public override void OnItemCombine(InventoryItem combineItem)
        {
            if (combineItem.ItemGuid != BatteryInventoryItem || !isEquipped)
                return;

            Inventory.Instance.RemoveItem(combineItem, 1);
            StartCoroutine(ReloadCameraBattery());
            isBusy = true;
        }

        IEnumerator ReloadCameraBattery()
        {
            yield return gameManager.StartBackgroundFade(false);

            ItemObject.SetActive(true);
            SetCameraEffects(false);
            CameraLight.gameObject.SetActive(false);
            cameraOverlay.SetActive(false);

            Animator.SetTrigger(CameraReload);
            yield return new WaitForSeconds(CameraHideFade);
            StartCoroutine(gameManager.StartBackgroundFade(true));

            yield return new WaitForAnimatorClip(Animator, CameraReload, CameraShowFade);

            currentBattery = new Percentage(100).From(BatteryLife);
            UpdateBattery();

            yield return gameManager.StartBackgroundFade(false);

            ItemObject.SetActive(false);
            SetCameraEffects(true);
            CameraLight.gameObject.SetActive(true);
            cameraOverlay.SetActive(true);

            yield return gameManager.StartBackgroundFade(true);
            isBusy = false;
        }

        public override void OnItemSelect()
        {
            ItemObject.SetActive(true);
            StartCoroutine(ShowCamera());
            CameraAudio.SetSoundClip(CameraEquip, play: true);
            isBusy = true;
        }

        IEnumerator ShowCamera()
        {
            Animator.SetTrigger(CameraShow);

            yield return new WaitForAnimatorClip(Animator, CameraShow, CameraShowFade);
            yield return gameManager.StartBackgroundFade(false);

            ItemObject.SetActive(false);
            SetCameraEffects(true);
            CameraLight.gameObject.SetActive(true);
            cameraOverlay.SetActive(true);
            objectiveManager.ObjectivesGroup.alpha = 0f;

            yield return gameManager.StartBackgroundFade(true);
            stopwatch.Start();
            isEquipped = true;
            isBusy = false;
        }

        public override void OnItemDeselect()
        {
            StartCoroutine(HideCamera());
            stopwatch.Stop();
            isBusy = true;
        }

        IEnumerator HideCamera()
        {
            yield return gameManager.StartBackgroundFade(false);

            ItemObject.SetActive(true);
            SetCameraEffects(false);
            Animator.SetTrigger(CameraHide);
            CameraLight.gameObject.SetActive(false);
            cameraOverlay.SetActive(false);
            objectiveManager.ObjectivesGroup.alpha = 1f;

            yield return new WaitForSeconds(CameraHideFade);
            StartCoroutine(gameManager.StartBackgroundFade(true));
            yield return new WaitForAnimatorClip(Animator, CameraHide);

            ItemObject.SetActive(false);
            isEquipped = false;
            isBusy = false;
        }

        private void SetCameraEffects(bool state)
        {
            CameraVolume.gameObject.SetActive(state);
        }

        public override void OnItemActivate()
        {
            objectiveManager.ObjectivesGroup.alpha = 0f;
            CameraLight.gameObject.SetActive(true);
            cameraOverlay.SetActive(true);
            SetCameraEffects(true);

            stopwatch.Start();
            ItemObject.SetActive(false);
            isEquipped = true;
            isBusy = false;
        }

        public override void OnItemDeactivate()
        {
            objectiveManager.ObjectivesGroup.alpha = 1f;
            CameraLight.gameObject.SetActive(false);
            cameraOverlay.SetActive(false);
            SetCameraEffects(false);

            stopwatch.Stop();
            ItemObject.SetActive(false);
            isEquipped = false;
            isBusy = false;
        }

        public override StorableCollection OnCustomSave()
        {
            return new StorableCollection()
            {
                { "recordingTime", stopwatch.ElapsedTicks },
                { "batteryEnergy", currentBattery }
            };
        }

        public override void OnCustomLoad(JToken data)
        {
            long ticks = data["recordingTime"].ToObject<long>();
            TimeSpan stopwatchOffset = TimeSpan.FromTicks(ticks);
            stopwatch = new CustomStopwatch(stopwatchOffset);

            currentBattery = data["batteryEnergy"].ToObject<float>();
            UpdateBattery();

            batteryColor = batteryEnergy > BatteryLowPercent.Ratio()
                ? BatteryFullColor : BatteryLowColor;

            batteryIcon.color = batteryColor;
            batteryFill.color = batteryColor;
        }
    }
}