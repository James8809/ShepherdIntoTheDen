using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Crafting;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class AbilitiesUI : MonoBehaviour
{
    [Header("Drag GameObjects")] 
    public GameObject abilityItemContainer;
    public Transform abilityDescriptionContainer;
    public TextMeshProUGUI noItemsText;
    
    [Header("Crests Prefabs")]
    public GameObject abilityItemTab;

    private InventoryManager inventoryManager;
    private CrestManager crestManager;

    private Crest selectedItem;
    private Dictionary<Crest, GameObject>  itemTabsTable = new Dictionary<Crest, GameObject>();

    private Dictionary<GameObject, bool> itemTabs = new Dictionary<GameObject, bool>();

    private void Awake()
    {
        inventoryManager = FindObjectOfType<InventoryManager>(true);
        crestManager = FindObjectOfType<CrestManager>(true);
    }

    public void RefreshAbilitiesTab()
    {
        //DeactivateAbilitiesContainer();
        if (inventoryManager == null)
            inventoryManager = FindObjectOfType<InventoryManager>(true);
        //var crests = crestManager.GetEquipped();
        bool hasCrestsEquipped = itemTabs.ContainsValue(true);
        // bool hasCrestsEquipped = false;
        // foreach (Transform child in abilityItemContainer.transform)
        // {
        //     if (child.gameObject.activeSelf)
        //     {
        //         hasCrestsEquipped = true;
        //         break;
        //     }
        // }
        
        abilityItemContainer.SetActive(hasCrestsEquipped);
        abilityDescriptionContainer.gameObject.SetActive(hasCrestsEquipped);
        noItemsText.gameObject.SetActive(!hasCrestsEquipped);
        
        // if (hasCrestsEquipped)
        //     CheckCrestItemContainer(crests);
            //FillCrestItemContainer(crests.Keys.ToArray());
    }

    public void AddAbilityItemTab(Crest crest)
    {
        if(crest.abilityObj == null){
            Debug.Log("no ability");
            return;
        }
        if(!itemTabsTable.ContainsKey(crest))
        {
            var itemTabContainer = Instantiate(abilityItemTab, abilityItemContainer.transform);
            itemTabContainer.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text = crest.abilityObj.itemName;
            itemTabContainer.transform.Find("ItemImage").GetComponent<Image>().sprite = crest.abilityObj.icon;
            //itemTabContainer.transform.Find("ItemImage").GetComponent<DragDrop>().abilityName = crest.abilityObj.abilityName;
            itemTabContainer.GetComponent<DragDrop>().abilityName = crest.abilityObj.abilityName;
            itemTabs.Add(itemTabContainer, itemTabContainer.activeSelf);
            itemTabsTable.Add(crest, itemTabContainer);
            itemTabContainer.GetComponent<Button>().onClick.AddListener(() => OnItemClicked(crest));
        }
        else
        {
            itemTabsTable[crest].SetActive(true);
            itemTabs[itemTabsTable[crest]] = true;
        }
    }
    public void DeactivateAbilityItemTab(Crest crest)
    {
        if(crest.abilityObj == null){
            Debug.Log("no ability");
            return;
        }
        var itemTab = itemTabsTable[crest];
        itemTab.GetComponent<DragDrop>().OnUnequip();
        itemTab.SetActive(false);
        itemTabs[itemTabsTable[crest]] = false;
    }
    
    
    private void OnItemClicked(Crest i)
    {
        selectedItem = i;
        SetDescriptionToItem(i);
    }
    
    private void SetDescriptionToItem(Crest item)
    {
        // UI Description
        Debug.Log("setting descritption");
        var itemInformationUI = abilityDescriptionContainer.Find("ItemInformation").transform;
        var itemNameUI = itemInformationUI.Find("ItemName").GetComponent<TextMeshProUGUI>();
        var itemImageUI = itemInformationUI.Find("DescriptionImage").GetComponent<Image>();
        var itemDescriptionUI = itemInformationUI.Find("ItemDescription").GetComponent<TextMeshProUGUI>();
        var itemTypeUI = itemInformationUI.Find("ItemType").GetComponent<TextMeshProUGUI>();
        
        itemNameUI.text = item.abilityObj.itemName;
        itemImageUI.sprite = item.abilityObj.icon;
        itemDescriptionUI.text = item.abilityObj.description;
        itemTypeUI.text = "Abilities";
        abilityDescriptionContainer.GetComponent<DOTweenAnimation>().DORestart();
    }
    
    private void ClearAbilitiesContainer()
    {
        foreach (Transform child in abilityItemContainer.transform) {
            Destroy(child.gameObject);
        }
    }
    private void DeactivateAbilitiesContainer()
    {
        foreach (Transform child in abilityItemContainer.transform) {
            child.gameObject.SetActive(false);
        }
    }

    
    
}
