using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Maybe this should be a general class and not attached to the player :/
 *
 * TODO: find out if i really need 2 classes for this or it could be one mega class that manages both effects +
 * hitting things in general.
 */
public class PlayerCombatManager : MonoBehaviour
{
    private CombatEffectManager _combatEffectManager;

    private void Awake()
    {
        _combatEffectManager = FindObjectOfType<CombatEffectManager>();
    }

    public void OnEnemyHit()
    {
        // stop time for 3 frames
        _combatEffectManager.StopTime(3);
        // shake the camera
        _combatEffectManager.ShakeCamera(10, .9f);
    }
}
