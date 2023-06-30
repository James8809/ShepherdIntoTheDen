using System;
using System.Collections;
using System.Collections.Generic;
using Crafting;
using UnityEngine;
using DG.Tweening;

public class PlayerStats : MonoBehaviour
{
    [HideInInspector] public float attackMultiplier = 1;
    [HideInInspector] public float damageTakenMultiplier = 1;
    [HideInInspector] public float speedMultiplier = 1;

    private float attackMultTime = 0;
    private float damageTakenMultTime = 0;
    private float speedMultTime = 0;

    private float totalAttackTime;
    private float totalDamageTakenTime;
    private float totalSpeedTime;

    private PlayerController playerController;
    public WeaponObject weapon;
    private StatBoostUI statBoostUI;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>(true);
        weapon = GetComponentInChildren<Weapon>(true).weaponType;
        weapon.ResetMultiplier();
        statBoostUI = FindObjectOfType<StatBoostUI>();
    }

    // Stat multipliers
    public void SetAttackMultiplier(StatBooster statBooster)
    {
        SetupMultiplierCoroutine(ref attackMultTime, ref attackMultiplier, ref totalAttackTime, statBooster,
            () =>
        {
            attackMultiplier = statBooster.value;
            weapon.SetMultiplier(statBooster.value);
        }, AttackMultiplierTimer());
    }
    public void SetDamageTakenMultiplier(StatBooster statBooster)
    {
        SetupMultiplierCoroutine(ref damageTakenMultTime, ref damageTakenMultiplier,  ref totalDamageTakenTime, statBooster,
            () =>
        {
            damageTakenMultiplier = statBooster.value;
        }, DamageTakenMultiplierTimer());
    }
    public void SetSpeedMultiplier(StatBooster statBooster)
    {
        SetupMultiplierCoroutine(ref speedMultTime, ref speedMultiplier, ref totalSpeedTime, statBooster,
            () =>
        {
            playerController.speedMultiplier = statBooster.value;
            speedMultiplier = statBooster.value;
        }, SpeedMultiplierTimer());
    }
    
    private void SetupMultiplierCoroutine(ref float multTime, ref float varMultiplier, ref float totalTime,
        StatBooster statBooster, Action startMultiplierFunc, IEnumerator coroutineTimerFunc)
    {
        // multiplier with no time limit
        if (statBooster.duration < 0) return;
        
        // Different multiplier
        if (multTime > 0 && !Mathf.Approximately(statBooster.value, varMultiplier))
            return;

        totalTime += statBooster.duration;
        // if statBooster.durationr approx 0, start coroutine
        if (Mathf.Approximately(multTime, 0)) {
            multTime += statBooster.duration;
            statBoostUI.CreateStatBoostTab(statBooster);
            StartCoroutine(coroutineTimerFunc);
        }
        else {
            multTime += statBooster.duration;
        }
        startMultiplierFunc?.Invoke();
    }
    
    // Coroutines
    private IEnumerator AttackMultiplierTimer()
    {
        while (attackMultTime > 0)
        {
            attackMultTime -= Time.deltaTime;
            statBoostUI.UpdateStatTabData(StatBooster.StatType.Attack, attackMultTime / totalAttackTime);
            yield return null;
        }

        attackMultTime = 0;
        attackMultiplier = 1;
        totalAttackTime = 0;
        var weapon = GetComponentInChildren<Weapon>(true).weaponType;
        weapon.ResetMultiplier();
        statBoostUI.DestroyTab(StatBooster.StatType.Attack);
        Debug.Log("Attack Multiplier done");
    }
    
    private IEnumerator DamageTakenMultiplierTimer()
    {
        while (damageTakenMultTime > 0)
        {
            damageTakenMultTime -= Time.deltaTime;
            statBoostUI.UpdateStatTabData(StatBooster.StatType.Defense, damageTakenMultTime / totalDamageTakenTime);
            yield return null;
        }

        damageTakenMultTime = 0;
        totalDamageTakenTime = 0;
        damageTakenMultiplier = 1;
        statBoostUI.DestroyTab(StatBooster.StatType.Defense);
        Debug.Log("Damage Taken Multiplier done");
    }
    
    private IEnumerator SpeedMultiplierTimer()
    {
        while (speedMultTime > 0)
        {
            speedMultTime -= Time.deltaTime;
            statBoostUI.UpdateStatTabData(StatBooster.StatType.Speed, speedMultTime / totalSpeedTime);
            yield return null;
        }

        speedMultTime = 0;
        totalSpeedTime = 0;
        Debug.Log("Speed Multiplier done");
        playerController.speedMultiplier = 1;
        speedMultiplier = 1;
        statBoostUI.DestroyTab(StatBooster.StatType.Speed);

    }
}
