using System.Linq;
using UnityEditor;
using UnityEngine;
using UHFPS.Scriptable;

namespace UHFPS.Editors
{
    public class InventoryItemsExport : EditorWindow
    {
        private InventoryAsset asset;
        private GameLocalizationAsset localizationAsset;
        private string keysSection = "item";

        public void Show(InventoryAsset asset)
        {
            this.asset = asset;
        }

        private void OnGUI()
        {
            Rect rect = position;
            rect.xMin += 5f;
            rect.xMax -= 5f;
            rect.yMin += 5f;
            rect.yMax -= 5f;
            rect.x = 5;
            rect.y = 5;

            GUILayout.BeginArea(rect);
            {
                EditorGUILayout.HelpBox("This tool automatically generates keys for the item title and description. These keys will be exported to the GameLocalization asset and assigned to the items. The title and description will be populated with the item's Title and Description text.", MessageType.Info);
                EditorGUILayout.HelpBox((asset.Items.Count * 2) + " key will be exported.", MessageType.Info);

                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    localizationAsset = (GameLocalizationAsset)EditorGUILayout.ObjectField(new GUIContent("GameLocalization Asset"), localizationAsset, typeof(GameLocalizationAsset), false);
                    keysSection = EditorGUILayout.TextField(new GUIContent("Keys Section"), keysSection);

                    EditorGUILayout.Space();
                    using (new EditorGUI.DisabledGroupScope(localizationAsset == null))
                    {
                        if (GUILayout.Button(new GUIContent("Export Keys"), GUILayout.Height(25f)))
                        {
                            SerializedObject serializedObject = new SerializedObject(asset);
                            SerializedProperty itemsLits = serializedObject.FindProperty("Items");

                            for (int i = 0; i < asset.Items.Count; i++)
                            {
                                var rawItem = asset.Items[i];
                                SerializedProperty itemElement = itemsLits.GetArrayElementAtIndex(i);
                                SerializedProperty item = itemElement.FindPropertyRelative("item");
                                SerializedProperty title = item.FindPropertyRelative("Title");

                                SerializedProperty localization = item.FindPropertyRelative("LocalizationSettings");
                                SerializedProperty titleKeyProp = localization.FindPropertyRelative("titleKey");
                                SerializedProperty descKeyProp = localization.FindPropertyRelative("descriptionKey");

                                string itemTitle = title.stringValue.Replace(" ", "").ToLower();
                                string titleKey = keysSection + ".title." + itemTitle;
                                string descriptionKey = keysSection + ".description." + itemTitle;

                                titleKeyProp.stringValue = titleKey;
                                descKeyProp.stringValue = descriptionKey;

                                localizationAsset.AddSectionKey(titleKey, rawItem.item.Title);
                                localizationAsset.AddSectionKey(descriptionKey, rawItem.item.Description);
                            }

                            serializedObject.ApplyModifiedProperties();
                        }
                    }
                }
                EditorGUILayout.EndVertical();
            }
            GUILayout.EndArea();
        }
    }
}