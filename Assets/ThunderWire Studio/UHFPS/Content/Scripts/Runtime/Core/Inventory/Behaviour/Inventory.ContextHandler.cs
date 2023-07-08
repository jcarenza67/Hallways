using System.Linq;
using UnityEngine;
using UHFPS.Input;

namespace UHFPS.Runtime
{
    public partial class Inventory
    {
        public struct Shortcut
        {
            public ShortcutSlot slot;
            public InventoryItem item;
        }

        private InventoryItem activeItem;
        private ExamineController examine;
        private PlayerItemsManager playerItems;

        private readonly Shortcut[] shortcuts = new Shortcut[4];

        private bool bindShortcut;
        private bool itemSelector;

        public void InitializeContextHandler()
        {
            contextMenu.contextUse.onClick.AddListener(UseItem);
            contextMenu.contextExamine.onClick.AddListener(ExamineItem);
            contextMenu.contextCombine.onClick.AddListener(CombineItem);
            contextMenu.contextShortcut.onClick.AddListener(ShortcutItem);
            contextMenu.contextDrop.onClick.AddListener(DropItem);
            contextMenu.contextDiscard.onClick.AddListener(DiscardItem);
            examine = playerPresence.Component<ExamineController>();
            playerItems = playerPresence.PlayerManager.PlayerItems;

            // initialize shortcuts
            shortcuts[0].slot = shortcutSettings.Slot01;
            shortcuts[1].slot = shortcutSettings.Slot02;
            shortcuts[2].slot = shortcutSettings.Slot03;
            shortcuts[3].slot = shortcutSettings.Slot04;
        }

        public void ContextUpdate()
        {
            if (examine.IsExamining || gameManager.IsPaused) 
                return;

            for (int i = 0; i < shortcuts.Length; i++)
            {
                if(InputManager.ReadButtonOnce("Shortcut" + (1 + i), Controls.SHORTCUT_PREFIX + (1 + i)))
                {
                    if (bindShortcut && activeItem != null)
                    {
                        SetShortcut(i);
                        bindShortcut = false;
                    }
                    else if (shortcuts[i].item != null && !gameManager.IsInventoryShown)
                    {
                        UseItem(shortcuts[i].item);
                    }
                    break;
                }
            }
        }

        public void UseItem()
        {
            UseItem(activeItem);
            ShowContextMenu(false);
            activeItem = null;
        }

        private void UseItem(InventoryItem item)
        {
            Debug.Log("Use: " + item.Item.Title);

            if (itemSelector)
            {
                inventorySelector.OnInventoryItemSelect(this, item);
                inventorySelector = null;
                itemSelector = false;
            }
            else
            {
                var usableType = item.Item.UsableSettings.usableType;
                if (usableType == UsableType.PlayerItem)
                {
                    int playerItemIndex = item.Item.UsableSettings.playerItemIndex;
                    if(playerItemIndex >= 0) PlayerItems.SwitchPlayerItem(playerItemIndex);
                }
                else if(usableType == UsableType.HealthItem)
                {
                    PlayerHealth playerHealth = playerPresence.PlayerManager.PlayerHealth;
                    int healAmount = (int)item.Item.UsableSettings.healthPoints;
                    int currentHealth = playerHealth.EntityHealth;

                    if(currentHealth < playerHealth.MaxEntityHealth)
                    {
                        playerHealth.OnApplyHeal(healAmount);
                        RemoveItem(item, 1);
                    }
                }
            }

            gameManager.ShowInventoryPanel(false);
        }

        public void CombineItem()
        {
            foreach (var item in carryingItems)
            {
                bool hasCombination = activeItem.Item.CombineSettings.Any(x => x.combineWithID == item.Key.ItemGuid);
                item.Key.SetCombinable(true, hasCombination);
            }

            ShowContextMenu(false);
        }

