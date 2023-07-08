using UHFPS.Runtime;
using UnityEngine;
using UnityEditor;
using ThunderWire.Editors;
using UHFPS.Scriptable;

namespace UHFPS.Editors
{
    [CustomEditor(typeof(GLocText)), CanEditMultipleObjects]
    public class GLocTextEditor : MonoBehaviourEditor<GLocText>
    {
        GameLocalizationAsset localizationAsset;

        public override void OnEnable()
        {
            base.OnEnable();

            if (GameLocalization.HasReference)
                localizationAsset = GameLocalization.Instance.LocalizationAsset;
        }

        public override void OnInspectorGUI()
        {
            EditorDrawing.DrawInspectorHeader(new GUIContent("Gloc Text (Localization)"), Target);
            EditorGUILayout.Space();

            serializedObject.Update();
            {
                Properties.Draw("GlocKey");
                Properties.Draw("ObserveMany");

                EditorGUILayout.Space();
                Properties.Draw("OnUpdateText");

                EditorGUILayout.Space();
                using (new EditorGUI.DisabledGroupScope(localizationAsset == null))
                {
                    if (GUILayout.Button("Ping Localization Asset", GUILayout.Height(25)))
                    {
                        EditorGUIUtility.PingObject(localizationAsset);
                    }
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}