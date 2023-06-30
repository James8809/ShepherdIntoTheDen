using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crafting;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class InventoryTabUI : MonoBehaviour
{
    [Header("Inventory Prefabs")]
    public GameObject itemIcon;
    public GameObject inventoryItemContainer;
    public GameObject itemUseButton;
    
    private InventoryManager inventoryManager;
    private Item selectedItem;
    private LoadoutUI loadoutUI;


    void Awake()
    {        
        inventoryManager = FindObjectOfType<InventoryManager>(true);
        loadoutUI = FindObjectOfType<LoadoutUI>(true);
        InitializeInventory();
    }
    
    public void InitializeInventory()
    {
        var items = inventoryManager.GetAllItems();
        if (selectedItem)
            selectedItem = items.Keys.ToArray()[0];
        RefreshInventoryTab(true);
    }
    
    /* ----- Inventory Tab ----- */
    public void RefreshInventoryTab(bool selectFirst)
    {
        ClearInventoryItemContainer();
        if (!inventoryManager)
            inventoryManager = FindObjectOfType<InventoryManager>(true);
        var items = inventoryManager.GetAllItems();
        bool hasItems = items.Count > 0;
        if (!hasItems) return;
        FillContainerWithItemIcon(items, inventoryItemContainer);
        if (!selectedItem)
            selectedItem = items.Keys.ToArray()[0];
        ChangeDescriptionToItem(selectedItem);
    }

    public void ChangeDescriptionToItem(Item item)
    {
        var itemInformationContainer = transform.Find("ItemInformation");
        var itemNameUI = itemInformationContainer.Find("ItemName").GetComponent<TextMeshProUGUI>();
        var itemImageUI = itemInformationContainer.Find("DescriptionImage").GetComponent<Image>();
        var itemDescriptionUI = itemInformationContainer.Find("ItemDescription").GetComponent<TextMeshProUGUI>();
        var itemTypeUI = itemInformationContainer.Find("ItemType").GetComponent<TextMeshProUGUI>();
        
        itemNameUI.text = item.itemName;
        itemImageUI.sprite = item.icon;
        itemDescriptionUI.text = item.description;
        itemTypeUI.text = item is Resource ? "Resource" : item is Consumable ? "Consumable" : "Crest";
        itemInformationContainer.GetComponent<DOTweenAnimation>().DORestart();
        
        var itemUseButtonContainer = itemInformationContainer.transform.Find("ItemUseButtonContainer");
        
        // Clear Buttons
        foreach (Transform child in itemUseButtonContainer.transform) {
            Destroy(child.gameObject);
        }

        if (item is Consumable)
        {
            // Consume Item Button
            var popupButton = Instantiate(itemUseButton, itemUseButtonContainer);
            popupButton.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "Consume";
            popupButton.GetComponent<Button>().onClick.AddListener(() => ConsumeItem((Consumable)item));
            
            // Equip Item Button
            if (loadoutUI.IsItemEquiped((Consumable) item)) return;
            var equipButton = Instantiate(itemUseButton, itemUseButtonContainer);
            equipButton.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "Equip";
            equipButton.GetComponent<Button>().onClick.AddListener(() => loadoutUI.EquipLoadoutItem((Consumable)item));
        }
    }

    private void OnItemClicked(Item item, GameObject itemIconContainer)
    {
        selectedItem = item;
        ChangeDescriptionToItem(item);
    }
    
    public void ConsumeItem(Consumable consumable)
    {
        inventoryManager.ConsumeItem(consumable);
        RefreshInventoryTab(false);
    }

    private void FillContainerWithItemIcon(Dictionary<Item, int> dic, GameObject container)
    {
        foreach (var item in dic)
        {
            if (item.Value <= 0) //None of this item
                continue;
            var itemIconContainer  = Instantiate(itemIcon, container.transform);
            itemIconContainer.transform.Find("Image").GetComponent<Image>().sprite = item.Key.icon;
            itemIconContainer.GetComponentInChildren<TextMeshProUGUI>().text = "x" + item.Value;
            itemIconContainer.GetComponent<Button>().onClick.AddListener(() => OnItemClicked(item.Key, itemIconContainer));
            itemIconContainer.SetActive(true);
        }
    }

    private void ClearInventoryItemContainer()
    {
        var itemInformationContainer = transform.Find("ItemInformation");
        var itemUseButtonContainer = itemInformationContainer.transform.Find("ItemUseButtonContainer");
        
        // Clear Buttons
        foreach (Transform child in itemUseButtonContainer.transform) {
            Destroy(child.gameObject);
        }
        foreach (Transform child in inventoryItemContainer.transform) {
            Destroy(child.gameObject);
        }
    }



}
