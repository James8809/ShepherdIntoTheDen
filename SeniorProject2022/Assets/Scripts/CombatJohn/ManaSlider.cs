using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ManaSlider : MonoBehaviour
{
    private Material _sliderMat;
    private PlayerManaSystem _manaSystem;

    public int framesBetweenUpdate = 3;
    private float currentPct = 1.0f;

    private void Awake()
    {
        _sliderMat = GetComponent<Image>().material;
        _manaSystem = FindObjectOfType<PlayerManaSystem>();
    }
    

    private void OnEnable()
    {
        _manaSystem.onManaInitialized += InitializeBar;
        _manaSystem.onManaChanged += UpdateShownMana;
    }
    
    private void OnDisable()
    {
        _manaSystem.onManaInitialized -= InitializeBar;
        _manaSystem.onManaChanged -= UpdateShownMana;
    }

    public void SetSliderPct(float pct)
    {
        _sliderMat.SetFloat("_HealthPercent", pct);
    }

    private void UpdateShownMana(int currentMana, int maxMana, int change)
    {
        float newPct = (float) currentMana / maxMana;
        // Start coroutine to slowly adjust health over time, and cancel existing coroutines if there is one.
        DOTween.To(() => currentPct, x =>
            {
                _sliderMat.SetFloat("_HealthPercent", currentPct);
                currentPct = x;
            },
            newPct, framesBetweenUpdate / 60.0f);
    }
    

    private void InitializeBar(int currentMana, int maxMana)
    {
        float pct = (float)currentMana / maxMana;
        currentPct = pct;
        SetSliderPct(pct);
    }
}
