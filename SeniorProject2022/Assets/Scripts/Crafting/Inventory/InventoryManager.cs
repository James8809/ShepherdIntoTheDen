    using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Crafting;

public class InventoryManager : MonoBehaviour
{
    private Dictionary<Item, int> items = new Dictionary<Item, int>();
    private List<CraftableItem> craftableItems = new List<CraftableItem>();
    [Header("Debug")] public CraftItem[] itemsAddOnStart;

    private void Awake()
    {
        foreach (var item in itemsAddOnStart)
        {
            AddItem(item.item, item.amount);
        }
        Debug.Log("inventory manager awake");
    }

    #region Inventory Essentials
    public Dictionary<Resource, int> GetResources()
    {
        return items.Where(i => i.Key is Resource)
            .ToDictionary(i => i.Key as Resource, i => i.Value);
    }

    public Dictionary<Consumable, int> GetConsumables()
    {
        return items.Where(i => i.Key is Consumable)
            .ToDictionary(i => i.Key as Consumable, i => i.Value);
    }

    public Dictionary<Crest, int> GetCrests()
    {
        return items.Where(i => i.Key is Crest)
            .ToDictionary(i => i.Key as Crest, i => i.Value);
    }

    public List<CraftableItem> GetCraftableItems()
    {
        return craftableItems;
    }

    public Dictionary<Item, int> GetAllItems()
    {
        return items;
    }

    public void AddItem(Item item, int quantity)
    {
        if (items.TryGetValue(item, out var q))
            items[item] = Mathf.Max(q + quantity, 0);
        else 
            items.Add(item, Mathf.Max(quantity, 0));
    }
    
    public void RemoveItem(Item item)
    {
        AddItem(item, -1000);
    }

    public void ClearInventory()
    {
        items = new Dictionary<Item, int>();
    }

    public bool IsItemInInventory(Item i, int amount)
    {
        return items.ContainsKey(i) && items[i] >= amount;
    }

    public int GetItemAmountOwned(Item i)
    {
        if (items.ContainsKey(i)) return items[i];
        return 0;
    }
    
    public void AddRecipeToCraftableItems(CraftableItem item)
    {
        if (craftableItems.Contains(item)) return;
        craftableItems.Add(item);
    }

    // TODO: Error checking.
    public void CraftItem(CraftableItem item)
    {
        if (!craftableItems.Contains(item)) return; // don't have recipe
        Debug.Log("Crafting " + item.itemName);
        foreach (CraftItem i in item.CraftRecipe)
        {
            AddItem(i.item, i.amount * -1);
        }
        AddItem(item, 1);
    }

    // Probably move to inventory ui manager
    public bool CanCraftItem(CraftableItem item)
    {
        var recipe = item.CraftRecipe;
        foreach (var craftItem in recipe)
        {
            if (!items.TryGetValue(craftItem.item, out var quantity) && quantity > craftItem.amount)
                return false;
        }
        return true;
    }
    
    public void ConsumeItem(Consumable consumable)
    {
        if (GetItemAmountOwned(consumable) <= 0)
            return;
        consumable.Consume(PlayerController.Instance);
        AddItem(consumable, -1);
    }
    
    #endregion
    

    // Loadout Logic
    
}

