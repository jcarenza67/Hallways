using UnityEngine;
using UnityEditor;
using ThunderWire.Editors;

namespace UHFPS.Editors
{
    public class PuzzleEditor<T> : Editor where T : MonoBehaviour
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
            GUIContent puzzleBaseContent = EditorGUIUtility.TrTextContentWithIcon(" Puzzle Base Settings", "Settings");
            if(EditorDrawing.BeginFoldoutBorderLayout(puzzleBaseContent, ref settingsFoldout))
            {
                using (new EditorDrawing.BorderBoxScope(new GUIContent("Puzzle Camera")))
                {
                    Properties.Draw("PuzzleCamera");
                    Properties.Draw("SwitchCameraFadeSpeed");
                    Properties.Draw("ControlsFormat");
                }

                EditorGUILayout.Space(1f);

                using (new EditorDrawing.BorderBoxScope(new GUIContent("Puzzle Layers")))
                {
                    Properties.Draw("CullLayers");
                    Properties.Draw("InteractLayer");
                    Properties.Draw("DisabledLayer");
                    Properties.Draw("EnablePointer");
                }

                EditorGUILayout.Space(1f);

                using (new EditorDrawing.BorderBoxScope(new GUIContent("Ignore Colliders")))
                {
                    EditorGUI.indentLevel++;
                    {
                        Properties.Draw("CollidersEnable");
                        Properties.Draw("CollidersDisable");
                    }
                    EditorGUI.indentLevel--;
                }

                EditorDrawing.EndBorderHeaderLayout();
            }
        }
    }
}