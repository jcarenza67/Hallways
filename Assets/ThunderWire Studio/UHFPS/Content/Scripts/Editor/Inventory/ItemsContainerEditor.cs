using UnityEngine;
using UnityEditor;
using UHFPS.Runtime;
using ThunderWire.Editors;

namespace UHFPS.Editors
{
    [CustomEditor(typeof(ItemsContainer))]
    public class ItemsContainerEditor : MonoBehaviourEditor<ItemsContainer>
    {
        public override void OnInspectorGUI()
        {
            EditorDrawing.DrawInspectorHeader(new GUIContent("Items Container"), Target);
            EditorGUILayout.Space();

            serializedObject.Update();
            {
                using(new EditorDrawing.BorderBoxScope(new GUIContent("Animation")))
                {
                    Properties.Draw("Animator");
                    Properties.Draw("OpenParameter");
                }

                EditorGUILayout.Space();

                using (new EditorDrawing.BorderBoxScope(new GUIContent("Storage Settings")))
                {
                    Properties.Draw("ContainerTitle");
                    Properties.Draw("Rows");
                    Properties.Draw("Columns");
                }

                EditorGUILayout.Space();

                using (new EditorDrawing.BorderBoxScope(new GUIContent("Sounds")))
                {
                    Properties.Draw("OpenSound");
                    Properties.Draw("CloseSound");
                    Properties.Draw("CloseWithAnimation");
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}