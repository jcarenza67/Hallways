using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UHFPS.Runtime;
using ThunderWire.Editors;

namespace UHFPS.Editors
{
    [CustomEditor(typeof(SaveGameManager))]
    public class SaveGameManagerEditor : Editor
    {
        private SaveGameManager manager;
        private string saveFolderName;
        private bool debugExpanded;

        private void OnEnable()
        {
            manager = (SaveGameManager)target;
            saveFolderName = "Save";
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorDrawing.DrawInspectorHeader(new GUIContent("Save Game Manager"), target);
            EditorGUILayout.Space();
            DrawPropertiesExcluding(serializedObject, "m_Script", "constantSaveables", "runtimeSaveables");

            EditorGUILayout.Space();
            using (new EditorDrawing.BorderBoxScope(new GUIContent("Saveables Searcher")))
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorUtils.TrIconText($"Constant Saveables: {manager.constantSaveables.Count}", MessageType.Info, EditorStyles.miniBoldLabel);
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorUtils.TrIconText($"Runtime Saveables: {manager.runtimeSaveables.Count}", MessageType.Info, EditorStyles.miniBoldLabel);
                EditorGUILayout.EndVertical();

                if (GUILayout.Button("Find Saveables", GUILayout.Height(25)))
                {
                    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                    stopwatch.Start();

                    MonoBehaviour[] monos = FindObjectsOfType<MonoBehaviour>(true);
                    var saveables = from mono in monos
                                    let type = mono.GetType()
                                    where typeof(ISaveable).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract
                                    let token = $"{type.Name}{SaveGameManager.TOKEN_SEPARATOR}{GUID.Generate()}"
                                    select new SaveGameManager.SaveablePair(token, mono);

                    manager.constantSaveables = saveables.ToList();
                    stopwatch.Stop();

                    EditorUtility.SetDirty(target);
                    Debug.Log($"<color=yellow>[Saveables Searcher]</color> Found {saveables.Count()} saveables in {stopwatch.ElapsedMilliseconds}ms. <color=red>SAVE YOUR SCENE!</color>");
                }
            }

            EditorGUILayout.Space();

            if(EditorDrawing.BeginFoldoutBorderLayout(new GUIContent("Functions Debug (Runtime Only)"), ref debugExpanded))
            {
                using (new EditorGUI.DisabledGroupScope(!Application.isPlaying))
                {
                    Rect saveGameRect = EditorGUILayout.GetControlRect();
                    Rect saveGameBtn = EditorGUI.PrefixLabel(saveGameRect, new GUIContent("Save Game"));
                    if (GUI.Button(saveGameBtn, new GUIContent("Save")))
                    {
                        SaveGameManager.SaveGame();
                    }

                    Rect loadGameRect = EditorGUILayout.GetControlRect();
                    Rect loadGameBtn = EditorGUI.PrefixLabel(loadGameRect, new GUIContent("Load Game"));

                    Rect loadGameText = loadGameBtn;
                    loadGameText.xMax *= 0.8f;
                    loadGameBtn.xMin = loadGameText.xMax + 2f;

                    saveFolderName = EditorGUI.TextField(loadGameText, saveFolderName);
                    if (GUI.Button(loadGameBtn, new GUIContent("Load")))
                    {
                        if (!string.IsNullOrEmpty(saveFolderName))
                        {
                            SaveGameManager.GameLoadType = SaveGameManager.LoadType.LoadGameState;
                            SaveGameManager.LoadFolderName = saveFolderName;

                            string sceneName = SceneManager.GetActiveScene().name;
                            SaveGameManager.LoadSceneName = sceneName;
                            SceneManager.LoadScene(SaveGameManager.LMS);
                        }
                    }
                }

                EditorDrawing.EndBorderHeaderLayout();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}