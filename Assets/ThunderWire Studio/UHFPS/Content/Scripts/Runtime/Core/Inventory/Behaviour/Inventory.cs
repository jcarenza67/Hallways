using System;
using System.Reactive.Subjects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UHFPS.Scriptable;
using Newtonsoft.Json.Linq;
using TMPro;

namespace UHFPS.Runtime
{
    public enum Orientation { Horizontal, Vertical };
    public enum InventorySound { ItemSelect, ItemMove, ItemPut, ItemError }

    public partial class Inventory : Singleton<Inventory>, ISaveableCustom
    {
        #region Structures
        [Serializable]
        public sealed class Settings
        {
            public ushort rows = 5;
            public ushort columns = 5;
            public float cellSize = 100f;
            public float spacing = 10f;
            public float dragTime = 0.05f;
            public float rotateTime = 0.05f;
            public float dropStrength = 10f;
        }

        [Serializable]
        public sealed class SlotSettings
        {
            public GameObject slotPrefab;
            public GameObject slotItemPrefab;

            [Header("Slot Textures")]
            public Sprite normalSlotFrame;
            public Sprite restrictedSlotFrame;

            [Header("Slot Colors")]
            public Color itemNormalColor = Color.white;
            public Color itemHoverColor = Color.white;
            public Color itemMoveColor = Color.white;
            public Color itemErrorColor = Color.white;
            public float colorChangeSpeed = 20f;

            [Header("Slot Quantity")]
            public Color normalQuantityColor = Color.white;
            public Color zeroQuantityColor = Color.red;
        }

        [Serializable]
        public sealed class ContainerSettings
        {
            public RectTransform containerObject;
            public RectTransform containerItems;
            public GridLayoutGroup containerSlots;
        }

        [Serializable]
        public sealed class ItemInfo
        {
            public GameObject infoPanel;
            public TMP_Text infoText;
            public float titleFontSize = 25f;
            public float descriptionFontSize = 20f;
            public Color descriptionFontColor = Color.white;
        }

        [Serializable]
        public sealed class ContextMenu
        {
            public GameObject contextMenu;
            public GameObject blockerPanel;

            [Header("Context Buttons")]
            public Button contextUse;
            public Button contextExamine;
            public Button contextCombine;
            public Button contextShortcut;
            public Button contextDrop;
            public Button contextDiscard;
        }

        [Serializable]
        public sealed class ShortcutSettings
        {
            public ShortcutSlot Slot01;
            public ShortcutSlot Slot02;
            public ShortcutSlot Slot03;
            public ShortcutSlot Slot04;
        }

        [Serializable]
        public sealed class Sounds
        {
            public AudioClip itemSelectSound;
            public AudioClip itemMoveSound;
            public AudioClip itemPutSound;
            public AudioClip itemErrorSound;

            [Header("Settings")]
            [Range(0f, 1f)]
            public float volume = 1f;
            public float nextSoundDelay = 0.1f;
        }

        [Serializable]
        public sealed class ExpandableSlots
        {
            public bool enabled;
            public bool showExpandableSlots;
            public ushort expandableRows;
        }

        [Serializable]
        public struct StartingItem
        {
            public string GUID;
            public string title;
            public ushort quantity;
            public ItemCustomData data;
        }

        public struct OccupyData
        {
            public InventoryItem inventoryItem;
            public InventorySlot[] occupiedSlots;
        }

        public struct FreeSpace
        {
            public int x;
            public int y;
            public Orientation orientation;

            public FreeSpace(int x, int y, Orientation orientation)
            {
                this.x = x;
                this.y = y;
                this.orientation = orientation;
            }
        }

        public struct ItemCreationData
        {
            public string itemGuid;
            public ushort quantity;
            public Orientation orientation;
            public Vector2Int coords;
            public ItemCustomData customData;
            public Transform parent;
            public InventorySlot[,] slotsSpace;
        }
        #endregion

        public InventoryAsset inventoryAsset;

        // references
        public Transform inventoryContainers;
        public GridLayoutGroup slotsLayoutGrid;
        public Transform itemsTransform;

        // settings
        public Settings settings;
        public SlotSettings slotSettings;
        public ContainerSettings containerSettings;
        public ItemInfo itemInfo;
        public ShortcutSettings shortcutSettings;
        public ContextMenu contextMenu;
        public Sounds sounds;

        // features
        public List<StartingItem> startingItems = new();
        public ExpandableSlots expandableSlots;

        // inventory
        public InventorySlot[,] slots;
        public Dictionary<string, Item> items;
        public Dictionary<InventoryItem, InventorySlot[]> carryingItems;

        // container
        public InventoryContainer currentContainer;
        public InventorySlot[,] containerSlots;
        public Dictionary<InventoryItem, InventorySlot[]> containerItems;

        private int expandedRows;
        private float nextSoundDelay;
        private bool contextShown;

