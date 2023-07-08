using UnityEngine;
using UnityEditor;
using UHFPS.Scriptable;
using ThunderWire.Editors;

namespace UHFPS.Editors
{
    [CustomEditor(typeof(SurfaceDetailsAsset))]
    public class SurfaceDetailsEditor : Editor
    {
        SerializedProperty Surfaces;

        private void OnEnable()
        {
            Surfaces = serializedObject.FindProperty("Surfaces");
        }

        public override void OnInspectorGUI()
        {
            EditorDrawing.DrawInspectorHeader(new GUIContent("Surface Details"));
            EditorGUILayout.Space();

            serializedObject.Update();
            { 
                if(EditorDrawing.BeginFoldoutBorderLayout(Surfaces, new GUIContent("Surface Details")))
                {
                    for (int i = 0; i < Surfaces.arraySize; i++)
                    {
                        SerializedProperty listItem = Surfaces.GetArrayElementAtIndex(i);
                        var properties = EditorDrawing.GetAllProperties(listItem);

                        string surfaceName = properties["SurfaceName"].stringValue;
                        string title = !string.IsNullOrEmpty(surfaceName) ? surfaceName : "Element " + i;

                        if (EditorDrawing.BeginFoldoutBorderLayout(listItem, new GUIContent(title), out Rect itemHeaderRect))
                        {
                            properties.Draw("SurfaceName");

                            EditorGUILayout.Space(2f);
                            if (EditorDrawing.BeginFoldoutBorderLayout(properties["SurfaceProperties"], new GUIContent("Surface Properties")))
                            {
                                properties.DrawRelative("SurfaceProperties.SurfaceTag");
                                EditorGUI.indentLevel++;
                                properties.DrawRelative("SurfaceProperties.SurfaceTextures");
                                EditorGUI.indentLevel--;
                                EditorDrawing.EndBorderHeaderLayout();
                            }

                            EditorGUILayout.Space(1f);
                            if (EditorDrawing.BeginFoldoutBorderLayout(properties["FootstepProperties"], new GUIContent("Footstep Properties")))
                            {
                                EditorGUI.indentLevel++;
                                properties.DrawRelative("FootstepProperties.SurfaceFootsteps");
                                properties.DrawRelative("FootstepProperties.SurfaceLandSteps");
                                EditorGUI.indentLevel--;
                                EditorDrawing.EndBorderHeaderLayout();
                            }

                            EditorGUILayout.Space(1f);
                            if (EditorDrawing.BeginFoldoutBorderLayout(properties["ImpactProperties"], new GUIContent("Impact Properties")))
                            {
                                EditorGUILayout.LabelField("Bulletmark", EditorStyles.boldLabel);
                                EditorGUI.indentLevel++;
                                properties.DrawRelative("ImpactProperties.SurfaceBulletmarks");
                                properties.DrawRelative("ImpactProperties.BulletmarkImpacts");
                                EditorGUI.indentLevel--;

                                EditorGUILayout.Space(2f);
                                EditorGUILayout.LabelField("Meleemark", EditorStyles.boldLabel);
                                EditorGUI.indentLevel++;
                                properties.DrawRelative("ImpactProperties.SlashImpactType");
                                properties.DrawRelative("ImpactProperties.StabImpactType");
                                EditorGUI.indentLevel--;
                                EditorDrawing.EndBorderHeaderLayout();
                            }

                            EditorGUILayout.Space(1f);
                            if (EditorDrawing.BeginFoldoutBorderLayout(properties["MultiplierProperties"], new GUIContent("Volume Multipliers")))
                            {
                                properties.DrawRelative("MultiplierProperties.FootstepsMultiplier");
                                properties.DrawRelative("MultiplierProperties.LandStepsMultiplier");
                                EditorDrawing.EndBorderHeaderLayout();
                            }

                            EditorDrawing.EndBorderHeaderLayout();
                        }

                        GUIContent minus = EditorGUIUtility.TrIconContent("Toolbar Minus");
                        Rect minusRect = itemHeaderRect;
                        minusRect.xMin = minusRect.xMax - EditorGUIUtility.singleLineHeight;
                        minusRect.x -= 3f;
                        minusRect.y += 3f;

                        if (GUI.Button(minusRect, minus, EditorStyles.iconButton))
                        {
                            Surfaces.DeleteArrayElementAtIndex(i);
                        }

                        if (i < Surfaces.arraySize) EditorGUILayout.Space(1f);
                    }

                    if (Surfaces.arraySize > 0)
                        EditorGUILayout.Space(2f);

                    if (GUILayout.Button(new GUIContent("Add Surface")))
                    {
                        Surfaces.arraySize++;
                    }
                    EditorDrawing.EndBorderHeaderLayout();
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}