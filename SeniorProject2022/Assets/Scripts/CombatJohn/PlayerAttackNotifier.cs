using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackNotifier : MonoBehaviour
{
    private PlayerCombatManager _combatManager;

    private void Awake()
    {
        // need some sort of manager to guarantee these things are always in the scene.
        _combatManager = GetComponentInParent<PlayerCombatManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _combatManager.OnEnemyHit();
    }
}
