using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthText : MonoBehaviour
{
    private PlayerHealthSystem _healthSystem;
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        _healthSystem = FindObjectOfType<PlayerHealthSystem>();
    }

    private void OnEnable()
    {
        _healthSystem.onHealthChanged += ChangeHealthText;
        _healthSystem.onHealthInitialized += InitializeHealth;
    }
    
    private void OnDisable()
    {
        _healthSystem.onHealthChanged -= ChangeHealthText;
        _healthSystem.onHealthInitialized -= InitializeHealth;
    }

    public void ChangeHealthText(int currentHealth, int maxHealth, int healthChanged)
    {
        text.text = currentHealth + "/" + maxHealth;
    }

    public void InitializeHealth(int currentHealth, int maxHealth)
    {
        text.text = currentHealth + "/" + maxHealth;
    }
}