        private IInventorySelector inventorySelector;
        private PlayerPresenceManager playerPresence;
        private GameManager gameManager;
        private AudioSource inventorySounds;

        public GameObject Player => playerPresence.Player;

        public PlayerItemsManager PlayerItems => playerPresence.PlayerManager.PlayerItems;

        public bool ContainerOpened => currentContainer != null;

        public Subject<InventoryItem> OnItemAdded = new(); 
        public Subject<string> OnItemRemoved = new(); 

        /// <summary>
        /// Number of vertical slots.
        /// </summary>
        public int SlotRows
        {
            get
            {
                if (expandableSlots.enabled)
                    return settings.rows - (expandableSlots.expandableRows - expandedRows);

                return settings.rows;
            }
        }

        /// <summary>
        /// Number of max vertical slots.
        /// </summary>
        public int MaxSlotRows
        {
            get
            {
                return ContainerOpened
                    ? Math.Max(SlotRows, currentContainer.Rows)
                    : SlotRows;
            }
        }

        /// <summary>
        /// Number of horizontal slots.
        /// </summary>
        public int SlotColumns
        {
            get
            {
                if (ContainerOpened)
                    return currentContainer.Columns + settings.columns;

                return settings.columns;
            }
        }

        public InventorySlot this[int y, int x]
        {
            get
            {
                if (ContainerOpened)
                {
                    if (IsContainerCoords(x, y, out bool isContainer))
                    {
                        if (isContainer)
                        {
                            return containerSlots[y, x];
                        }
                        else
                        {
                            int X = x - currentContainer.Columns;
                            return slots[y, X];
                        }
                    }

                    return null;
                }

                return slots[y, x];
            }
        }

        private void Awake()
        {
            slots = new InventorySlot[settings.rows, settings.columns];
            items = new Dictionary<string, Item>();
            carryingItems = new Dictionary<InventoryItem, InventorySlot[]>();
            containerItems = new Dictionary<InventoryItem, InventorySlot[]>();

            // slot grid setting
            slotsLayoutGrid.cellSize = new Vector2(settings.cellSize, settings.cellSize);
            slotsLayoutGrid.spacing = new Vector2(settings.spacing, settings.spacing);

            // slot instantiation
            for (int y = 0; y < settings.rows; y++)
            {
                for (int x = 0; x < settings.columns; x++)
                {
                    GameObject slot = Instantiate(slotSettings.slotPrefab, slotsLayoutGrid.transform);
                    slot.name = $"Slot [{y},{x}]";

                    RectTransform rect = slot.GetComponent<RectTransform>();
                    rect.localScale = Vector3.one;

                    InventorySlot inventorySlot = slot.GetComponent<InventorySlot>();
                    slots[y, x] = inventorySlot;

                    if (expandableSlots.enabled && y >= settings.rows - expandableSlots.expandableRows)
                    {
                        inventorySlot.frame.sprite = slotSettings.restrictedSlotFrame;
                        inventorySlot.CanvasGroup.alpha = 0.3f;
                    }
                }
            }

            if (!inventoryAsset) throw new NullReferenceException("Inventory asset is not set!");

            // item caching
            foreach (var item in inventoryAsset.Items)
            {
                Item itemClone = item.item.DeepCopy();

#if UHFPS_LOCALIZATION
                itemClone.LocalizationSettings.titleKey.SubscribeGloc(text => itemClone.Title = text);
                itemClone.LocalizationSettings.descriptionKey.SubscribeGloc(text => itemClone.Description = text);
#endif

                items.Add(item.guid, itemClone);
            }

            // initialize other stuff
            playerPresence = GetComponent<PlayerPresenceManager>();
            gameManager = GetComponent<GameManager>();
            inventorySounds = GetComponent<AudioSource>();
            contextMenu.contextMenu.SetActive(false);
            contextMenu.blockerPanel.SetActive(false);
            itemInfo.infoPanel.SetActive(false);

            // initialize context handler
            InitializeContextHandler();
        }

        private void Start()
        {
            if (!SaveGameManager.GameWillLoad)
            {
                foreach (var item in startingItems)
                {
                    AddItem(item.GUID, item.quantity, item.data);
                }
            }
        }

        private void Update()
        {
            nextSoundDelay = nextSoundDelay > 0
                ? nextSoundDelay -= Time.deltaTime : 0;

            ContextUpdate();
        }

