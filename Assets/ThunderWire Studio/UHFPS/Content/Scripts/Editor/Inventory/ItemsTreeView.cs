using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEditor;
using ThunderWire.Editors;

namespace UHFPS.Editors 
{
    public class ItemsTreeView : TreeView
    {
        public Subject<string> OnItemSelect = new();
        public Subject<Unit> OnAddNewItem = new();
        public Subject<Unit> OnDeleteItem = new();
        public Subject<Unit> OnDirty = new();

        private readonly InventoryBuilder.TempBuilderData builderData;
        private List<InventoryTreeViewItem> treeItems;
        private bool InitiateContextMenuOnNextRepaint = false;

        private Color normalItemColor = Color.white;
        private Color modifiedItemColor = new Color(1, 0.9f, 0.58f);

        private const string k_DeleteCommand = "Delete";
        private const string k_SoftDeleteCommand = "SoftDelete";

        internal class InventoryTreeViewItem : TreeViewItem
        {
            public InventoryBuilder.ItemProperty item;

            public InventoryTreeViewItem(int id, int depth, InventoryBuilder.ItemProperty item)
                : base(id, depth, item.title.stringValue)
            {
                this.item = item;
            }
        }

        public ItemsTreeView(TreeViewState viewState, InventoryBuilder.TempBuilderData builderData) : base(viewState)
        {
            this.builderData = builderData;
            Reload();
        }

        public void ChangeTitle(string itemID, string title)
        {
            foreach (var item in treeItems)
            {
                if(item.item.GUID == itemID)
                {
                    item.displayName = title;
                    break;
                }
            }
        }

        protected override TreeViewItem BuildRoot()
        {
            treeItems = new List<InventoryTreeViewItem>();
            var root = new TreeViewItem(0, -1);

            int index = 1;
            foreach (var item in builderData.ItemProperties)
            {
                InventoryTreeViewItem treeViewItem = new(index, 0, item.Value);
                treeItems.Add(treeViewItem);
                root.AddChild(treeViewItem);
                index++;
            }

            if (root.children == null)
                root.children = new List<TreeViewItem>();

            return root;
        }

        private void PopUpContextMenu()
        {
            var menu = new GenericMenu();

            menu.AddItem(new GUIContent("Delete"), false, () =>
            {
                DeleteSelected();
            });

            menu.ShowAsContext();
        }

        public override void OnGUI(Rect rect)
        {
            EditorGUIUtility.SetIconSize(new Vector2(15, 15));
            GUIContent headerTitle = EditorGUIUtility.TrTextContentWithIcon(" INVENTORY ITEMS", "Prefab On Icon");
            Rect headerRect = EditorDrawing.DrawHeaderWithBorder(ref rect, headerTitle, 20f, false);

            headerRect.xMin = headerRect.xMax - EditorGUIUtility.singleLineHeight - EditorGUIUtility.standardVerticalSpacing;
            headerRect.width = EditorGUIUtility.singleLineHeight;
            headerRect.y += EditorGUIUtility.standardVerticalSpacing;

            if (GUI.Button(headerRect, EditorUtils.Styles.PlusIcon, EditorUtils.Styles.IconButton))
            {
                OnAddNewItem.OnNext(Unit.Default);
            }

            if (InitiateContextMenuOnNextRepaint)
            {
                InitiateContextMenuOnNextRepaint = false;
                PopUpContextMenu();
            }

            HandleCommandEvent(Event.current);
            base.OnGUI(rect);
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            InventoryTreeViewItem treeViewItem = (InventoryTreeViewItem)args.item;

            Rect boxRect = args.rowRect;
            boxRect.width = 5f;
            boxRect.height -= 2f;
            boxRect.y += 1f;
            boxRect.x += 1f;
            EditorGUI.DrawRect(boxRect, treeViewItem.item.isModified ? modifiedItemColor : normalItemColor);

            Rect labelRect = args.rowRect;
            labelRect.x += 10f;
            EditorGUI.LabelField(labelRect, treeViewItem.displayName);
        }

