using UnityEngine;
using UnityEditor;
using UHFPS.Scriptable;
using ThunderWire.Editors;

namespace UHFPS.Editors
{
    [CustomEditor(typeof(ObjectivesAsset))]
    public class ObjectivesAssetEditor : Editor
    {
        private SerializedProperty Objectives;

        private void OnEnable()
        {
            Objectives = serializedObject.FindProperty("Objectives");
        }

        public override void OnInspectorGUI()
        {
            EditorDrawing.DrawInspectorHeader(new GUIContent("Objectives Asset"));
            EditorGUILayout.Space();

            serializedObject.Update();
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorGUILayout.PropertyField(Objectives);
                EditorGUILayout.EndVertical();
                EditorGUI.indentLevel--;
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}