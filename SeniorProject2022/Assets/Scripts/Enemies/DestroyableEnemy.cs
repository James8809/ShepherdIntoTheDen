using UnityEngine;
using System;

public abstract class DestroyableEnemy : MonoBehaviour
{
    [HideInInspector] public Health health;
    public Action<DestroyableEnemy> OnDeath;
    public Vector3 startingPosition;
    private bool startInitialized = false;

    protected virtual void EnemyAwake()
    {
        
    }
    
    

    private void Awake()
    {
        health = GetComponent<Health>();
        // this will make enemies respawn in exactly where they started
        if (!startInitialized)
        {
            startingPosition = transform.position; // save spawn position for respawn.
            startInitialized = true;
        }
        health.ResetHealth();

        if (startInitialized)
        {
            transform.position = startingPosition;
        }

        EnemyAwake();
    }
}