        protected override float GetCustomRowHeight(int row, TreeViewItem item) => 20f;

        protected override bool CanMultiSelect(TreeViewItem item) => true;

        protected override bool CanStartDrag(CanStartDragArgs args) => true;

        protected override void SelectionChanged(IList<int> selectedIds)
        {
            if(selectedIds.Count == 1)
            {
                OnItemSelect.OnNext(treeItems[selectedIds[0] - 1].item.GUID);
            }
            else
            {
                OnItemSelect.OnNext(string.Empty);
            }
        }

        protected override void ContextClickedItem(int id)
        {
            InitiateContextMenuOnNextRepaint = true;
            Repaint();
        }

        protected override void SetupDragAndDrop(SetupDragAndDropArgs args)
        {
            DragAndDrop.PrepareStartDrag();
            DragAndDrop.SetGenericData("IDs", args.draggedItemIDs.Select(x => x - 1).ToArray());
            DragAndDrop.SetGenericData("Type", "InventoryItems");
            DragAndDrop.StartDrag("Items");
        }

        protected override DragAndDropVisualMode HandleDragAndDrop(DragAndDropArgs args)
        {
            int[] draggedIDs = (int[])DragAndDrop.GetGenericData("IDs");
            string type = (string)DragAndDrop.GetGenericData("Type");

            switch (args.dragAndDropPosition)
            {
                case DragAndDropPosition.BetweenItems:
                    {
                        if (type.Equals("InventoryItems"))
                        {
                            if (args.performDrop)
                            {
                                MoveElements(draggedIDs, args.insertAtIndex);
                            }

                            return DragAndDropVisualMode.Move;
                        }
                        return DragAndDropVisualMode.Rejected;
                    }
                case DragAndDropPosition.UponItem:
                case DragAndDropPosition.OutsideItems:
                    return DragAndDropVisualMode.Rejected;
                default:
                    Debug.LogError("Unhandled enum " + args.dragAndDropPosition);
                    return DragAndDropVisualMode.None;
            }
        }

        private void MoveElements(int[] drag, int insert)
        {
            var _tmpItemsList = builderData.ItemProperties.ToList();
            var _tmpItems = (from item in _tmpItemsList
                             let i = _tmpItemsList.Select(x => x.Key).ToList().IndexOf(item.Key)
                             where drag.Any(x => x == i)
                             select item).ToArray();

            for (int i = 0; i < _tmpItems.Count(); i++)
            {
                var item = _tmpItems[i];
                int index = _tmpItemsList.Select(x => x.Key).ToList().IndexOf(item.Key);
                int insertTo = insert > index ? insert - 1 : insert;

                _tmpItemsList.RemoveAt(index);
                _tmpItemsList.Insert(insertTo, item);
            }

            builderData.ItemProperties = _tmpItemsList.ToDictionary(x => x.Key, y => y.Value);
            OnDirty.OnNext(Unit.Default);
            SetSelection(new int[0]);
            Reload();
        }

        private void HandleCommandEvent(Event uiEvent)
        {
            if (uiEvent.type == EventType.ValidateCommand)
            {
                switch (uiEvent.commandName)
                {
                    case k_DeleteCommand:
                    case k_SoftDeleteCommand:
                        if (HasSelection())
                            uiEvent.Use();
                        break;
                }
            }
            else if (uiEvent.type == EventType.ExecuteCommand)
            {
                switch (uiEvent.commandName)
                {
                    case k_DeleteCommand:
                    case k_SoftDeleteCommand:
                        DeleteSelected();
                        OnDeleteItem.OnNext(Unit.Default);
                        break;
                }
            }
        }

        private void DeleteSelected()
        {
            var toDelete = GetSelection().Select(x => x - 1).OrderByDescending(i => i);
            SetSelection(new int[0]);

            foreach (var index in toDelete)
            {
                builderData.ItemProperties.Remove(treeItems[index].item.GUID);
            }

            builderData.AssetObject.ApplyModifiedProperties();
            builderData.AssetObject.Update();
            OnDeleteItem.OnNext(Unit.Default);
            Reload();
        }
    }
}