using UnityEngine;
using UnityEditor;
using UHFPS.Runtime;
using ThunderWire.Editors;

namespace UHFPS.Editors
{
    [CustomEditor(typeof(CutsceneTrigger))]
    public class CutsceneTriggerEditor : MonoBehaviourEditor<CutsceneTrigger>
    {
        public override void OnInspectorGUI()
        {
            EditorDrawing.DrawInspectorHeader(new GUIContent("Cutscene Trigger"), Target);
            EditorGUILayout.Space();

            CutsceneTrigger.CutsceneTypeEnum cutsceneType = (CutsceneTrigger.CutsceneTypeEnum)Properties["CutsceneType"].enumValueIndex;

            serializedObject.Update();
            {
                Properties.Draw("CutsceneType");
                Properties.Draw("Cutscene");

                if(cutsceneType == CutsceneTrigger.CutsceneTypeEnum.CameraCutscene)
                {
                    EditorGUILayout.Space();
                    Properties.Draw("CutsceneCamera");
                    Properties.Draw("CutsceneFadeSpeed");
                }
                else
                {
                    EditorGUILayout.Space();
                    Properties.Draw("InitialPosition");
                    Properties.Draw("InitialLook");
                }

                EditorGUILayout.Space();
                if (EditorDrawing.BeginFoldoutBorderLayout(Properties["OnCutsceneStart"], new GUIContent("Events")))
                {
                    Properties.Draw("OnCutsceneStart");
                    Properties.Draw("OnCutsceneEnd");
                    EditorDrawing.EndBorderHeaderLayout();
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}