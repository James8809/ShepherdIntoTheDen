using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManaText : MonoBehaviour
{
    private PlayerManaSystem _manaSystem;
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        _manaSystem = FindObjectOfType<PlayerManaSystem>();
    }

    private void OnEnable()
    {
        _manaSystem.onManaChanged += ChangeManaText;
        _manaSystem.onManaInitialized += InitializeMana;
    }
    
    private void OnDisable()
    {
        _manaSystem.onManaChanged -= ChangeManaText;
        _manaSystem.onManaInitialized -= InitializeMana;
    }

    public void ChangeManaText(int currentMana, int maxMana, int manaChanged)
    {
        text.text = currentMana + "/" + maxMana;
    }

    public void InitializeMana(int currentMana, int maxMana)
    {
        text.text = currentMana + "/" + maxMana;
    }
}
