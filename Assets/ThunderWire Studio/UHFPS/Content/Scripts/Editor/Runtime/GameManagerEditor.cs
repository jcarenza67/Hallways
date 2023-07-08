using System.Linq;
using UnityEngine;
using UnityEditor;
using UHFPS.Runtime;
using ThunderWire.Editors;

namespace UHFPS.Editors
{
    [CustomEditor(typeof(GameManager))]
    public class GameManagerEditor : MonoBehaviourEditor<GameManager>
    {
        public override void OnInspectorGUI()
        {
            EditorDrawing.DrawInspectorHeader(new GUIContent("UHFPS GameManager"), Target);
            EditorGUILayout.Space();

            serializedObject.Update();
            {
                Properties.Draw("Modules");
                Properties.Draw("GlobalPPVolume");
                Properties.Draw("HealthPPVolume");
                Properties.Draw("BackgroundFade");

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
                EditorGUILayout.Space(2f);

                if(EditorDrawing.BeginFoldoutBorderLayout(Properties["GamePanel"], new GUIContent("Game Panels")))
                {
                    EditorGUILayout.LabelField("Main Panels", EditorStyles.boldLabel);
                    Properties.Draw("GamePanel");
                    Properties.Draw("PausePanel");
                    Properties.Draw("DeadPanel");

                    EditorGUILayout.Space(2f);
                    EditorGUILayout.LabelField("Sub Panels", EditorStyles.boldLabel);
                    Properties.Draw("HUDPanel");
                    Properties.Draw("TabPanel");

                    EditorGUILayout.Space(2f);
                    EditorGUILayout.LabelField("Feature Panels", EditorStyles.boldLabel);
                    Properties.Draw("InventoryPanel");
                    Properties.Draw("FloatingIcons");

                    EditorDrawing.EndBorderHeaderLayout();
                }

                EditorGUILayout.Space(1f);

                if (EditorDrawing.BeginFoldoutBorderLayout(Properties["ReticleImage"], new GUIContent("HUD References")))
                {
                    if (EditorDrawing.BeginFoldoutBorderLayout(Properties["ControlsInfoPanel"], new GUIContent("Reticle/Stamina")))
                    {
                        Properties.Draw("ReticleImage");
                        Properties.Draw("StaminaSlider");
                        EditorDrawing.EndBorderHeaderLayout();
                    }

                    EditorGUILayout.Space(1f);

                    if (EditorDrawing.BeginFoldoutBorderLayout(Properties["ControlsInfoText"], new GUIContent("Interact/Controls Info")))
                    {
                        Properties.Draw("ControlsInfoPanel");
                        Properties.Draw("ControlsInfoText");
                        Properties.Draw("InteractText");
                        Properties.Draw("InteractFormat");
                        EditorDrawing.EndBorderHeaderLayout();
                    }

                    EditorGUILayout.Space(1f);

                    if (EditorDrawing.BeginFoldoutBorderLayout(Properties["PointerImage"], new GUIContent("Interact Pointer")))
                    {
                        Properties.Draw("PointerImage");
                        Properties.Draw("NormalPointer");
                        Properties.Draw("HoverPointer");
                        EditorDrawing.EndBorderHeaderLayout();
                    }

                    EditorGUILayout.Space(1f);

                    if (EditorDrawing.BeginFoldoutBorderLayout(Properties["ItemPickupLayout"], new GUIContent("Pickup Message")))
                    {
                        Properties.Draw("ItemPickupLayout");
                        Properties.Draw("ItemPickup");
                        Properties.Draw("PickupMessageTime");
                        EditorDrawing.EndBorderHeaderLayout();
                    }

                    EditorGUILayout.Space(1f);

                    if (EditorDrawing.BeginFoldoutBorderLayout(Properties["HintMessageGroup"], new GUIContent("Hint Message")))
                    {
                        Properties.Draw("HintMessageGroup");
                        Properties.Draw("HintMessageFadeSpeed");
                        EditorDrawing.EndBorderHeaderLayout();
                    }

                    EditorGUILayout.Space(1f);

                    if (EditorDrawing.BeginFoldoutBorderLayout(Properties["HealthBar"], new GUIContent("Health Panel")))
                    {
                        Properties.Draw("HealthBar");
                        Properties.Draw("Hearthbeat");
                        Properties.Draw("HealthPercent");
                        EditorDrawing.EndBorderHeaderLayout();
                    }

                    EditorGUILayout.Space(1f);

                    if (EditorDrawing.BeginFoldoutBorderLayout(Properties["PaperPanel"], new GUIContent("Paper Panel")))
                    {
                        Properties.Draw("PaperPanel");
                        Properties.Draw("PaperText");
                        Properties.Draw("PaperFadeSpeed");
                        EditorDrawing.EndBorderHeaderLayout();
                    }

                    EditorGUILayout.Space(1f);

                    if (EditorDrawing.BeginFoldoutBorderLayout(Properties["ExamineInfoPanel"], new GUIContent("Examine Panel")))
                    {
                        Properties.Draw("ExamineInfoPanel");
                        Properties.Draw("ExamineHotspots");
                        Properties.Draw("ExamineText");
                        Properties.Draw("ExamineFadeSpeed");
                        EditorDrawing.EndBorderHeaderLayout();
                    }

                    EditorGUILayout.Space(1f);

                    if (EditorDrawing.BeginFoldoutBorderLayout(Properties["BloodOverlay"], new GUIContent("Overlays")))
                    {
                        Properties.Draw("BloodOverlay");
                        Properties.Draw("SanityOverlay");
                        EditorDrawing.EndBorderHeaderLayout();
                    }

                    EditorDrawing.EndBorderHeaderLayout();
                }

                EditorGUILayout.Space(1f);

                if (EditorDrawing.BeginFoldoutBorderLayout(Properties["EnableBlur"], new GUIContent("Blur Settings")))
                {
                    Properties.Draw("EnableBlur");
                    Properties.Draw("BlurRadius");
                    Properties.Draw("BlurDuration");

                    EditorDrawing.EndBorderHeaderLayout();
                }

                EditorGUILayout.Space(1f);

                EditorDrawing.DrawList(Properties["GraphicReferencesRaw"], new GUIContent("Custom UI References"));

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Manager Modules", EditorStyles.boldLabel);
                EditorGUILayout.Space(2f);

                if (Target.Modules != null)
                {
                    SerializedObject modulesAsset = new SerializedObject(Target.Modules);
                    SerializedProperty modules = modulesAsset.FindProperty("ManagerModules");

                    if (Target.Modules.ManagerModules.Any(x => x == null))
                    {
                        EditorGUILayout.HelpBox("There are elements that have an empty module reference!", MessageType.Warning);
                    }

                    for (int i = 0; i < modules.arraySize; i++)
                    {
                        SerializedProperty moduleProperty = modules.GetArrayElementAtIndex(i);
                        Object module = moduleProperty.objectReferenceValue;
                        if (module == null) continue;

                        Editor moduleEditor = CreateEditor(module);
                        Rect headerRect = EditorGUILayout.GetControlRect(false, 22f);

                        Texture2D icon = Resources.Load<Texture2D>("EditorIcons/module");
                        GUIContent header = new GUIContent($" {module} (Module)", icon);

                        using (new EditorDrawing.IconSizeScope(12))
                        {
                            if (moduleProperty.isExpanded = EditorDrawing.DrawFoldoutHeader(headerRect, header, moduleProperty.isExpanded))
                                moduleEditor.OnInspectorGUI();
                        }
                    }

                    EditorGUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
                    EditorGUILayout.HelpBox("To add new modules, open the Manager Modules asset.", MessageType.Info);
                }
                else
                {
                    EditorGUILayout.HelpBox("Assign the Manager Modules asset to view all modules.", MessageType.Info);
                }

                EditorGUILayout.Space();
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}