using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UHFPS.Runtime;
using ThunderWire.Editors;

namespace UHFPS.Editors
{
    [CustomEditor(typeof(JumpscareTrigger))]
    public class JumpscareTriggerEditor : MonoBehaviourEditor<JumpscareTrigger>
    {
        public override void OnInspectorGUI()
        {
            EditorDrawing.DrawInspectorHeader(new GUIContent("Jumpscare Trigger"), Target);
            EditorGUILayout.Space();

            serializedObject.Update();
            {
                Properties.Draw("Animator");
                if (!Properties.DrawGetBool("PlayOnEnabled"))
                {
                    Properties.Draw("JumpscareState");
                }

                EditorGUILayout.Space(2f);
                using (new EditorDrawing.BorderBoxScope(new GUIContent("Settings")))
                {
                    Properties.Draw("WobbleAmplitudeGain");
                    Properties.Draw("WobbleFrequencyGain");
                    Properties.Draw("WobbleDuration");
                    Properties.Draw("SanityDuration");
                }

                EditorGUILayout.Space(2f);
                using (new EditorDrawing.BorderBoxScope(new GUIContent("Sounds")))
                {
                    Properties.Draw("JumpscareSound");
                }

                EditorGUILayout.Space(2f);
                if(EditorDrawing.BeginFoldoutBorderLayout(Properties["OnJumpscareStarted"], new GUIContent("Events")))
                {
                    Properties.Draw("OnJumpscareStarted");
                    Properties.Draw("OnJumpscareEnded");
                    EditorDrawing.EndBorderHeaderLayout();
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}