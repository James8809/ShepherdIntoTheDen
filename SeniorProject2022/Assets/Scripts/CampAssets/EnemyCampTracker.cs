using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

public class EnemyCampTracker : MonoBehaviour
{
    private int _numEnemiesInCamp;
    public Action OnAllEnemiesDead;
    private void Awake()
    {
        // get all enemies and link up their deaths to the tracker for the event when all are dead
        var enemies = GetComponentsInChildren<DestroyableEnemy>();
        _numEnemiesInCamp = enemies.Length;
        foreach (var enemy in enemies)
        {
            enemy.OnDeath += OnEnemyDeath;
        }
    }
    

    public void OnEnemyDeath(DestroyableEnemy enemy)
    {
        _numEnemiesInCamp--;
        if (_numEnemiesInCamp == 0)
        {
            // call event for all enemies dead in camp
            OnAllEnemiesDead();
        }
    }
}
