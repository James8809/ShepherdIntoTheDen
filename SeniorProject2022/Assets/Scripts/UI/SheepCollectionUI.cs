using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SheepCollectionUI : MonoBehaviour
{
    private TextMeshProUGUI sheepCollectionText;
    private int numSheepInScene;
    private int sheepCollected = 0;
    private const string startText = "Sheep Rescued: ";
    public Action OnAllSheepCollected;
    public bool toggleOnEnemyDeath;
    [SerializeField] private EnemyCampTracker _enemyCampTracker;

    private void Awake()
    {
        // subscribe to all dead event.
        if (_enemyCampTracker != null)
        {
            _enemyCampTracker.OnAllEnemiesDead += OnAllSheepCollected;
        }
        
        sheepCollectionText = GetComponent<TextMeshProUGUI>();
        numSheepInScene = FindObjectsOfType<SheepController>().Length;
        sheepCollected = 0;
        UpdateText();
    }

    private void OnEnable()
    {
        SheepController.OnSheepCollected += IncrementText;
    }

    private void OnDisable()
    {
        SheepController.OnSheepCollected -= IncrementText;
    }

    void UpdateText()
    {
        sheepCollectionText.text = startText + sheepCollected + "/" + numSheepInScene;
    }

    private void IncrementText()
    {
        ++sheepCollected;
        if (toggleOnEnemyDeath)
            return;
        if (sheepCollected == numSheepInScene)
        {
            // launch game win event.
            if (OnAllSheepCollected != null)
            {
                OnAllSheepCollected();
            }
        }
        UpdateText();
    }
    
}
