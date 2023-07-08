using System;
using UnityEngine;
using UnityEditor;
using UHFPS.Runtime;
using UHFPS.Scriptable;
using ThunderWire.Editors;
using UnityEditor.IMGUI.Controls;

namespace UHFPS.Editors
{
    [CustomPropertyDrawer(typeof(GString))]
    public class GStringDrawer : PropertyDrawer
    {
        private readonly Lazy<GameLocalizationAsset> asset = new(() =>
        {
            if (GameLocalization.HasReference)
            {
                GameLocalizationAsset asset = GameLocalization.Instance.LocalizationAsset;
                return asset;
            }

            return null;
        });

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty glocProp = property.FindPropertyRelative("GlocText");
            SerializedProperty textProp = property.FindPropertyRelative("NormalText");

            EditorGUI.BeginProperty(position, label, property);
            {
#if UHFPS_LOCALIZATION
                Rect finderRect = position;
                finderRect.xMin = finderRect.xMax - EditorGUIUtility.singleLineHeight;

                Rect dropdownRect = position;
                dropdownRect.width = 250f;
                dropdownRect.height = 0f;
                dropdownRect.y += 21f;
                dropdownRect.x += position.xMax - dropdownRect.width - EditorGUIUtility.singleLineHeight;

                position.xMax -= EditorGUIUtility.singleLineHeight + 2f;
                glocProp.stringValue = EditorGUI.TextField(position, label, glocProp.stringValue);

                GlocPicker glocPicker = new(new AdvancedDropdownState(), asset.Value);
                glocPicker.SelectedKey = glocProp.stringValue;
                glocPicker.OnItemPressed = (key) =>
                {
                    glocProp.stringValue = key;
                    property.serializedObject.ApplyModifiedProperties();
                };

                GUIContent finderIcon = EditorGUIUtility.TrIconContent("Search Icon", "Show GLoc Selector");
                using (new EditorGUI.DisabledGroupScope(asset.Value == null))
                {
                    if (GUI.Button(finderRect, finderIcon, EditorStyles.iconButton))
                    {
                        glocPicker.Show(dropdownRect, 370);
                    }
                }
#else
                textProp.stringValue = EditorGUI.TextField(position, label, textProp.stringValue);
#endif
            }
            EditorGUI.EndProperty();
        }

        private class GlocPicker : AdvancedDropdown
        {
            private class GlocElement : AdvancedDropdownItem
            {
                public string glocKey;
                public bool isNone;

                public GlocElement(string displayName) : base(displayName) { }

                public GlocElement(string displayName, string key) : base(displayName)
                {
                    glocKey = key;
                }

                public GlocElement() : base("Empty")
                {
                    isNone = true;
                }
            }

            public string SelectedKey;
            public Action<string> OnItemPressed;

            private readonly GameLocalizationAsset localizationAsset;

            public GlocPicker(AdvancedDropdownState state, GameLocalizationAsset localizationAsset) : base(state)
            {
                this.localizationAsset = localizationAsset;
                minimumSize = new Vector2(200f, 250f);
            }

            protected override AdvancedDropdownItem BuildRoot()
            {
                var root = new AdvancedDropdownItem("GLoc Key Selector");
                root.AddChild(new GlocElement()); // none selector

                foreach (var section in localizationAsset.Localizations)
                {
                    var sectionElement = new GlocElement(section.Section);

                    foreach (var localization in section.Localizations)
                    {
                        string key = section.Section + "." + localization.Key;
                        var localizationElement = new GlocElement(key, key);

                        if (SelectedKey == key)
                        {
                            localizationElement.icon = (Texture2D)EditorGUIUtility.TrIconContent("FilterSelectedOnly").image;
                        }

                        sectionElement.AddChild(localizationElement);
                    }

                    root.AddChild(sectionElement);
                }

                return root;
            }

            protected override void ItemSelected(AdvancedDropdownItem item)
            {
                var element = item as GlocElement;
                if (!element.isNone) OnItemPressed?.Invoke(element.glocKey);
                else OnItemPressed?.Invoke(string.Empty);
            }
        }
    }
}