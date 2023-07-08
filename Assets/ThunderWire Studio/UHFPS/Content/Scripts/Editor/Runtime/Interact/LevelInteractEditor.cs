using UnityEngine;
using UnityEditor;
using UHFPS.Runtime;
using ThunderWire.Editors;

namespace UHFPS.Editors
{
    [CustomEditor(typeof(LevelInteract))]
    public class LevelInteractEditor : Editor
    {
        SerializedProperty LevelLoadType;
        SerializedProperty NextLevelName;

        SerializedProperty CustomTransform;
        SerializedProperty TargetTransform;
        SerializedProperty LookUpDown;

        private void OnEnable()
        {
            LevelLoadType = serializedObject.FindProperty("LevelLoadType");
            NextLevelName = serializedObject.FindProperty("NextLevelName");

            CustomTransform = serializedObject.FindProperty("CustomTransform");
            TargetTransform = serializedObject.FindProperty("TargetTransform");
            LookUpDown = serializedObject.FindProperty("LookUpDown");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            LevelInteract.LevelType levelType = (LevelInteract.LevelType)LevelLoadType.enumValueIndex;

            EditorDrawing.DrawInspectorHeader(new GUIContent("Level Interact"), target);
            EditorGUILayout.Space();

            using(new EditorDrawing.BorderBoxScope(new GUIContent("Next Level"), 18f, true))
            {
                EditorGUILayout.PropertyField(LevelLoadType);
                EditorGUILayout.PropertyField(NextLevelName);
                EditorGUILayout.Space();

                if(levelType == LevelInteract.LevelType.NextLevel)
                    EditorGUILayout.HelpBox("The current world state will be saved and the player data will be saved and transferred to the next level.", MessageType.Info);
                else if (levelType == LevelInteract.LevelType.WorldState)
                    EditorGUILayout.HelpBox("The current world state will be saved, the world state of the next level will be loaded and the player data will be transferred. (Previous Scene Persistency must be enabled!)", MessageType.Info);
                else if (levelType == LevelInteract.LevelType.PlayerData)
                    EditorGUILayout.HelpBox("Only the player data will be saved and transferred to the next level.", MessageType.Info);

            }

            EditorGUILayout.Space();

            CustomTransform.boolValue = EditorDrawing.BeginToggleBorderLayout(new GUIContent("Custom Transform"), CustomTransform.boolValue);
            using (new EditorGUI.DisabledGroupScope(!CustomTransform.boolValue))
            {
                EditorGUILayout.PropertyField(TargetTransform);
                EditorGUILayout.PropertyField(LookUpDown);
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("The player position and rotation will be replaced by custom position and rotation specified by the target transform.", MessageType.Info);
            }
            EditorDrawing.EndBorderHeaderLayout();

            serializedObject.ApplyModifiedProperties();
        }
    }
}