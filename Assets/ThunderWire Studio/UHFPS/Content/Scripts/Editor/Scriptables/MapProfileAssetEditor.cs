using UnityEngine;
using UnityEditor;
using UHFPS.Scriptable;
using ThunderWire.Editors;

namespace UHFPS.Editors
{
    [CustomEditor(typeof(MapProfileAsset))]
    public class MapProfileAssetEditor : Editor
    {
        private SerializedProperty m_MapTexture;
        private SerializedProperty m_MapTextureSize;
        private SerializedProperty m_MapBackground;
        private SerializedProperty m_MapBounds;
        private SerializedProperty m_MapLayers;

        private void OnEnable()
        {
            m_MapTexture = serializedObject.FindProperty("MapTexture");
            m_MapTextureSize = serializedObject.FindProperty("MapTextureSize");
            m_MapBackground = serializedObject.FindProperty("MapBackground");
            m_MapBounds = serializedObject.FindProperty("MapBounds");
            m_MapLayers = serializedObject.FindProperty("MapLayers");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            using(new EditorUtils.BoxGroupScope("Settings", "Map Profile Settings"))
            {
                EditorGUILayout.PropertyField(m_MapTexture);
                EditorGUILayout.PropertyField(m_MapTextureSize);
                EditorGUILayout.PropertyField(m_MapBackground);
                EditorGUILayout.PropertyField(m_MapBounds);
            }

            EditorGUILayout.Space();

            if(m_MapLayers.arraySize > 0)
            {
                using (new EditorUtils.BoxGroupScope("RectMask2D Icon", "World Map Layers"))
                {
                    for (int i = 0; i < m_MapLayers.arraySize; i++)
                    {
                        SerializedProperty layer = m_MapLayers.GetArrayElementAtIndex(i);
                        SerializedProperty roomList = layer.FindPropertyRelative("RoomList");

                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        if (layer.isExpanded = EditorUtils.DrawBoxFoldoutHeader("Floor " + i, layer.isExpanded))
                        {
                            if(roomList.arraySize > 0)
                            {
                                for (int j = 0; j < roomList.arraySize; j++)
                                {
                                    SerializedProperty room = roomList.GetArrayElementAtIndex(j);
                                    SerializedProperty r_RoomPlanID = room.FindPropertyRelative("RoomPlanID");

                                    EditorGUILayout.BeginVertical(GUI.skin.box);
                                    if (room.isExpanded = EditorUtils.DrawFoldoutHeader(20, "Room " + r_RoomPlanID.intValue, room.isExpanded))
                                    {
                                        // room properties
                                        SerializedProperty r_RoomSprite = room.FindPropertyRelative("RoomSprite");
                                        SerializedProperty r_RoomBounds = room.FindPropertyRelative("RoomBounds");
                                        SerializedProperty r_BuildingPlanID = room.FindPropertyRelative("BuildingPlanID");
                                        SerializedProperty r_RoomTitle = room.FindPropertyRelative("RoomTitle");
                                        SerializedProperty r_DoorsList = room.FindPropertyRelative("DoorsList");

                                        EditorGUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
                                        Rect iconRect = GUILayoutUtility.GetRect(1, 64);
                                        iconRect.width = 64 + EditorGUIUtility.singleLineHeight;

                                        Rect boundsRect = GUILayoutUtility.GetRect(1, 40);
                                        boundsRect.xMin += iconRect.width + EditorGUIUtility.standardVerticalSpacing * 2;
                                        boundsRect.y -= iconRect.height;
                                        boundsRect.y += (64 / 2) - 20;

                                        // properties drawing
                                        EditorGUI.indentLevel++;
                                        {
                                            r_RoomSprite.objectReferenceValue = EditorGUI.ObjectField(iconRect, r_RoomSprite.objectReferenceValue, typeof(Sprite), false);
                                            EditorGUI.PropertyField(boundsRect, r_RoomBounds, GUIContent.none);

                                            EditorGUILayout.Space(-35 + EditorGUIUtility.standardVerticalSpacing);
                                            EditorGUILayout.LabelField("Room Settings", EditorStyles.boldLabel);
                                            EditorGUILayout.PropertyField(r_BuildingPlanID);
                                            EditorGUI.indentLevel++;
                                            EditorGUILayout.PropertyField(r_RoomTitle);
                                            EditorGUILayout.PropertyField(r_DoorsList);
                                            EditorGUI.indentLevel--;
                                        }
                                        EditorGUI.indentLevel--;
                                    }
                                    EditorGUILayout.EndVertical();
                                }
                            }
                            else
                            {
                                EditorGUILayout.HelpBox("The Map Layer has no rooms! Open MapProfileCreator and create the Map Profile Asset again.", MessageType.Warning);
                            }
                        }
                        EditorGUILayout.EndVertical();
                        if(i < m_MapLayers.arraySize - 1) EditorGUILayout.Space(1f);
                    }
                }
            }
            else
            {
                EditorGUILayout.HelpBox("No Map Layers are added to the Map Profile Asset! Open MapProfileCreator and create the Map Profile Asset again.", MessageType.Warning);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}