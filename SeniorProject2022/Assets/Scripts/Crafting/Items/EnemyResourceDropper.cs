using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;

public class EnemyResourceDropper : ResourceDropper
{
    private DestroyableEnemy enemy;
    void Start()
    {
        // Try find enemy manager
        enemy = GetComponent<DestroyableEnemy>();
        enemy.OnDeath += EnemySpawnResources;
    }

    // Update is called once per frame
    private void OnDestroy()
    {
        enemy.OnDeath -= EnemySpawnResources;
    }

    private void EnemySpawnResources(DestroyableEnemy enemy)
    {
        SpawnResources();
    }
}
