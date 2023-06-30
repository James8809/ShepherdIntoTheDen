using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crafting {
    public class Item : ScriptableObject
    {
        public string itemName;
        [Tooltip("Icon that shows up in the inventory.")]
        public Sprite icon;
        [TextArea(10, 100)]
        public string description;
    }
    
    [System.Serializable]
    public class CraftItem
    {
        public Item item;
        public int amount;
    }
    
    public class CraftableItem : Item
    {
        [Tooltip("Requirements in order to craft this item.")]
        public List<CraftItem> CraftRecipe;
    }
}
