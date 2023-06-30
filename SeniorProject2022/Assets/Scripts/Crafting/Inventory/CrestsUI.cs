using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Crafting;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class CrestsUI : MonoBehaviour
{
    [Header("Drag GameObjects")] 
    public GameObject crestItemContainer;
    public Transform crestDescriptionContainer;
    public GameObject equipButton;
    public TextMeshProUGUI noItemsText;
    
    [Header("Crests Prefabs")]
    public GameObject crestItemTab;

    private InventoryManager inventoryManager;
    private CrestManager crestManager;

    private Crest selectedItem;

    private void Awake()
    {
        inventoryManager = FindObjectOfType<InventoryManager>(true);
        crestManager = FindObjectOfType<CrestManager>(true);
        InitializeCrests();
    }

    private void InitializeCrests()
    {
        RefreshCrests();
    }

    public void RefreshCrests()
    {
        ClearCraftItemContainer();
        if (inventoryManager == null) {
            inventoryManager = FindObjectOfType<InventoryManager>(true);
            crestManager = FindObjectOfType<CrestManager>(true);
        }
        var crests = inventoryManager.GetCrests();
        bool hasCrests = crests.Count > 0;
        
        crestItemContainer.SetActive(hasCrests);
        crestDescriptionContainer.gameObject.SetActive(hasCrests);
        noItemsText.gameObject.SetActive(!hasCrests);

        if (!hasCrests) return;
        selectedItem = crests.Keys.ToArray()[0];
        FillCrestItemContainer(crests.Keys.ToArray());
        SetDescriptionToItem(selectedItem);
    }

    private void FillCrestItemContainer(Crest[] crests)
    {
        foreach (Crest i in crests)
        {
            var itemTabContainer = Instantiate(crestItemTab, crestItemContainer.transform);
            itemTabContainer.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text = i.itemName;
            itemTabContainer.transform.Find("ItemImage").GetComponent<Image>().sprite = i.icon;
            itemTabContainer.GetComponent<Button>().onClick.AddListener(() => OnItemClicked(i));
        }
    }
    
    private void OnItemClicked(Crest i)
    {
        selectedItem = i;
        SetDescriptionToItem(i);
    }
    
    private void SetDescriptionToItem(Crest item)
    {
        // UI Description
        var itemInformationUI = crestDescriptionContainer.Find("ItemInformation").transform;
        var itemNameUI = itemInformationUI.Find("ItemName").GetComponent<TextMeshProUGUI>();
        var itemImageUI = itemInformationUI.Find("DescriptionImage").GetComponent<Image>();
        var itemDescriptionUI = itemInformationUI.Find("ItemDescription").GetComponent<TextMeshProUGUI>();
        var itemTypeUI = itemInformationUI.Find("ItemType").GetComponent<TextMeshProUGUI>();
        
        itemNameUI.text = item.itemName;
        itemImageUI.sprite = item.icon;
        itemDescriptionUI.text = item.description;
        itemTypeUI.text = "Gear";
        crestDescriptionContainer.GetComponent<DOTweenAnimation>().DORestart();
        
        // Equip Button
        var isItemEquipped = crestManager.IsAbilityEnabled(item.ability);
        equipButton.GetComponent<Button>().onClick.RemoveAllListeners();
        equipButton.GetComponent<Button>().onClick.AddListener(() => OnEquipButton(item, !isItemEquipped));
        equipButton.GetComponentInChildren<TextMeshProUGUI>().text = isItemEquipped ? "Unequip" : "Equip";
    }

    private void OnEquipButton(Crest crest, bool enabled)
    {
        crestManager.EnableAbility(crest.ability, enabled);
        //crestManager.EquipCrest(crest, enabled);
        if(enabled)
        {
            Debug.Log("enable ability" + crest + ", " + crest.ability);
            FindObjectOfType<AbilitiesUI>(true).AddAbilityItemTab(crest);
        }
        else
        {
            Debug.Log("disable ability");
            FindObjectOfType<AbilitiesUI>(true).DeactivateAbilityItemTab(crest);
        }
        SetDescriptionToItem(selectedItem);
    }

    
    private void ClearCraftItemContainer()
    {
        foreach (Transform child in crestItemContainer.transform) {
            Destroy(child.gameObject);
        }
    }
}
