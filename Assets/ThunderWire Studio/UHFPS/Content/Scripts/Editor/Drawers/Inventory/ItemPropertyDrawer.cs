using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UHFPS.Runtime;
using UHFPS.Scriptable;
using ThunderWire.Editors;

namespace UHFPS.Editors
{
    [CustomPropertyDrawer(typeof(ItemProperty))]
    public class ItemPropertyDrawer : PropertyDrawer
    {
        private ItemProperty target;
        private readonly InventoryAsset inventoryAsset;
        private readonly bool hasInvReference;

        GUIStyle RichLabel => new GUIStyle(EditorStyles.miniBoldLabel) { richText = true };

        public ItemPropertyDrawer()
        {
            if (Inventory.HasReference)
            {
                inventoryAsset = Inventory.Instance.inventoryAsset;
                hasInvReference = true;
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (target == null)
            {
                object fieldObject = fieldInfo.GetValue(property.serializedObject.targetObject);
                target = (ItemProperty)fieldObject;
            }

            SerializedProperty guid = property.FindPropertyRelative("GUID");

            Item item = target?.GetItemRaw() ?? new Item();
            Texture itemIcon = item.Icon != null ? item.Icon.texture : null;
            string itemGUID = guid.stringValue ?? "None";
            string itemTitle = item.Title;

            if (!hasInvReference)
            {
                itemTitle = "<color=#ED213A>Inventory component reference is missing!</color>";
            }
            else if(inventoryAsset == null)
            {
                itemTitle = "<color=#ED213A>Inventory asset not defined!</color>";
            }
            else
            {
                itemTitle = "Title: " + itemTitle;
            }

            Rect dropdownRect = position;
            dropdownRect.width = 250f;
            dropdownRect.height = 0f;
            dropdownRect.y += 21f;
            dropdownRect.x += position.xMax - dropdownRect.width - EditorGUIUtility.singleLineHeight;

            Rect headerRect = EditorDrawing.DrawHeaderWithBorder(ref position, label);
            headerRect.width = EditorGUIUtility.singleLineHeight;
            headerRect.height = EditorGUIUtility.singleLineHeight;
            headerRect.x = position.width;
            headerRect.y += 2f;

            using (new EditorDrawing.IconSizeScope(16))
            {
                using (new EditorGUI.DisabledGroupScope(inventoryAsset == null))
                {
                    if (GUI.Button(headerRect, EditorUtils.Styles.Linked, EditorUtils.Styles.IconButton))
                    {
                        ItemPicker itemPicker = new ItemPicker(new AdvancedDropdownState(), inventoryAsset);
                        itemPicker.OnItemPressed += obj =>
                        {
                            guid.stringValue = obj.guid;
                            property.serializedObject.ApplyModifiedProperties();
                        };

                        itemPicker.Show(dropdownRect);
                    }
                }
            }

            Rect iconPreviewRect = position;
            float iconPreviewSize = 3f + EditorGUIUtility.singleLineHeight * 2;
            iconPreviewRect.width = iconPreviewSize;
            iconPreviewRect.height = iconPreviewSize;
            iconPreviewRect.y += 2f;
            iconPreviewRect.x += 2f;

            Rect iconPreviewTextureRect = iconPreviewRect;
            if (!itemIcon)
            {
                iconPreviewTextureRect.width -= 18f;
                iconPreviewTextureRect.height -= 18f;
                iconPreviewTextureRect.y += 9f;
                iconPreviewTextureRect.x += 9f;
            }

            GUI.Box(iconPreviewRect, GUIContent.none, EditorStyles.helpBox);
            Texture missingImage = EditorGUIUtility.TrIconContent("scenevis_hidden@2x").image;
            EditorDrawing.DrawTransparentTexture(iconPreviewTextureRect, itemIcon != null ? itemIcon : missingImage);

            Rect itemTitleRect = iconPreviewRect;
            itemTitleRect.width = position.width;
            itemTitleRect.height = EditorGUIUtility.singleLineHeight;
            itemTitleRect.xMin += iconPreviewSize + EditorGUIUtility.standardVerticalSpacing;
            itemTitleRect.xMax -= 4f;
            itemTitleRect.y += 2f;

            Rect itemGUIDRect = itemTitleRect;
            itemGUIDRect.y += EditorGUIUtility.singleLineHeight - 1f;

            EditorGUI.LabelField(itemTitleRect, itemTitle, RichLabel);
            EditorGUI.LabelField(itemGUIDRect, "GUID: " + itemGUID, EditorStyles.miniBoldLabel);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 3 + 11f;
        }

        public class ItemPicker : AdvancedDropdown
        {
            private class ItemPickerDropdownItem : AdvancedDropdownItem
            {
                public InventoryAsset.ReferencedItem item;

                public ItemPickerDropdownItem(InventoryAsset.ReferencedItem item) : base(item.item.Title)
                {
                    this.item = item;
                }
            }

            private readonly InventoryAsset asset;
            public event Action<InventoryAsset.ReferencedItem> OnItemPressed;

            public ItemPicker(AdvancedDropdownState state, InventoryAsset asset) : base(state) 
            {
                this.asset = asset;
                minimumSize = new Vector2(200f, 250f);
            }

            protected override AdvancedDropdownItem BuildRoot()
            {
                var root = new AdvancedDropdownItem("Inventory Items");

                if (asset != null)
                {
                    foreach (var item in asset.Items)
                    {
                        var dropdownItem = new ItemPickerDropdownItem(item);
                        dropdownItem.icon = (Texture2D)EditorGUIUtility.TrIconContent("Prefab On Icon").image;
                        root.AddChild(dropdownItem);
                    }
                }

                return root;
            }

            protected override void ItemSelected(AdvancedDropdownItem item)
            {
                OnItemPressed?.Invoke((item as ItemPickerDropdownItem).item);
            }
        }
    }
}