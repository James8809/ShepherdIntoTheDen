using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using Enemy;
using UnityEngine;

public class EnemyRespawner : MonoBehaviour
{
    private int _numEnemies;
    public Action OnAllEnemiesDead;
    private DestroyableEnemy[] enemies;
    private bool respawningEnemies = false;
    [SerializeField] private float respawnDelay = 25.0f;    // time in seconds to start respawning.
    [SerializeField] private float respawnInterval = 7.0f;
    private List<DestroyableEnemy> deadEnemies;
    private void Awake()
    {
        // get all enemies and link up their deaths to the tracker for the event when all are dead
        enemies = GetComponentsInChildren<DestroyableEnemy>();
        _numEnemies = enemies.Length;
        respawningEnemies = false;
        deadEnemies = new List<DestroyableEnemy>();
        foreach (var enemy in enemies)
        {
            enemy.OnDeath += OnEnemyDeath;
        }
    }
    

    public void OnEnemyDeath(DestroyableEnemy enemy)
    {
        _numEnemies--;
        deadEnemies.Add(enemy);
        if (_numEnemies <= 0 && !respawningEnemies)
        {
            // start respawning enemies
            StartCoroutine(RespawnEnemies());
        }
    }

    private void SpawnEnemy(DestroyableEnemy enemy)
    {
        enemy.gameObject.SetActive(true);
    }

    private IEnumerator RespawnEnemies()
    {
        respawningEnemies = true;
        // wait a bit before respawning enemies
        yield return new WaitForSeconds(respawnDelay);
        while (Vector3.Distance(PlayerController.Instance.transform.position, transform.position) < 30.0f)
        {
            yield return null;
        }
        
        // select a random enemy (that is dead) to respawn
        // remove it from the dead enemies list
        int numDeadEnemies = deadEnemies.Count;
        // shuffle the list
        for (int i = 0; i < deadEnemies.Count; i++) {
            var temp = deadEnemies[i];
            int randomIndex = UnityEngine.Random.Range(i, deadEnemies.Count);
            deadEnemies[i] = deadEnemies[randomIndex];
            deadEnemies[randomIndex] = temp;
        }

        for (int i = 0; i < numDeadEnemies; i++)
        {
            SpawnEnemy(deadEnemies[i]);
            yield return new WaitForSeconds(respawnInterval);
        }
        
        respawningEnemies = false;
    }
}
