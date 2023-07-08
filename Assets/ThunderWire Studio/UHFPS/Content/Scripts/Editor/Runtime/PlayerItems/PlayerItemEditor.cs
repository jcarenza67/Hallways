using UnityEngine;
using UnityEditor;
using ThunderWire.Editors;

namespace UHFPS.Editors
{
    public class PlayerItemEditor<T> : Editor where T : MonoBehaviour
    {
        public T Target { get; private set; }
        public PropertyCollection Properties { get; private set; }

        private bool settingsFoldout;

        public virtual void OnEnable()
        {
            Target = target as T;
            Properties = EditorDrawing.GetAllProperties(serializedObject);
        }

        public override void OnInspectorGUI()
        {
            GUIContent playerItemSettingsContent = EditorGUIUtility.TrTextContentWithIcon(" PlayerItem Base Settings", "Settings");
            if (EditorDrawing.BeginFoldoutBorderLayout(playerItemSettingsContent, ref settingsFoldout))
            {
                if(EditorDrawing.BeginFoldoutToggleBorderLayout(new GUIContent("Wall Detection"), Properties["EnableWallDetection"]))
                {
                    using (new EditorGUI.DisabledGroupScope(!Properties.BoolValue("EnableWallDetection")))
                    {
                        Properties.Draw("ShowRayGizmos");
                        Properties.Draw("WallHitMask");
                        Properties.Draw("WallHitRayDistance");
                        Properties.Draw("WallHitRayRadius");
                        Properties.Draw("WallHitAmount");
                        Properties.Draw("WallHitTime");
                        Properties.Draw("WallHitRayOffset");
                    }
                    EditorDrawing.EndBorderHeaderLayout();
                }

                if (EditorDrawing.BeginFoldoutToggleBorderLayout(new GUIContent("Look Sway"), Properties["EnableSway"]))
                {
                    using (new EditorGUI.DisabledGroupScope(!Properties.BoolValue("EnableSway")))
                    {
                        Properties.Draw("<SwayPivot>k__BackingField");
                        Properties.Draw("LookSwayAmount");
                        Properties.Draw("ADSLookSwayAmount");
                        Properties.Draw("WalkSidewaySway");
                        Properties.Draw("WalkForwardSway");
                        Properties.Draw("MaxLookSwayAmount");
                        Properties.Draw("SwaySpeed");
                    }
                    EditorDrawing.EndBorderHeaderLayout();
                }

                EditorDrawing.EndBorderHeaderLayout();
            }
        }
    }
}