using System;

namespace UHFPS.Runtime 
{
    [Serializable]
    public class ItemProperty
    {
        public string GUID;

        public static implicit operator string(ItemProperty item)
        {
            return item.GUID;
        }

        public bool HasItem
        {
            get => !string.IsNullOrEmpty(GUID)
                   && Inventory.HasReference
                   && Inventory.Instance.items.ContainsKey(GUID);
        }

        /// <summary>
        /// Get Item from Inventory (Runtime).
        /// </summary>
        public Item GetItem()
        {
            if (Inventory.HasReference)
            {
                if(Inventory.Instance.items.TryGetValue(GUID, out Item item))
                    return item;
            }

            return new Item();
        }

        /// <summary>
        /// Get Item from Inventory Asset.
        /// </summary>
        public Item GetItemRaw()
        {
            if (Inventory.HasReference && Inventory.Instance.inventoryAsset != null)
            {
                foreach (var item in Inventory.Instance.inventoryAsset.Items)
                {
                    if (item.guid == GUID) 
                        return item.item;
                }
            }

            return new Item();
        }
    }
}