using System;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UHFPS.Runtime;
using ThunderWire.Editors;

namespace UHFPS.Editors
{
    [CustomEditor(typeof(PlayerStateMachine))]
    public class PlayerStateMachineEditor : MonoBehaviourEditor<PlayerStateMachine>
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            {
                EditorDrawing.DrawInspectorHeader(new GUIContent("Player State Machine"), Target);
                EditorGUILayout.Space();

                Properties.Draw("StatesAsset");
                Properties.Draw("SurfaceMask");
                Properties.Draw("ControllerOffset");

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Player Settings", EditorStyles.boldLabel);
                EditorDrawing.DrawClassBorderFoldout(Properties["PlayerBasicSettings"], new GUIContent("Basic Settings"));
                EditorDrawing.DrawClassBorderFoldout(Properties["PlayerFeatures"], new GUIContent("Player Features"));
                EditorDrawing.DrawClassBorderFoldout(Properties["PlayerStamina"], new GUIContent("Player Stamina"));
                EditorDrawing.DrawClassBorderFoldout(Properties["PlayerControllerSettings"], new GUIContent("Controller Settings"));

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Controller States", EditorStyles.boldLabel);
                EditorDrawing.DrawClassBorderFoldout(Properties["StandingState"], new GUIContent("Standing State"));
                EditorDrawing.DrawClassBorderFoldout(Properties["CrouchingState"], new GUIContent("Crouching State"));

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Player States", EditorStyles.boldLabel);
                EditorGUILayout.Space(EditorGUIUtility.standardVerticalSpacing);

                if (Target.StatesAsset != null)
                {
                    Vector2 iconSize = EditorGUIUtility.GetIconSize();
                    SerializedObject statesSerializedObject = Application.isPlaying && Target.StatesAssetRuntime != null
                        ? new SerializedObject(Target.StatesAssetRuntime)
                        : new SerializedObject(Target.StatesAsset);

                    PropertyCollection stateProperties = EditorDrawing.GetAllProperties(statesSerializedObject);
                    SerializedProperty statesProperty = stateProperties["PlayerStates"];

                    statesSerializedObject.Update();
                    {
                        if (stateProperties.Count > 2)
                        {
                            if(EditorDrawing.BeginFoldoutBorderLayout(stateProperties["PlayerStates"], new GUIContent("Global Variables" + (Application.isPlaying ? "*" : ""))))
                            {
                                foreach (var item in stateProperties.Skip(2))
                                {
                                    EditorGUILayout.PropertyField(item.Value);
                                }
                                EditorDrawing.EndBorderHeaderLayout();
                            }
                            EditorGUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
                        }

                        if (Target.StatesAsset.PlayerStates.Count > 0)
                        {
                            Type currentState = null;
                            Type previousState = null;
                            if (Application.isPlaying)
                            {
                                if (Target.CurrentState.HasValue)
                                    currentState = Target.CurrentState?.stateData.stateAsset.GetType();

                                if (Target.PreviousState.HasValue)
                                    previousState = Target.PreviousState?.stateData.stateAsset.GetType();
                            }

                            for (int i = 0; i < statesProperty.arraySize; i++)
                            {
                                SerializedProperty state = statesProperty.GetArrayElementAtIndex(i);
                                SerializedProperty stateAsset = state.FindPropertyRelative("stateAsset");
                                SerializedProperty isEnabled = state.FindPropertyRelative("isEnabled");

                                bool expanded = state.isExpanded;
                                bool toggle = isEnabled.boolValue;

                                string name = stateAsset.objectReferenceValue.ToString();
                                EditorGUIUtility.SetIconSize(new Vector2(14, 14));
                                GUIContent title = EditorGUIUtility.TrTextContentWithIcon(" " + name, "NavMeshAgent Icon");
                                Rect header = EditorDrawing.DrawScriptableBorderFoldoutToggle(stateAsset, title, ref expanded, ref toggle);

                                state.isExpanded = expanded;
                                isEnabled.boolValue = toggle;

                                if (Application.isPlaying)
                                {
                                    if (currentState != null && stateAsset.objectReferenceValue.GetType() == currentState)
                                    {
                                        Rect currStateRect = header;
                                        currStateRect.xMin = header.xMax - EditorGUIUtility.singleLineHeight;

                                        GUIContent currStateIndicator = EditorGUIUtility.TrIconContent("greenLight", "Current State");
                                        EditorGUI.LabelField(currStateRect, currStateIndicator);
                                    }

                                    if (previousState != null && stateAsset.objectReferenceValue.GetType() == previousState)
                                    {
                                        Rect prevStateRect = header;
                                        prevStateRect.xMin = header.xMax - EditorGUIUtility.singleLineHeight;

                                        GUIContent prevStateIndicator = EditorGUIUtility.TrIconContent("orangeLight", "Previous State");
                                        EditorGUI.LabelField(prevStateRect, prevStateIndicator);
                                    }
                                }
                            }
                        }
                    }
                    statesSerializedObject.ApplyModifiedProperties();

                    EditorGUIUtility.SetIconSize(iconSize);
                    EditorGUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
                    EditorGUILayout.HelpBox("To add new states open player state asset.", MessageType.Info);
                }
                else
                {
                    EditorGUILayout.HelpBox("Assign a player state asset to display all states.", MessageType.Info);
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}