        public void CombineWith(InventoryItem secondItem)
        {
            // reset the combinability status of items
            foreach (var item in carryingItems)
                item.Key.SetCombinable(false, false);

            // active = the item in which the combination was called
            var activeCombination = activeItem.Item.CombineSettings.FirstOrDefault(x => x.combineWithID == secondItem.ItemGuid);
            // second = the item that was used after selecting combine
            var secondCombination = secondItem.Item.CombineSettings.FirstOrDefault(x => x.combineWithID == activeItem.ItemGuid);

            // active combination events
            if (!string.IsNullOrEmpty(activeCombination.combineWithID))
            {
                // call active inventory item, player item combination events
                if (activeCombination.eventAfterCombine && secondItem.Item.UsableSettings.usableType == UsableType.PlayerItem)
                {
                    int playerItemIndex = secondItem.Item.UsableSettings.playerItemIndex;
                    var playerItem = playerItems.PlayerItems[playerItemIndex];

                    // check if it is possible to combine a player item (e.g. reload) with an active item
                    if (playerItem.CanCombine()) playerItem.OnItemCombine(activeItem);
                }

                // remove the active item if keepAfterCombine is false
                if (!activeCombination.keepAfterCombine)
                {
                    RemoveShortcut(activeItem);
                    RemoveItem(activeItem, 1);
                }
            }

            // second combination events
            if (!string.IsNullOrEmpty(secondCombination.combineWithID))
            {
                // remove the second item if keepAfterCombine is false
                if (!secondCombination.keepAfterCombine)
                {
                    RemoveShortcut(secondItem);
                    RemoveItem(secondItem, 1);
                }
            }

            // select player item after combine
            if (activeCombination.selectAfterCombine)
            {
                int playerItemIndex = activeCombination.playerItemIndex;
                if (playerItemIndex >= 0) playerPresence.PlayerManager.PlayerItems.SwitchPlayerItem(playerItemIndex);
            }

            if (!string.IsNullOrEmpty(activeCombination.resultCombineID))
            {
                AddItem(activeCombination.resultCombineID, 1, null);
            }

            activeItem = null;
        }

        public void ShortcutItem()
        {
            bindShortcut = true;
            ShowContextMenu(false);
            contextMenu.blockerPanel.SetActive(true);
        }

        private void SetShortcut(int index)
        {
            if(shortcuts[index].item == activeItem)
            {
                shortcuts[index].item = null;
                shortcuts[index].slot.SetItem(null);
            }
            else
            {
                // unbind from other slot
                RemoveShortcut(activeItem);

                // bind to a new slot
                shortcuts[index].item = activeItem;
                shortcuts[index].slot.SetItem(activeItem);
            }

            activeItem = null;
            bindShortcut = false;
            contextMenu.blockerPanel.SetActive(false);
        }

        private void SetShortcut(int index, InventoryItem item)
        {
            shortcuts[index].item = item;
            shortcuts[index].slot.SetItem(item);
        }

        private void RemoveShortcut(InventoryItem item)
        {
            for (int i = 0; i < shortcuts.Length; i++)
            {
                if (shortcuts[i].item == item)
                {
                    shortcuts[i].item = null;
                    shortcuts[i].slot.SetItem(null);
                    break;
                }
            }
        }

        public void ExamineItem()
        {
            Vector3 examinePosition = examine.InventoryPosition;
            Item item = activeItem.Item;

            if (item.ItemObject != null)
            {
                GameObject examineObj = Instantiate(item.ItemObject.Object, examinePosition, Quaternion.identity);
                examineObj.name = "Examine " + item.Title;
                examine.ExamineFromInventory(examineObj);
            }
            else
            {
                Debug.LogError("[Inventory] Could not examine an item because the item does not contain an item drop object!");
            }

            OnCloseInventory();
            activeItem = null;
        }

        public void DropItem()
        {
            Vector3 dropPosition = examine.DropPosition;
            Item item = activeItem.Item;

            if(item.ItemObject != null)
            {
                GameObject dropObj = SaveGameManager.InstantiateSaveable(item.ItemObject, dropPosition, Vector3.zero, "Drop of " + item.Title);

                if(dropObj.TryGetComponent(out Rigidbody rigidbody))
                {
                    rigidbody.useGravity = true;
                    rigidbody.isKinematic = false;
                    rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                    rigidbody.AddForce(playerPresence.PlayerCamera.transform.forward * settings.dropStrength);
                }
                else
                {
                    Debug.LogError("[Inventory] Drop item must have a Rigidbody component to apply drop force!");
                    return;
                }

                if(dropObj.TryGetComponent(out InteractableItem interactable))
                {
                    interactable.DisableType = InteractableItem.DisableTypeEnum.Destroy;
                    interactable.Quantity = (ushort)activeItem.Quantity;
                }

                RemoveItem(activeItem);
            }
            else
            {
                Debug.LogError("[Inventory] Could not drop an item because the item does not contain an item drop object!");
            }

            RemoveShortcut(activeItem);
            ShowContextMenu(false);
            activeItem = null;
        }

        public void DiscardItem()
        {
            RemoveShortcut(activeItem);
            RemoveItem(activeItem);
            ShowContextMenu(false);
            activeItem = null;
        }
    }
}