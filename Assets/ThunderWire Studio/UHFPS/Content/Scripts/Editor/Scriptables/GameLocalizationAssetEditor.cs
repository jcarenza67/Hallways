using System.Linq;
using UnityEngine;
using UnityEditor;
using UHFPS.Scriptable;
using ThunderWire.Editors;

namespace UHFPS.Editors
{
    [CustomEditor(typeof(GameLocalizationAsset))]
    public class GameLocalizationAssetEditor : Editor
    {
        private const string LOCALIZATION_SYMBOL = "UHFPS_LOCALIZATION";
        private SerializedProperty localizations;

        private void OnEnable()
        {
            localizations = serializedObject.FindProperty("Localizations");
        }

        public override void OnInspectorGUI()
        {
            EditorDrawing.DrawInspectorHeader(new GUIContent("Game Localization Asset"));
            EditorGUILayout.Space();

            EditorGUILayout.HelpBox("To enable or disable UHFPS localization, click the button below. A scripting symbol will automatically be included in the player settings to allow you to use GLoc Localization.", MessageType.Info);
            EditorGUILayout.Space(1f);

            string toggleText = CheckActivation() ? "Disable" : "Enable";
            if (GUILayout.Button($"{toggleText} GLoc Localization", GUILayout.Height(25f)))
            {
                ToggleScriptingSymbol();
            }

            EditorGUILayout.Space();

            serializedObject.Update();
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorGUILayout.PropertyField(localizations);
                EditorGUILayout.EndVertical();
                EditorGUI.indentLevel--;
            }
            serializedObject.ApplyModifiedProperties();
        }

        private bool CheckActivation()
        {
            BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            return defines.Contains(LOCALIZATION_SYMBOL);
        }

        private void ToggleScriptingSymbol()
        {
            BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            string[] definesParts = defines.Split(';');

            if (defines.Contains(LOCALIZATION_SYMBOL))
                definesParts = definesParts.Except(new[] { LOCALIZATION_SYMBOL }).ToArray();
            else
                definesParts = definesParts.Concat(new[] { LOCALIZATION_SYMBOL }).ToArray();

            defines = string.Join(";", definesParts);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, defines);
        }
    }
}