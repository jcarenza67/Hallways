using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UHFPS.Tools;
using Newtonsoft.Json.Linq;

namespace UHFPS.Runtime
{
    public abstract class InventoryContainer : MonoBehaviour, ISaveable
    {
        public struct ContainerItem
        {
            public string ItemGuid;
            public Item Item;
            public int Quantity;
            public Orientation Orientation;
            public ItemCustomData CustomData;
            public Vector2Int Coords;
        }

        public Dictionary<string, ContainerItem> ContainerItems = new();

        public string ContainerTitle;
        [Range(2, 10)] public ushort Rows = 5;
        [Range(2, 10)] public ushort Columns = 5;

        protected Inventory inventory;

        public virtual void Awake()
        {
            inventory = Inventory.Instance;
        }

        public virtual void Store(InventoryItem inventoryItem, Vector2Int coords)
        {
            string containerGuid = GameTools.GetGuid();
            ContainerItems.Add(containerGuid, new ContainerItem()
            {
                ItemGuid = inventoryItem.ItemGuid,
                Item = inventoryItem.Item,
                Quantity = inventoryItem.Quantity,
                Orientation = inventoryItem.orientation,
                CustomData = inventoryItem.CustomData,
                Coords = coords,
            });

            inventoryItem.ContainerGuid = containerGuid;
        }

        public virtual void Remove(InventoryItem inventoryItem)
        {
            if (ContainerItems.ContainsKey(inventoryItem.ContainerGuid))
            {
                ContainerItems.Remove(inventoryItem.ContainerGuid);
            }
        }

        public virtual void Move(InventoryItem inventoryItem, Inventory.FreeSpace freeSpace)
        {
            if (ContainerItems.TryGetValue(inventoryItem.ContainerGuid, out ContainerItem item))
            {
                item.Coords = new Vector2Int(freeSpace.x, freeSpace.y);
                item.Orientation = freeSpace.orientation;
            }
        }

        public virtual void OnStorageClose() { }

        public bool Contains(string itemGuid) => ContainerItems.Values.Any(x => x.ItemGuid == itemGuid);

        public int Count() => ContainerItems.Count;

        public virtual StorableCollection OnSave()
        {
            StorableCollection saveableBuffer = new StorableCollection();

            foreach (var item in ContainerItems)
            {
                saveableBuffer.Add(item.Key, new StorableCollection()
                {
                    { "item", item.Value.ItemGuid },
                    { "quantity", item.Value.Quantity },
                    { "orientation", item.Value.Orientation },
                    { "position", item.Value.Coords.ToSaveable() },
                    { "customData", item.Value.CustomData.GetJson() },
                });
            }

            return saveableBuffer;
        }

        public virtual void OnLoad(JToken data)
        {
            JObject savedItems = (JObject)data;
            if (savedItems == null) return;

            foreach (var itemProp in savedItems.Properties())
            {
                JToken token = itemProp.Value;

                string containerGuid = itemProp.Name;
                string itemGuid = token["item"].ToString();
                Item item = inventory.items[itemGuid];
                int quantity = (int)token["quantity"];
                Orientation orientation = (Orientation)(int)token["orientation"];
                Vector2Int position = token["position"].ToObject<Vector2Int>();
                ItemCustomData customData = new ItemCustomData()
                {
                    JsonData = token["customData"].ToString()
                };

                ContainerItems.Add(containerGuid, new ContainerItem()
                {
                     ItemGuid = itemGuid,
                     Item = item,
                     Quantity = quantity,
                     Orientation = orientation,
                     CustomData = customData,
                     Coords = new Vector2Int(position.x, position.y)
                });
            }
        }
    }
}