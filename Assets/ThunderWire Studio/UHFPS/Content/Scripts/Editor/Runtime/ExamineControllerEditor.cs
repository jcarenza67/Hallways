using UnityEngine;
using UnityEditor;
using UHFPS.Runtime;
using ThunderWire.Editors;
using UHFPS.Editors;

namespace UHFPS.Runtime
{
    [CustomEditor(typeof(ExamineController))]
    public class ExamineControllerEditor : MonoBehaviourEditor<ExamineController>
    {
        public override void OnInspectorGUI()
        {
            EditorDrawing.DrawInspectorHeader(new GUIContent("Examine Controller"), Target);
            EditorGUILayout.Space();

            serializedObject.Update();
            {
                Properties.Draw("FocusCullLayes");
                Properties.Draw("FocusLayer");
                Properties.Draw("HotspotPrefab");

                EditorGUILayout.Space();
                using (new EditorDrawing.BorderBoxScope(new GUIContent("Controls Settings")))
                {
                    Properties.Draw("ControlsFormat");
                    Properties.Draw("SpaceSeparator");
                    Properties.Draw("ControlsPadding");
                }

                EditorGUILayout.Space(1f);
                if (EditorDrawing.BeginFoldoutBorderLayout(Properties["RotateTime"], new GUIContent("Settings")))
                {
                    Properties.Draw("RotateTime");
                    Properties.Draw("RotateMultiplier");
                    Properties.Draw("ZoomMultiplier");
                    Properties.Draw("TimeToExamine");
                    EditorDrawing.EndBorderHeaderLayout();
                }

                EditorGUILayout.Space(1f);
                if (EditorDrawing.BeginFoldoutBorderLayout(Properties["DropOffset"], new GUIContent("Offsets")))
                {
                    Properties.Draw("DropOffset");
                    Properties.Draw("InventoryOffset");
                    Properties.Draw("ShowLabels");
                    EditorDrawing.EndBorderHeaderLayout();
                }

                EditorGUILayout.Space(1f);
                if (EditorDrawing.BeginFoldoutBorderLayout(Properties["PickUpCurve"], new GUIContent("Pickup Curve")))
                {
                    Properties.Draw("PickUpCurve");
                    Properties.Draw("PickUpCurveMultiplier");
                    Properties.Draw("PickUpTime");
                    EditorDrawing.EndBorderHeaderLayout();
                }

                EditorGUILayout.Space(1f);
                if (EditorDrawing.BeginFoldoutBorderLayout(Properties["PutPositionCurve"], new GUIContent("Put Position Curve")))
                {
                    Properties.Draw("PutPositionCurve");
                    Properties.Draw("PutPositionCurveMultiplier");
                    Properties.Draw("PutPositionCurveTime");
                    EditorDrawing.EndBorderHeaderLayout();
                }

                EditorGUILayout.Space(1f);
                if (EditorDrawing.BeginFoldoutBorderLayout(Properties["PutRotationCurve"], new GUIContent("Put Rotation Curve")))
                {
                    Properties.Draw("PutRotationCurve");
                    Properties.Draw("PutRotationCurveMultiplier");
                    Properties.Draw("PutRotationCurveTime");
                    EditorDrawing.EndBorderHeaderLayout();
                }

                EditorGUILayout.Space(1f);
                if (EditorDrawing.BeginFoldoutBorderLayout(Properties["ExamineHintSound"], new GUIContent("Sounds")))
                {
                    Properties.Draw("ExamineHintSound");
                    EditorDrawing.EndBorderHeaderLayout();
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}