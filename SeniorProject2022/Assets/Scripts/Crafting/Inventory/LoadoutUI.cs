using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using TMPro;
using UnityEngine;
using Crafting;
using UnityEngine.UI;

public class LoadoutUI : MonoBehaviour
{
    private const int NUM_LOADOUT_SLOTS = 4;
        
    [Header("Loadout Prefabs")]
    public GameObject loadoutPanel;

    [Header("Drag GameObjects")]
    public GameObject loadoutPanelsContainer;
    public CanvasGroup blackPanel;
    
    // Private
    private GameObject[] loadoutPanels = new GameObject[NUM_LOADOUT_SLOTS];
    private Consumable[] loadoutItems = new Consumable[NUM_LOADOUT_SLOTS];
    private InventoryManager inventoryManager;

    private void Awake()
    {
        InitializeLoadout();
        inventoryManager = FindObjectOfType<InventoryManager>(true);
    }

    private void InitializeLoadout()
    {
        for (int i = 0; i < 2; i++) // change to 2 temporary 
        {
            var loadoutUI = Instantiate(loadoutPanel, loadoutPanelsContainer.transform);
            loadoutUI.transform.Find("KeybindText").GetComponent<TextMeshProUGUI>().text = i + 1 + "";
            loadoutUI.transform.Find("LoadoutImage").GetComponent<Image>().sprite = null;
            loadoutUI.transform.Find("NumItemText").GetComponent<TextMeshProUGUI>().text = "";
            loadoutPanels[i] = loadoutUI;
        }
        var loadoutUI1 = Instantiate(loadoutPanel, loadoutPanelsContainer.transform);
        loadoutUI1.transform.Find("KeybindText").GetComponent<TextMeshProUGUI>().text = "MMB";
        loadoutUI1.transform.Find("LoadoutImage").GetComponent<Image>().sprite = null;
        loadoutUI1.transform.Find("NumItemText").GetComponent<TextMeshProUGUI>().text = "";
        loadoutPanels[2] = loadoutUI1;
        var loadoutUI2 = Instantiate(loadoutPanel, loadoutPanelsContainer.transform);
        loadoutUI2.transform.Find("KeybindText").GetComponent<TextMeshProUGUI>().text = "RMB";
        loadoutUI2.transform.Find("LoadoutImage").GetComponent<Image>().sprite = null;
        loadoutUI2.transform.Find("NumItemText").GetComponent<TextMeshProUGUI>().text = "";
        loadoutPanels[3] = loadoutUI2;
    }

    public void EquipLoadoutItem(Consumable item)
    {
        if (loadoutItems.Contains(item))
            return;
        
        // enable black background
        blackPanel.gameObject.SetActive(true);
        
        // wait for button input
        for (int i = 0; i < 4; i++)
        {
            var button = loadoutPanels[i].GetComponent<Button>();
            var x = i;
            button.onClick.AddListener(() => EquipLoadoutItemAtSlot(item, x));;
            button.interactable = true;
        }
    }

    public void EquipLoadoutItemAtSlot(Consumable item, int index)
    {
        Debug.Log(index);
        blackPanel.gameObject.SetActive(false);
        loadoutItems[index] = item;
        loadoutPanels[index].transform.Find("LoadoutImage").GetComponent<Image>().enabled = true;
        loadoutPanels[index].transform.Find("LoadoutImage").GetComponent<Image>().sprite = item.icon;
        RefreshItemNumber(index);
        
        // disable button input
        for (int i = 0; i < 4; i++)
        {
            var button = loadoutPanels[i].GetComponent<Button>();
            button.interactable = false;
        }
    }

    public void RefreshItemNumber(int index)
    {
        if (loadoutItems[index] == null) // nothing in that slot
            return;
        var item = loadoutItems[index];
        loadoutPanels[index].transform.Find("NumItemText").GetComponent<TextMeshProUGUI>().text = "x" + inventoryManager.GetItemAmountOwned(item);
    }

    public void RefreshAllItemNumbers()
    {
        for (int i = 0; i < 4; i++)
        {
            RefreshItemNumber(i);
        }
    }

    public void UseLoadout(int index)
    {
        if (loadoutItems[index] == null) // nothing in that slot
            return;
        inventoryManager.ConsumeItem(loadoutItems[index]);
        RefreshItemNumber(index);
    }

    public bool IsItemEquiped(Consumable item)
    {
        return loadoutItems.Contains(item);
    }

}
