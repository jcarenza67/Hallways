using UnityEngine;
using UnityEditor;
using UHFPS.Runtime;
using ThunderWire.Editors;

namespace UHFPS.Editors
{
    [CustomEditor(typeof(CameraItem))]
    public class CameraItemEditor : PlayerItemEditor<CameraItem>
    {
        public override void OnInspectorGUI()
        {
            EditorDrawing.DrawInspectorHeader(new GUIContent("Camera Item"), Target);
            EditorGUILayout.Space();

            serializedObject.Update();
            {
                base.OnInspectorGUI();
                EditorGUILayout.Space();

                Properties.Draw("<ItemObject>k__BackingField");
                Properties.Draw("BatteryInventoryItem");
                Properties.Draw("CameraLight");
                Properties.Draw("CameraVolume");
                Properties.Draw("CameraAudio");

                EditorGUILayout.Space();
                using (new EditorDrawing.BorderBoxScope(new GUIContent("Battery Settings")))
                {
                    EditorDrawing.DrawPropertyAndLabel(Properties["BatteryLife"], new GUIContent("sec"));
                    
                    Properties.Draw("BatteryPercentage");
                    Properties.Draw("BatteryLowPercent");
                    Properties.Draw("LightIntensity");
                    Properties.Draw("BatteryFullColor");
                    Properties.Draw("BatteryLowColor");

                    EditorGUILayout.Space();
                    Rect batteryPercentageRect = EditorGUILayout.GetControlRect();
                    float batteryEnergy = Application.isPlaying ? Target.batteryEnergy : Target.BatteryPercentage.Ratio();
                    int batteryPercent = Mathf.RoundToInt(batteryEnergy * 100);
                    EditorGUI.ProgressBar(batteryPercentageRect, batteryEnergy, $"Battery Energy ({batteryPercent}%)");
                }

                EditorGUILayout.Space();
                using (new EditorDrawing.BorderBoxScope(new GUIContent("Zoom Settings")))
                {
                    Properties.Draw("LightZoomRange");
                    Properties.Draw("CameraZoomFOV");
                    Properties.Draw("CameraZoomSpeed");
                }

                EditorGUILayout.Space();
                using (new EditorDrawing.BorderBoxScope(new GUIContent("Animation Settings")))
                {
                    Properties.Draw("CameraShow");
                    Properties.Draw("CameraHide");
                    Properties.Draw("CameraReload");
                    Properties.Draw("CameraShowFade");
                    Properties.Draw("CameraHideFade");
                }

                EditorGUILayout.Space();
                using (new EditorDrawing.BorderBoxScope(new GUIContent("Sound Settings")))
                {
                    Properties.Draw("CameraEquip");
                    Properties.Draw("CameraUnequip");
                    Properties.Draw("CameraZoomIn");
                    Properties.Draw("CameraZoomOut");
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}