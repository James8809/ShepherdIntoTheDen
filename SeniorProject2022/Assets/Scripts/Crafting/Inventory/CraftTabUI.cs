using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using UnityEngine;
using Crafting;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class CraftTabUI : MonoBehaviour
{
    [Header("Drag GameObjects")]
    private InventoryManager inventoryManager;

    public GameObject craftableItemContainer;
    public Transform craftableDescriptionContainer;
    public Transform itemRequirementsContainer;
    public Button craftButton;
    public TextMeshProUGUI noItemsText;
    
    [Header("Prefabs")]
    public GameObject craftableItemTab;
    public GameObject itemRequirement;

    [Header("Requirements Icon")] 
    public Sprite checkmarkIcon;
    public Sprite xIcon;

    [Header("Debug")] public bool startWithAllItems = false;

    [HideInInspector] public Action OnRefresh;
    
    // Private Variables
    private CraftableItem selectedItem;

    void Awake()
    {        
        inventoryManager = FindObjectOfType<InventoryManager>(true);
        InitializeCraft();
    }

    private void InitializeCraft()
    {
        var items = Resources.LoadAll<CraftableItem>("Items/Consumables");
        selectedItem = items[0];
        RefreshCraftTab();
    }

    public void RefreshCraftTab()
    {
        if (!inventoryManager)
            inventoryManager = FindObjectOfType<InventoryManager>(true);

        CraftableItem[] items;
        if (startWithAllItems)
        {
            var consumables = Resources.LoadAll<CraftableItem>("Items/Consumables");
            var crests = Resources.LoadAll<CraftableItem>("Items/Crests");
            items = consumables.Concat(crests).ToArray();
        }
        else
            items = inventoryManager.GetCraftableItems()?.ToArray();
        bool hasCraftableItems = items.Length > 0;
        noItemsText.gameObject.SetActive(!hasCraftableItems);
        craftableDescriptionContainer.gameObject.SetActive(hasCraftableItems);
        if (items.Length == 0)
            return;
        selectedItem = items[0];
        FillCraftableItemContainer(items);
        SetDescriptionToItem(selectedItem);
    }

    private void FillCraftableItemContainer(CraftableItem[] items)
    {
        foreach (Transform child in craftableItemContainer.transform) {
            Destroy(child.gameObject);
        }
        foreach (CraftableItem i in items)
        {
            var craftableItemTabContainer = Instantiate(craftableItemTab, craftableItemContainer.transform);
            craftableItemTabContainer.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text = i.itemName;
            craftableItemTabContainer.transform.Find("ItemImage").GetComponent<Image>().sprite = i.icon;
            craftableItemTabContainer.GetComponent<Button>().onClick.AddListener(() => OnItemClicked(i));
        }
    }

    private void OnItemClicked(CraftableItem i)
    {
        selectedItem = i;
        SetDescriptionToItem(i);
    }

    private void SetDescriptionToItem(CraftableItem item)
    {
        // UI Description
        var itemInformationUI = craftableDescriptionContainer.Find("ItemInformation").transform;
        var itemNameUI = itemInformationUI.Find("ItemName").GetComponent<TextMeshProUGUI>();
        var itemImageUI = itemInformationUI.Find("DescriptionImage").GetComponent<Image>();
        var itemDescriptionUI = itemInformationUI.Find("ItemDescription").GetComponent<TextMeshProUGUI>();
        var itemTypeUI = itemInformationUI.Find("ItemType").GetComponent<TextMeshProUGUI>();
        
        itemNameUI.text = item.itemName;
        itemImageUI.sprite = item.icon;
        itemDescriptionUI.text = item.description;
        itemTypeUI.text = item is Resource ? "Resource" : item is Consumable ? "Consumable" : "Gear";
        craftableDescriptionContainer.GetComponent<DOTweenAnimation>().DORestart();

        FillRequirementInfo(item);
    }

    private void FillRequirementInfo(CraftableItem item)
    {
        // Clear itemRequirementsContainer
        foreach (Transform child in itemRequirementsContainer.transform) {
            Destroy(child.gameObject);
        }
        
        bool canCraft = true;
        foreach (CraftItem i in item.CraftRecipe)
        {
            var itemRequirementContainer = Instantiate(itemRequirement, itemRequirementsContainer);
            var requiredItemNameUI = itemRequirementContainer.transform.Find("RequiredItemName").GetComponent<TextMeshProUGUI>();
            var requiredItemAmountUI = itemRequirementContainer.transform.Find("RequiredItemAmount").GetComponent<TextMeshProUGUI>();
            var ownedAmountUI = itemRequirementContainer.transform.Find("OwnedAmount").GetComponent<TextMeshProUGUI>();
            var requiredItemIconUI = itemRequirementContainer.transform.Find("ItemIcon").GetComponent<Image>();
            var itemStatusUI = itemRequirementContainer.transform.Find("ItemStatus").GetComponent<Image>();

            requiredItemNameUI.text = i.item.itemName;
            requiredItemAmountUI.text = "x" + i.amount;
            requiredItemIconUI.sprite = i.item.icon;
            ownedAmountUI.text = "x" + inventoryManager.GetItemAmountOwned(i.item);
            bool hasResource = inventoryManager.IsItemInInventory(i.item, i.amount);
            itemStatusUI.sprite = hasResource ? checkmarkIcon : xIcon;
            canCraft &= hasResource;
        }
        
                    
        // if crest and already in inventory, can't craft
        // TODO: Make text that says it is already in inventory
        if (item is Crest)
            canCraft &= !inventoryManager.IsItemInInventory(item, 1);

        // Enable Craft button if craftable
        craftButton.gameObject.SetActive(canCraft);
        craftButton.onClick.RemoveAllListeners();
        craftButton.onClick.AddListener(() => CraftItem(item));
    }

    public void RefreshCraftTabRequirements()
    {
        if (selectedItem)
            FillRequirementInfo(selectedItem);
    }

    private void CraftItem(CraftableItem item)
    {
        inventoryManager.CraftItem(item);
        NotificationManager.Instance.CreateNotification(item);
        OnRefresh?.Invoke();
    }
}