        /// <summary>
        /// Add item to the free inventory space.
        /// </summary>
        /// <param name="guid">Unique ID of the item.</param>
        /// <param name="quantity">Quantity of the item to be added.</param>
        /// <param name="customData">Custom data of specified item.</param>
        /// <returns>Status whether the item has been added to the inventory.</returns>
        public bool AddItem(string guid, ushort quantity, ItemCustomData customData)
        {
            if (items.ContainsKey(guid))
            {
                Item item = items[guid];
                ushort maxStack = item.Properties.maxStack;
                InventoryItem inventoryItem = null;

                if (ContainsItem(guid, out OccupyData itemData) && item.Settings.isStackable)
                {
                    inventoryItem = itemData.inventoryItem;
                    int currQuantity = itemData.inventoryItem.Quantity;
                    int remainingQ = 0;

                    if (maxStack == 0)
                    {
                        currQuantity += quantity;
                        itemData.inventoryItem.SetQuantity(currQuantity);
                    }
                    else if (currQuantity < maxStack)
                    {
                        int newQ = currQuantity + quantity;
                        int q = Mathf.Min(maxStack, newQ);
                        remainingQ = newQ - q;
                        itemData.inventoryItem.SetQuantity(q);
                    }

                    if (remainingQ > 0)
                    {
                        int iterations = (int)Math.Ceiling((float)remainingQ / maxStack);
                        for (int i = 0; i < iterations; i++)
                        {
                            int q = Mathf.Min(maxStack, remainingQ);
                            inventoryItem = CreateItem(guid, (ushort)q, customData);
                            remainingQ -= maxStack;
                        }
                    }
                }
                else
                {
                    if (quantity < maxStack || maxStack == 0)
                    {
                        inventoryItem = CreateItem(guid, quantity, customData);
                    }
                    else
                    {
                        int iterations = (int)Math.Ceiling((float)quantity / maxStack);
                        for (int i = 0; i < iterations; i++)
                        {
                            int q = Mathf.Min(maxStack, quantity);
                            inventoryItem = CreateItem(guid, (ushort)q, customData);
                            quantity -= maxStack;
                        }
                    }
                }

                if (inventoryItem != null)
                {
                    OnItemAdded.OnNext(inventoryItem);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Remove item from inventory completly.
        /// </summary>
        /// <param name="guid">Unique ID of the item.</param>
        /// <returns>Status whether the item has been removed from the inventory.</returns>
        public bool RemoveItem(string guid)
        {
            if (ContainsItem(guid, out OccupyData itemData))
            {
                carryingItems.Remove(itemData.inventoryItem);
                Destroy(itemData.inventoryItem.gameObject);
                foreach (var slot in itemData.occupiedSlots)
                {
                    slot.itemInSlot = null;
                }

                OnItemRemoved.OnNext(guid);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Remove item quantity from inventory.
        /// </summary>
        /// <param name="guid">Unique ID of the item.</param>
        /// <param name="quantity">Quantity of the item to be removed.</param>
        /// <returns>Quantity of the item in the inevntory.</returns>
        public int RemoveItem(string guid, ushort quantity)
        {
            if (ContainsItem(guid, out OccupyData itemData))
            {
                if ((itemData.inventoryItem.Quantity - quantity) >= 1)
                {
                    int q = itemData.inventoryItem.Quantity - quantity;
                    itemData.inventoryItem.SetQuantity(q);
                    OnItemRemoved.OnNext(guid);
                    return q;
                }
                else
                {
                    carryingItems.Remove(itemData.inventoryItem);
                    Destroy(itemData.inventoryItem.gameObject);
                    foreach (var slot in itemData.occupiedSlots)
                    {
                        slot.itemInSlot = null;
                    }

                    OnItemRemoved.OnNext(guid);
                }
            }

            return 0;
        }

        /// <summary>
        /// Remove item from inventory or container completly.
        /// </summary>
        public void RemoveItem(InventoryItem inventoryItem)
        {
            if (!inventoryItem.isContainerItem && carryingItems.ContainsKey(inventoryItem))
            {
                InventorySlot[] occupiedSlots = carryingItems[inventoryItem];
                carryingItems.Remove(inventoryItem);
                foreach (var slot in occupiedSlots)
                {
                    slot.itemInSlot = null;
                }
            }
            else if (containerItems.ContainsKey(inventoryItem))
            {
                InventorySlot[] occupiedSlots = containerItems[inventoryItem];
                containerItems.Remove(inventoryItem);
                foreach (var slot in occupiedSlots)
                {
                    slot.itemInSlot = null;
                }

                if (currentContainer != null)
                    currentContainer.Remove(inventoryItem);
            }

            OnItemRemoved.OnNext(inventoryItem.ItemGuid);
            Destroy(inventoryItem.gameObject);
        }

        /// <summary>
        /// Remove item quantity from inventory.
        /// </summary>
        public int RemoveItem(InventoryItem inventoryItem, ushort quantity)
        {
            if (carryingItems.ContainsKey(inventoryItem))
            {
                if ((inventoryItem.Quantity - quantity) >= 1)
                {
                    int q = inventoryItem.Quantity - quantity;
                    inventoryItem.SetQuantity(q);
                    OnItemRemoved.OnNext(inventoryItem.ItemGuid);
                    return q;
                }
                else
                {
                    InventorySlot[] occupiedSlots = carryingItems[inventoryItem];
                    carryingItems.Remove(inventoryItem);
                    foreach (var slot in occupiedSlots)
                    {
                        slot.itemInSlot = null;
                    }

                    OnItemRemoved.OnNext(inventoryItem.ItemGuid);
                    Destroy(inventoryItem.gameObject);
                }
            }

            return 0;
        }

        /// <summary>
        /// Get <see cref="InventoryItem"/> reference from Inventory.
        /// </summary>
        public InventoryItem GetInventoryItem(string guid)
        {
            if(ContainsItem(guid, out OccupyData occupyData))
                return occupyData.inventoryItem;

            return null;
        }

        /// <summary>
        /// Get <see cref="OccupyData"/> reference from Inventory.
        /// </summary>
        public OccupyData GetOccupyData(string guid)
        {
            if (ContainsItem(guid, out OccupyData occupyData))
                return occupyData;

            return default;
        }

        /// <summary>
        /// Get the quantity of the item in the inevntory.
        /// </summary>
        public int GetItemQuantity(string guid)
        {
            if (ContainsItem(guid, out OccupyData occupyData))
                return occupyData.inventoryItem.Quantity;

            return 0;
        }

        /// <summary>
        /// Set the quantity of the item in the inevntory.
        /// </summary>
        public void SetItemQuantity(string guid, ushort quantity, bool removeWhenZero = true)
        {
            if (ContainsItem(guid, out OccupyData occupyData))
            {
                if (quantity >= 1 || !removeWhenZero)
                {
                    occupyData.inventoryItem.SetQuantity(quantity);
                }
                else if(removeWhenZero)
                {
                    carryingItems.Remove(occupyData.inventoryItem);
                    Destroy(occupyData.inventoryItem.gameObject);
                    foreach (var slot in occupyData.occupiedSlots)
                    {
                        slot.itemInSlot = null;
                    }
                }
            }
        }

        /// <summary>
        /// Expand the inventory slots that are expandable.
        /// </summary>
        /// <param name="rows">Rows to be expanded.</param>
        public void ExpandRows(ushort rows)
        {
            int expandableRows = expandableSlots.expandableRows - expandedRows;
            if (expandableSlots.enabled && rows <= expandableRows)
            {
                int expanded = 0;
                for (int y = 0; y < settings.rows; y++)
                {
                    if(y >= settings.rows - expandableRows)
                    {
                        for (int x = 0; x < settings.columns; x++)
                        {
                            InventorySlot slot = slots[y, x];
                            slot.frame.sprite = slotSettings.normalSlotFrame;
                            slot.CanvasGroup.alpha = 1f;
                        }

                        if (++expanded >= rows) break;
                    }
                }

                expandedRows += rows;
            }
        }

        /// <summary>
        /// Check if there free space from the desired position.
        /// </summary>
        /// <param name="x">Slot X position.</param>
        /// <param name="y">Slot Y position.</param>
        /// <returns>Status whether there is free space in desired position.</returns>
        public bool CheckSpaceFromPosition(int x, int y, ushort width, ushort height, InventoryItem item = null)
        {
            for (int yy = y; yy < y + height; yy++)
            {
                for (int xx = x; xx < x + width; xx++)
                {
                    if (yy < MaxSlotRows && xx < SlotColumns)
                    {
                        InventorySlot slot = this[yy, xx];
                        if (slot == null) return false;

                        if (slot.itemInSlot != null)
                        {
                            if (item != null && slot.itemInSlot == item) continue;
                            return false;
                        }
                    }
                    else return false;
                }
            }

            // check if width of the item has not overflowed from the container into the inventory
            if (currentContainer != null)
            {
                return (x <= currentContainer.Columns && x + width <= currentContainer.Columns) ||
                    (x >= currentContainer.Columns && x + width >= currentContainer.Columns);
            }

            return true;
        }

        /// <summary>
        /// Move item to the desired position.
        /// </summary>
        public void MoveItem(Vector2Int lastCoords, Vector2Int newCoords, InventoryItem inventoryItem)
        {
            // check if the last and new coordinates are in the inventory or container space
            bool lastContainerSpace = false, newContainerSpace = false;
            if (ContainerOpened)
            {
                IsContainerCoords(lastCoords.x, lastCoords.y, out lastContainerSpace);
                IsContainerCoords(newCoords.x, newCoords.y, out newContainerSpace);
            }

            if (!lastContainerSpace)
            {
                // unoccupy slots from inventory space
                if (carryingItems.TryGetValue(inventoryItem, out var inventorySlots))
                {
                    foreach (var slot in inventorySlots)
                    {
                        slot.itemInSlot = null;
                    }
                }
            }
            else
            {
                // unoccupy slots from container space
                if (containerItems.TryGetValue(inventoryItem, out var containerSlots))
                {
                    foreach (var slot in containerSlots)
                    {
                        slot.itemInSlot = null;
                    }
                }
            }

            // if the new coordinates are in inventory space
            if (!newContainerSpace)
            {
                if (lastContainerSpace)
                {
                    // remove item from the container space
                    currentContainer.Remove(inventoryItem);
                    containerItems.Remove(inventoryItem);
                    inventoryItem.ContainerGuid = string.Empty;
                    inventoryItem.isContainerItem = false;

                    // add item to inventory space
                    carryingItems.Add(inventoryItem, null);
                }

                // set item parent to the inventory panel transform
                inventoryItem.transform.SetParent(itemsTransform);
            }
            // if the new coordinates are in container space
            else
            {
                if (!lastContainerSpace)
                {
                    // remove item from the inventory space
                    carryingItems.Remove(inventoryItem);
                    RemoveShortcut(inventoryItem);

                    // add item to container space
                    currentContainer.Store(inventoryItem, newCoords);
                    containerItems.Add(inventoryItem, null);
                    inventoryItem.isContainerItem = true;
                }
                else
                {
                    // move a container item to new coordinates
                    currentContainer.Move(inventoryItem, new FreeSpace()
                    {
                        x = newCoords.x,
                        y = newCoords.y,
                        orientation = inventoryItem.orientation
                    });
                }

                // set item parent to the container panel transform
                inventoryItem.transform.SetParent(containerSettings.containerItems);

                // if the item is equipped, unequip the current item
                if (inventoryItem.Item.UsableSettings.usableType == UsableType.PlayerItem)
                {
                    int playerItemIndex = inventoryItem.Item.UsableSettings.playerItemIndex;
                    if (playerItemIndex >= 0 && PlayerItems.CurrentItemIndex == playerItemIndex)
                        PlayerItems.DeselectCurrent();
                }
            }

            // occupy new slots
            OccupySlots(newContainerSpace, newCoords, inventoryItem);
        }

        /// <summary>
        /// Occupy slots with the item in the new coordinates.
        /// </summary>
        private void OccupySlots(bool isContainerSpace, Vector2Int newCoords, InventoryItem inventoryItem)
        {
            Item item = inventoryItem.Item;
            int maxY = item.Height, maxX = item.Width;

            // rotate the item if the orientation is vertical
            if (inventoryItem.orientation == Orientation.Vertical)
            {
                maxY = item.Width;
                maxX = item.Height;
            }

            InventorySlot[] slotsToOccupy = new InventorySlot[maxY * maxX];

            int slotIndex = 0;
            for (int yy = newCoords.y; yy < newCoords.y + maxY; yy++)
            {
                for (int xx = newCoords.x; xx < newCoords.x + maxX; xx++)
                {
                    InventorySlot slot = this[yy, xx];
                    slot.itemInSlot = inventoryItem;
                    slotsToOccupy[slotIndex++] = slot;
                }
            }

            if (!isContainerSpace)
            {
                carryingItems[inventoryItem] = slotsToOccupy;
            }
            else
            {
                containerItems[inventoryItem] = slotsToOccupy;
            }
        }

        /// <summary>
        /// Open the inventory container.
        /// </summary>
        /// <param name="container">Container to be opened.</param>
        public void OpenContainer(InventoryContainer container)
        {
            // initialize container slots
            containerSlots = new InventorySlot[container.Rows, container.Columns];
            currentContainer = container;

            // slot grid setting
            containerSettings.containerSlots.cellSize = new Vector2(settings.cellSize, settings.cellSize);
            containerSettings.containerSlots.spacing = new Vector2(settings.spacing, settings.spacing);

            // set the container panel size to fit the number of container columns
            Vector2 grdLayoutSize = containerSettings.containerObject.sizeDelta;
            grdLayoutSize.x = settings.cellSize * container.Columns + settings.spacing * (container.Columns - 1);
            containerSettings.containerObject.sizeDelta = grdLayoutSize;

            // slot instantiation
            for (int y = 0; y < container.Rows; y++)
            {
                for (int x = 0; x < container.Columns; x++)
                {
                    GameObject slot = Instantiate(slotSettings.slotPrefab, containerSettings.containerSlots.transform);
                    slot.name = $"Container Slot [{y},{x}]";

                    RectTransform rect = slot.GetComponent<RectTransform>();
                    rect.localScale = Vector3.one;

                    InventorySlot inventorySlot = slot.GetComponent<InventorySlot>();
                    containerSlots[y, x] = inventorySlot;
                }
            }

            // container items creation
            foreach (var containerItem in container.ContainerItems)
            {
                InventoryItem inventoryItem = CreateItem(new ItemCreationData()
                {
                    itemGuid = containerItem.Value.ItemGuid,
                    quantity = (ushort)containerItem.Value.Quantity,
                    orientation = containerItem.Value.Orientation,
                    coords = containerItem.Value.Coords,
                    customData = containerItem.Value.CustomData,
                    parent = containerSettings.containerItems,
                    slotsSpace = containerSlots
                });

                inventoryItem.ContainerGuid = containerItem.Key;
                inventoryItem.isContainerItem = true;
                inventoryItem.ContainerOpened(currentContainer.Columns);

                containerItems.Add(inventoryItem, null);
                OccupySlots(true, containerItem.Value.Coords, inventoryItem);
            }

            // set carrying items container opened
            foreach (var carryingItem in carryingItems.Keys)
            {
                carryingItem.ContainerOpened(currentContainer.Columns);
            }

            containerSettings.containerObject.gameObject.SetActive(true);

            gameManager.SetBlur(true, true);
            gameManager.FreezePlayer(true, true, false);
            gameManager.ShowPanel(GameManager.PanelType.InventoryPanel);
        }

        /// <summary>
        /// Open the inventory item selection menu.
        /// </summary>
        public void OpenItemSelector(IInventorySelector inventorySelector)
        {
            itemSelector = true;
            this.inventorySelector = inventorySelector;
            gameManager.ShowInventoryPanel(true);
        }

        /// <summary>
        /// Play inventory sound.
        /// </summary>
        public void PlayInventorySound(InventorySound sound)
        {
            if (inventorySounds != null && nextSoundDelay <= 0)
            {
                AudioClip clip = null;

                switch (sound)
                {
                    case InventorySound.ItemSelect:
                        clip = sounds.itemSelectSound;
                        break;
                    case InventorySound.ItemMove:
                        clip = sounds.itemMoveSound;
                        break;
                    case InventorySound.ItemPut:
                        clip = sounds.itemPutSound;
                        break;
                    case InventorySound.ItemError:
                        clip = sounds.itemErrorSound;
                        break;
                }

                if (clip != null)
                {
                    inventorySounds.PlayOneShot(clip, sounds.volume);
                    nextSoundDelay = sounds.nextSoundDelay;
                }
            }
        }

        /// <summary>
        /// Check if the item is in inventory.
        /// </summary>
        /// <param name="guid">Unique ID of the item.</param>
        /// <returns>Status whether the item is in inventory.</returns>
        public bool ContainsItem(string guid, out OccupyData occupyData)
        {
            if (carryingItems.Count > 0)
            {
                foreach (var item in carryingItems)
                {
                    if (item.Key.ItemGuid == guid)
                    {
                        occupyData = new OccupyData()
                        {
                            inventoryItem = item.Key,
                            occupiedSlots = item.Value
                        };
                        return true;
                    }
                }
            }

            occupyData = new OccupyData();
            return false;
        }

        /// <summary>
        /// Check if the item is in inventory.
        /// </summary>
        /// <param name="guid">Unique ID of the item.</param>
        /// <returns>Status whether the item is in inventory.</returns>
        public bool ContainsItem(string guid)
        {
            if (carryingItems.Count > 0)
            {
                foreach (var item in carryingItems)
                {
                    if (item.Key.ItemGuid == guid)
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Check if the item coordinates are in the container view.
        /// </summary>
        /// <param name="isContainer">Result if the coordinates of the item are in the container view.</param>
        /// <returns>Result if the coordinates are not overflowned.</returns>
        public bool IsContainerCoords(int x, int y, out bool isContainer)
        {
            if (y >= 0)
            {
                if (x >= 0 && x < currentContainer.Columns && y < currentContainer.Rows)
                {
                    isContainer = true;
                    return true;
                }
                else
                {
                    int X = x - currentContainer.Columns;
                    if (X >= 0 && X < settings.columns && y < SlotRows)
                    {
                        isContainer = false;
                        return true;
                    }
                }
            }

            isContainer = false;
            return false;
        }

        /// <summary>
        /// Check if the item coordinates are not overflowed.
        /// </summary>
        public bool IsCoordsOverflowed(int x, int y)
        {
            if (x < 0 || y < 0) 
                return true;

            if (ContainerOpened)
            {
                if (x > currentContainer.Columns)
                {
                    return x - currentContainer.Columns > settings.columns || y > SlotRows;
                }

                return x > currentContainer.Columns || y > currentContainer.Rows;
            }

            return x > SlotColumns || y > MaxSlotRows;
        }

        #region Events
        public void ShowContextMenu(bool show, InventoryItem invItem = null)
        {
            if (!ContainerOpened && show && invItem != null)
            {
                activeItem = invItem;
                Item item = invItem.Item;

                Vector3[] itemCorners = new Vector3[4];
                invItem.GetComponent<RectTransform>().GetWorldCorners(itemCorners);

                if (invItem.orientation == Orientation.Horizontal)
                    contextMenu.contextMenu.transform.position = itemCorners[2];
                else if (invItem.orientation == Orientation.Vertical)
                    contextMenu.contextMenu.transform.position = itemCorners[1];

                bool use = item.Settings.isUsable || itemSelector;
                contextMenu.contextUse.gameObject.SetActive(use);
                bool examine = item.Settings.isExaminable && !itemSelector;
                contextMenu.contextExamine.gameObject.SetActive(examine);
                bool combine = item.Settings.isCombinable && !invItem.isContainerItem && !itemSelector;
                contextMenu.contextCombine.gameObject.SetActive(combine);
                bool shortcut = item.Settings.canBindShortcut && !invItem.isContainerItem && !itemSelector;
                contextMenu.contextShortcut.gameObject.SetActive(shortcut);
                bool drop = item.Settings.isDroppable;
                contextMenu.contextDrop.gameObject.SetActive(drop);
                bool discard = item.Settings.isDiscardable;
                contextMenu.contextDiscard.gameObject.SetActive(discard);

                contextMenu.contextMenu.SetActive(true);
                contextMenu.blockerPanel.SetActive(true);
                contextShown = true;
            }
            else
            {
                contextMenu.contextMenu.SetActive(false);
                contextMenu.blockerPanel.SetActive(false);
                contextMenu.contextUse.gameObject.SetActive(false);
                contextMenu.contextExamine.gameObject.SetActive(false);
                contextMenu.contextCombine.gameObject.SetActive(false);
                contextMenu.contextShortcut.gameObject.SetActive(false);
                contextMenu.contextDrop.gameObject.SetActive(false);
                contextMenu.contextDiscard.gameObject.SetActive(false);
                contextShown = false;
            }
        }

        public void ShowItemInfo(string guid)
        {
            Item item = items[guid];
            string descriptionColor = ColorUtility.ToHtmlStringRGB(itemInfo.descriptionFontColor);

            itemInfo.infoText.text = $"<size={itemInfo.titleFontSize}>{item.Title}</size>" +
                $"\n<size={itemInfo.descriptionFontSize}><color=#{descriptionColor}>{item.Description}</color></size>";

            itemInfo.infoPanel.SetActive(true);
        }

        public void HideItemInfo()
        {
            if (!contextShown) itemInfo.infoPanel.SetActive(false);
        }

        public void OnBlockerClicked()
        {
            ShowContextMenu(false);

            if (bindShortcut)
            {
                bindShortcut = false;
                activeItem = null;
            }
        }

        public void OnCloseInventory()
        {
            if (ContainerOpened)
            {
                foreach (var item in containerItems)
                {
                    Destroy(item.Key.gameObject);
                }

                if (containerSlots != null)
                {
                    foreach (var slot in containerSlots)
                    {
                        Destroy(slot.gameObject);
                    }
                }

                containerSlots = new InventorySlot[0, 0];
                containerItems.Clear();

                containerSettings.containerObject.gameObject.SetActive(false);
                currentContainer.OnStorageClose();
                currentContainer = null;
            }

            foreach (var item in carryingItems)
            {
                item.Key.OnCloseInventory();
            }

            itemSelector = false;
            bindShortcut = false;

            inventorySelector = null;
            activeItem = null;

            ShowContextMenu(false);
            HideItemInfo();
        }
        #endregion

        private InventoryItem CreateItem(string guid, ushort quantity, ItemCustomData customData)
        {
            Item item = items[guid];

            if (CheckSpace(item.Width, item.Height, out FreeSpace space))
            {
                InventoryItem inventoryItem = CreateItem(new ItemCreationData()
                {
                    itemGuid = guid,
                    quantity = quantity,
                    orientation = space.orientation,
                    coords = new Vector2Int(space.x, space.y),
                    customData = customData,
                    parent = itemsTransform,
                    slotsSpace = slots
                });

                AddItemToFreeSpace(space, inventoryItem);
                return inventoryItem;
            }

            return null;
        }

        private InventoryItem CreateItem(ItemCreationData itemCreationData)
        {
            Item item = items[itemCreationData.itemGuid];

            GameObject itemGO = Instantiate(slotSettings.slotItemPrefab, itemCreationData.parent);
            RectTransform rect = itemGO.GetComponent<RectTransform>();
            InventoryItem inventoryItem = itemGO.GetComponent<InventoryItem>();

            if (itemCreationData.orientation == Orientation.Vertical)
                rect.localEulerAngles = new Vector3(0, 0, -90);

            float width = settings.cellSize * item.Width;
            width += item.Width > 1 ? settings.spacing * (item.Width - 1) : 0;

            float height = settings.cellSize * item.Height;
            height += item.Height > 1 ? settings.spacing * (item.Height - 1) : 0;

            rect.sizeDelta = new Vector2(width, height);
            rect.localScale = Vector3.one;

            RectTransform slot = itemCreationData.slotsSpace[itemCreationData.coords.y, itemCreationData.coords.x].GetComponent<RectTransform>();
            Vector2 offset = inventoryItem.GetOrientationOffset();
            Vector2 position = new Vector2(slot.localPosition.x, slot.localPosition.y) + offset;
            rect.anchoredPosition = position;

            inventoryItem.SetItem(this, new InventoryItem.ItemData()
            {
                guid = itemCreationData.itemGuid,
                item = item,
                quantity = itemCreationData.quantity,
                orientation = itemCreationData.orientation,
                customData = itemCreationData.customData,
                slotSpace = itemCreationData.coords
            });

            return inventoryItem;
        }

        private void AddItemToFreeSpace(FreeSpace space, InventoryItem inventoryItem)
        {
            Item item = inventoryItem.Item;
            int maxY = item.Height, maxX = item.Width;

            if (space.orientation == Orientation.Vertical)
            {
                maxY = item.Width;
                maxX = item.Height;
            }

            InventorySlot[] occupiedSlots = new InventorySlot[maxY * maxX];
            int slotIndex = 0;

            for (int y = space.y; y < space.y + maxY; y++)
            {
                for (int x = space.x; x < space.x + maxX; x++)
                {
                    InventorySlot slot = slots[y, x];
                    slot.itemInSlot = inventoryItem;
                    occupiedSlots[slotIndex++] = slot;
                }
            }

            carryingItems.Add(inventoryItem, occupiedSlots);
        }

        private bool CheckSpace(ushort width, ushort height, out FreeSpace slotSpace)
        {
            for (int y = 0; y < SlotRows; y++)
            {
                for (int x = 0; x < SlotColumns; x++)
                {
                    if (width == height)
                    {
                        if (CheckSpaceFromPosition(x, y, width, height))
                        {
                            slotSpace = new FreeSpace(x, y, Orientation.Horizontal);
                            return true;
                        }
                    }
                    else
                    {
                        if (CheckSpaceFromPosition(x, y, width, height))
                        {
                            slotSpace = new FreeSpace(x, y, Orientation.Horizontal);
                            return true;
                        }
                        else if (CheckSpaceFromPosition(x, y, height, width))
                        {
                            slotSpace = new FreeSpace(x, y, Orientation.Vertical);
                            return true;
                        }
                    }
                }
            }

            slotSpace = new FreeSpace();
            return false;
        }

        public StorableCollection OnCustomSave()
        {
            StorableCollection saveableBuffer = new StorableCollection();
            StorableCollection itemsToSave = new StorableCollection();
            StorableCollection shortcutsSave = new StorableCollection();

            int index = 0;
            foreach (var item in carryingItems)
            {
                itemsToSave.Add("item_" + index++, new StorableCollection()
                {
                    { "item", item.Key.ItemGuid },
                    { "quantity", item.Key.Quantity },
                    { "orientation", item.Key.orientation },
                    { "position", item.Key.Position.ToSaveable() },
                    { "customData", item.Key.CustomData.GetJson() },
                });
            }

            shortcutsSave.Add("shortcut_0", shortcuts[0].item != null ? shortcuts[0].item.ItemGuid : "{}");
            shortcutsSave.Add("shortcut_1", shortcuts[1].item != null ? shortcuts[1].item.ItemGuid : "{}");
            shortcutsSave.Add("shortcut_2", shortcuts[2].item != null ? shortcuts[2].item.ItemGuid : "{}");
            shortcutsSave.Add("shortcut_3", shortcuts[3].item != null ? shortcuts[3].item.ItemGuid : "{}");

            saveableBuffer.Add("expanded", expandedRows);
            saveableBuffer.Add("items", itemsToSave);
            saveableBuffer.Add("shortcuts", shortcutsSave);
            return saveableBuffer;
        }

        public void OnCustomLoad(JToken data)
        {
            int expandedRowsCount = (int)data["expanded"];
            if (expandedRowsCount > 0) ExpandRows((ushort)expandedRowsCount);

            JObject items = (JObject)data["items"];

            foreach (var itemProp in items.Properties())
            {
                JToken token = itemProp.Value;

                string itemGuid = token["item"].ToString();
                int quantity = (int)token["quantity"];
                Orientation orientation = (Orientation)(int)token["orientation"];
                Vector2Int position = token["position"].ToObject<Vector2Int>();
                ItemCustomData customData = new ItemCustomData()
                {
                    JsonData = token["customData"].ToString()
                };

                InventoryItem inventoryItem = CreateItem(new ItemCreationData()
                {
                    itemGuid = itemGuid,
                    quantity = (ushort)quantity,
                    orientation = orientation,
                    coords = position,
                    customData = customData,
                    parent = itemsTransform,
                    slotsSpace = slots
                });

                AddItemToFreeSpace(new FreeSpace()
                {
                    x = position.x,
                    y = position.y,
                    orientation = orientation
                }, inventoryItem);
            }

            LoadShortcut(0, data["shortcuts"]["shortcut_0"].ToString());
            LoadShortcut(1, data["shortcuts"]["shortcut_1"].ToString());
            LoadShortcut(2, data["shortcuts"]["shortcut_2"].ToString());
            LoadShortcut(3, data["shortcuts"]["shortcut_3"].ToString());
        }

        private void LoadShortcut(int index, string itemGuid)
        {
            if (string.IsNullOrEmpty(itemGuid)) return;
            InventoryItem inventoryItem = GetInventoryItem(itemGuid);
            if(inventoryItem != null) SetShortcut(index, inventoryItem);
        }
    }
}