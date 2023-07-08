using System;
using UnityEngine;

namespace UHFPS.Runtime
{
    public enum ImageOrientation { Normal, Flipped };
    public enum FlipDirection { Left, Right };
    public enum UsableType { PlayerItem, HealthItem }

    [Serializable]
    public sealed class Item
    {
        public string Title;
        public string Description;
        public ushort Width;
        public ushort Height;
        public ImageOrientation Orientation;
        public FlipDirection FlipDirection;
        public Sprite Icon;

        public ObjectReference ItemObject;

        [Serializable]
        public struct ItemSettings 
        {
            public bool isUsable;
            public bool isStackable;
            public bool isExaminable;
            public bool isCombinable;
            public bool isDroppable;
            public bool isDiscardable;
            public bool canBindShortcut;
            public bool alwaysShowQuantity;
        }
        public ItemSettings Settings;

        [Serializable]
        public struct ItemUsableSettings
        {
            public UsableType usableType;
            public int playerItemIndex;
            public uint healthPoints;
        }
        public ItemUsableSettings UsableSettings;

        [Serializable]
        public struct ItemProperties
        {
            public ushort maxStack;
        }
        public ItemProperties Properties;

        [Serializable]
        public struct ItemCombineSettings
        {
            public string combineWithID;
            public string resultCombineID;
            public int playerItemIndex;

            [Tooltip("After combining, do not remove the item from inventory.")]
            public bool keepAfterCombine;
            [Tooltip("After combining, call the combine event if the second inventory item is a player item. The combine event will be called only on the second item.")]
            public bool eventAfterCombine;
            [Tooltip("After combining, select the player item instead of adding the result item to the inventory.")]
            public bool selectAfterCombine;
        }
        public ItemCombineSettings[] CombineSettings;

        [Serializable]
        public struct Localization
        {
            public string titleKey;
            public string descriptionKey;
        }
        public Localization LocalizationSettings;

        /// <summary>
        /// Creates a new instance of a class with the same value as an existing instance.
        /// </summary>
        public Item DeepCopy()
        {
            return new Item()
            {
                Title = Title,
                Description = Description,
                Width = Width,
                Height = Height,
                Orientation = Orientation,
                FlipDirection = FlipDirection,
                Icon = Icon,
                ItemObject = ItemObject,
                Settings = Settings,
                UsableSettings = UsableSettings,
                Properties = Properties,
                CombineSettings = CombineSettings,
                LocalizationSettings = LocalizationSettings
            };
        }
    }
}