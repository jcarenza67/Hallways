using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using ThunderWire.Attributes;

namespace UHFPS.Runtime
{
    [InspectorHeader("Saves UI Loader")]
    public class SavesUILoader : MonoBehaviour
    {
        public BackgroundFader BackgroundFader;
        public Button ContinueButton;

        [Header("Save Slot")]
        public Transform SaveSlotsParent;
        public GameObject SaveSlotPrefab;

        [Header("Settings")]
        public bool FadeOutAtStart;
        public bool LoadAtStart;
        public float FadeSpeed;

        [Header("Events")]
        public UnityEvent OnSavesBeingLoaded;
        public UnityEvent<SavedGameInfo?> OnSavesLoaded;

        private readonly Dictionary<GameObject, SavedGameInfo> saveSlots = new();
        private SavedGameInfo? lastSave;

        IEnumerator Start()
        {
            if (LoadAtStart)
            {
                yield return new WaitForEndOfFrame();
                yield return new WaitToTaskComplete(LoadAllSaves());

                // enable or disable continue button when last save exists
                if (ContinueButton != null)
                    ContinueButton.gameObject.SetActive(lastSave.HasValue);

                if (FadeOutAtStart) yield return BackgroundFader.StartBackgroundFade(true);
                OnSavesLoaded?.Invoke(lastSave);
            }

            yield return null;
        }

        public async void LoadSavedGames()
        {
            foreach (var slot in saveSlots)
            {
                Destroy(slot.Key);
            }

            saveSlots.Clear();
            OnSavesBeingLoaded?.Invoke();
            await LoadAllSaves();
            OnSavesLoaded?.Invoke(lastSave);
        }

        public void LoadLastSave()
        {
            if (!lastSave.HasValue) 
                return;

            SaveGameManager.SetLoadGameState(lastSave.Value.Scene, lastSave.Value.Foldername);
            StartCoroutine(FadeAndLoadGame());
        }

        private async Task LoadAllSaves()
        {
            var savedGames = await SaveGameManager.SaveGameReader.ReadAllSaves();
            lastSave = savedGames.Length > 0 ? savedGames[0] : null;

            for (int i = 0; i < savedGames.Length; i++)
            {
                SavedGameInfo saveInfo = savedGames[i];
                GameObject slotGO = Instantiate(SaveSlotPrefab, SaveSlotsParent);
                slotGO.name = "Slot" + i.ToString();

                LoadGameSlot loadGameSlot = slotGO.GetComponent<LoadGameSlot>();
                loadGameSlot.Initialize(i, saveInfo);

                Button loadButton = slotGO.GetComponent<Button>();
                loadButton.onClick.AddListener(() =>
                {
                    SaveGameManager.SetLoadGameState(saveInfo.Scene, saveInfo.Foldername);
                    StartCoroutine(FadeAndLoadGame());
                });

                saveSlots.Add(slotGO, saveInfo);
            }
        }

        IEnumerator FadeAndLoadGame()
        {
            if(BackgroundFader != null) yield return BackgroundFader.StartBackgroundFade(false, fadeSpeed: FadeSpeed);
            SceneManager.LoadScene(SaveGameManager.LMS);
        }
    }
}