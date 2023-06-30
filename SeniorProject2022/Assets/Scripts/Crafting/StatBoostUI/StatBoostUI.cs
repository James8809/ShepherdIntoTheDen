using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crafting;
using TMPro;
using UnityEngine.UI;

public class StatBoostUI : MonoBehaviour
{
    [Header("Drag GameObjects")] public GameObject statBoostContainer;

    [Header("Prefabs")] public GameObject StatBoostTab;

    private Dictionary<StatBooster.StatType, GameObject> statBoostTabs = new Dictionary<StatBooster.StatType, GameObject>();

    public void UpdateStatTabData(StatBooster.StatType statType, float percentage)
    {
        var tab = statBoostTabs[statType];
        tab.transform.Find("CircleIcon").GetComponent<Image>().fillAmount = percentage;
    }

    public void DestroyTab(StatBooster.StatType statType)
    {
        var tab = statBoostTabs[statType];
        Destroy(tab);
        statBoostTabs.Remove(statType);
    }


    public void CreateStatBoostTab(StatBooster statBooster)
    {
        if (statBoostTabs.ContainsKey(statBooster.statType)) //already have that stat type
            return;
        var tab = Instantiate(StatBoostTab, statBoostContainer.transform);
        tab.transform.Find("TypeText").GetComponent<TextMeshProUGUI>().text = statBooster.statType.ToString();
        tab.transform.Find("MultiplierAmountText").GetComponent<TextMeshProUGUI>().text = statBooster.value + "x";
        tab.transform.Find("Icon").GetComponent<Image>().sprite = statBooster.statIcon;
        tab.transform.Find("CircleIcon").GetComponent<Image>().color = statBooster.circleIconColor;
        statBoostTabs[statBooster.statType] = tab;
    }
}
