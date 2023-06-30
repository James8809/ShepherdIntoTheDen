using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    private Material _sliderMat;
    private PlayerHealthSystem _healthSystem;

    public int framesBetweenUpdate = 3;
    private float currentPct = 1.0f;

    private void Awake()
    {
        _sliderMat = GetComponent<Image>().material;
        _healthSystem = FindObjectOfType<PlayerHealthSystem>();
        SetSliderPct(currentPct);
    }
    
    private void OnEnable()
    {
        _healthSystem.onHealthInitialized += InitializeBar;
        _healthSystem.onHealthChanged += UpdateShownHealth;
    }
    
    private void OnDisable()
    {
        _healthSystem.onHealthInitialized -= InitializeBar;
        _healthSystem.onHealthChanged -= UpdateShownHealth;
    }

    public void SetSliderPct(float pct)
    {
        _sliderMat.SetFloat("_HealthPercent", pct);
    }

    private void UpdateShownHealth(int currentHealth, int maxHealth, int change)
    {
        float newPct = (float) currentHealth / maxHealth;
        // Start coroutine to slowly adjust health over time, and cancel existing coroutines if there is one.
        DOTween.To(() => currentPct, x =>
            {
                _sliderMat.SetFloat("_HealthPercent", currentPct);
                currentPct = x;
            },
            newPct, framesBetweenUpdate / 60.0f);
    }
    

    private void InitializeBar(int currentHealth, int maxHealth)
    {
        float pct = (float)currentHealth / maxHealth;
        currentPct = pct;
        SetSliderPct(pct);
    }
}
