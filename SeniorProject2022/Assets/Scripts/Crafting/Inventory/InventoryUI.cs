
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [Header("Game Objects")] public GameObject inventoryContainer;
    public GameObject craftContainer;
    public GameObject crestsContainer;
    public GameObject abilitiesContainer;
    public EventSystem eventSystem;

    [Header("Tabs")] public GameObject inventoryTab;
    public GameObject craftTab;
    public GameObject crestsTab;
    public GameObject abilitiesTab;

    private InventoryTabUI inventoryTabUI;
    private CraftTabUI craftTabUI;
    private LoadoutUI loadoutUI;
    private CrestsUI crestsUI;
    private AbilitiesUI abilitiesUI;


    private bool inventoryEnabled = false;

    // Selected Tab Color
    private ColorBlock selectedColor;
    private ColorBlock unselectedColor;

    private void Awake()
    {
        var colorBlock = inventoryTab.GetComponent<Button>().colors;
        colorBlock.normalColor = Color.white;
        selectedColor = colorBlock;
        colorBlock.normalColor = new Color(0.44f, 0.44f, 0.44f);
        unselectedColor = colorBlock;


        // select inventory first
        inventoryTab.GetComponent<Button>().colors = selectedColor;

        // Initialize Inventory First
        inventoryTabUI = inventoryContainer.GetComponent<InventoryTabUI>();
        Debug.Log("inventoryTabUI");
        craftTabUI = craftContainer.GetComponent<CraftTabUI>();
        loadoutUI = FindObjectOfType<LoadoutUI>(true);
        crestsUI = FindObjectOfType<CrestsUI>(true);
        abilitiesUI = FindObjectOfType<AbilitiesUI>(true);

        // Actions
        craftTabUI.OnRefresh += () => RefreshAllTabs();
    }

    private void OnEnable()
    {
        RefreshAllTabs();
    }

    public void SwitchToTab(string tabType) // Inventory, Craft, or Crests
    {
        DisableAllTabs();
        var containerToActivate = tabType == "Inventory" ? inventoryContainer :
            tabType == "Craft" ? craftContainer :
            tabType == "Abilities" ? abilitiesContainer: crestsContainer;
        var tabToActivate = tabType == "Inventory" ? inventoryTab :
            tabType == "Craft" ? craftTab : 
            tabType == "Abilities" ? abilitiesTab: crestsTab;

        FMODUnity.RuntimeManager.PlayOneShot("UI_Open");
        tabToActivate.GetComponent<Button>().colors = selectedColor;
        SetActiveWithTransition(containerToActivate, true);
        RefreshAllTabs();
    }

    private void DisableAllTabs()
    {
        // Disable Button color
        inventoryTab.GetComponent<Button>().colors = unselectedColor;
        craftTab.GetComponent<Button>().colors = unselectedColor;
        crestsTab.GetComponent<Button>().colors = unselectedColor;
        abilitiesTab.GetComponent<Button>().colors = unselectedColor;

        SetActiveWithTransition(inventoryContainer, false);
        SetActiveWithTransition(craftContainer, false);
        SetActiveWithTransition(crestsContainer, false);
        SetActiveWithTransition(abilitiesContainer, false);
    }

    private void RefreshAllTabs()
    {
        inventoryTabUI.RefreshInventoryTab(false);
        craftTabUI.RefreshCraftTabRequirements();
        loadoutUI.RefreshAllItemNumbers();
        crestsUI.RefreshCrests();
        craftTabUI.RefreshCraftTab();
        abilitiesUI.RefreshAbilitiesTab();
    }

    /* ----- Toggle UI ----- */
    // returns if inventory is on (true) or off (false)
    public bool ToggleInventory()
    {
        inventoryEnabled = !inventoryEnabled;
        SetActiveWithTransition(gameObject, inventoryEnabled);
        inventoryTabUI.RefreshInventoryTab(false);
        return inventoryEnabled;
    }

    public void SetActiveWithTransition(GameObject obj, bool enabled)
    {
        if (enabled)
        {
            obj.SetActive(true);
            return;
        }
        if (!obj.activeSelf)
            return;
        obj.GetComponent<CanvasGroup>().DOFade(0, 0.2f).OnComplete(()=> obj.SetActive(false));
        
        /*
        var animations = GetComponents<DOTweenAnimation>();
        foreach (var anim in animations)
        { 
            if (enabled)
            {
                anim.onComplete = new UnityEvent();
            }
            else {
                var e = new UnityEvent();
                e.RemoveAllListeners();
                e.AddListener(() => obj.SetActive(false));
                anim.DOPlayBackwards();
            }
        }
        */
    }

}
