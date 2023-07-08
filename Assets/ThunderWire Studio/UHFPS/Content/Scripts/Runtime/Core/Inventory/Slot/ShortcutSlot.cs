using UnityEngine;
using UnityEngine.UI;
using UHFPS.Tools;
using TMPro;

namespace UHFPS.Runtime
{
    public class ShortcutSlot : MonoBehaviour
    {
        public GameObject itemPanel;

        [Header("References")]
        public CanvasGroup fadePanel;
        public Image background;
        public Image frame;
        public Image itemIcon;
        public TMP_Text quantity;

        private InventoryItem inventoryItem;
        private Inventory inventory;

        public void SetItem(InventoryItem inventoryItem)
        {
            this.inventoryItem = inventoryItem;

            if(inventoryItem != null)
            {
                inventory = inventoryItem.Inventory;
                Item item = inventoryItem.Item;

                // icon orientation and scaling
                Vector2 slotSize = itemIcon.rectTransform.rect.size;
                slotSize -= new Vector2(10, 10);
                Vector2 iconSize = item.Icon.rect.size;

                Vector2 scaleRatio = slotSize / iconSize;
                float scaleFactor = Mathf.Min(scaleRatio.x, scaleRatio.y);

                itemIcon.sprite = item.Icon;
                itemIcon.rectTransform.sizeDelta = iconSize * scaleFactor;
                quantity.text = inventoryItem.Quantity.ToString();

                frame.sprite = inventory.slotSettings.normalSlotFrame;
                background.Alpha(1f);
                fadePanel.alpha = 1f;
                itemPanel.SetActive(true);
            }
            else
            {
                itemIcon.sprite = null;
                quantity.text = string.Empty;

                frame.sprite = inventory.slotSettings.restrictedSlotFrame;
                background.Alpha(0.6f);
                fadePanel.alpha = 0.3f;
                itemPanel.SetActive(false);
            }
        }

        private void Update()
        {
            UpdateItemQuantity();
        }

        private void UpdateItemQuantity()
        {
            if (inventoryItem == null)
                return;

            int itemQuantity = inventoryItem.Quantity;

            if (!inventoryItem.Item.Settings.alwaysShowQuantity)
            {
                if (itemQuantity > 1) 
                    quantity.text = inventoryItem.Quantity.ToString();
                else quantity.text = string.Empty;
            }
            else
            {
                quantity.text = itemQuantity.ToString();
                quantity.color = itemQuantity >= 1
                    ? inventory.slotSettings.normalQuantityColor
                    : inventory.slotSettings.zeroQuantityColor;
            }
        }
    }
}