using UnityEngine;
using UnityEditor;
using ThunderWire.Editors;
using UHFPS.Runtime;

namespace UHFPS.Editors
{
    [CustomEditor(typeof(BreakableCrate))]
    public class BreakableCrateEditor : MonoBehaviourEditor<BreakableCrate>
    {
        public override void OnInspectorGUI()
        {
            EditorDrawing.DrawInspectorHeader(new GUIContent("Breakable Crate"), Target);
            EditorGUILayout.Space();

            serializedObject.Update();
            {
                if (Application.isPlaying)
                {
                    float health = Target.EntityHealth / 100f;
                    Rect healthProgressBarRect = EditorGUILayout.GetControlRect();
                    EditorGUI.ProgressBar(healthProgressBarRect, health, $"Crate Health ({Target.EntityHealth}%)");
                    EditorGUILayout.Space();
                }

                if (Properties.BoolValue("SpawnRandomItem"))
                {
                    EditorDrawing.DrawList(Properties["CrateItems"], new GUIContent("Crate Items"));
                    EditorGUILayout.Space(2f);
                }
                else
                {
                    Properties.Draw("ItemInside");
                }

                Properties.Draw("BrokenCratePrefab");
                Properties.Draw("CrateCenter");

                EditorGUILayout.Space();
                using(new EditorDrawing.BorderBoxScope(new GUIContent("Breakable Settings")))
                {
                    Properties.Draw("SpawnRandomItem");
                    Properties.Draw("ShowFLoatingIcon");
                    Properties.Draw("PiecesKeepTime");
                    Properties.Draw("BrokenRotation");
                    Properties.Draw("SpawnedRotation");
                }

                EditorGUILayout.Space(2f);
                using (new EditorDrawing.BorderBoxScope(new GUIContent("Explosion Settings")))
                {
                    Properties.Draw("ExplosionEffect");
                    Properties.Draw("UpwardsModifer");
                    Properties.Draw("ExplosionPower");
                    Properties.Draw("ExplosionRadius");
                }

                EditorGUILayout.Space(2f);
                using (new EditorDrawing.BorderBoxScope(new GUIContent("Sounds")))
                {
                    Properties.Draw("BreakSound");
                }

                EditorGUILayout.Space(2f);
                using (new EditorDrawing.BorderBoxScope(new GUIContent("Events")))
                {
                    Properties.Draw("OnCrateBreak");
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}