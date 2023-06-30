using System;
using Unity.Mathematics;
using UnityEngine;

public class StraightProjectile : ProjectileBehavior
{
    public float speed = 3.0f;
    [SerializeField] private GameObject deathParticles;

    private void FixedUpdate()
    {
        _rb.MovePosition(transform.position + Time.fixedDeltaTime * speed * transform.forward );
    }
    
    private void DestroyAndSpawnParticles()
    {
        var dp = Instantiate(deathParticles, transform.position, transform.rotation);
        Destroy(dp, 3.0f);    // so there aren't infinite particle systems :0
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        DestroyAndSpawnParticles();
    }

    private void OnTriggerEnter(Collider other)
    {
        DestroyAndSpawnParticles();
    }
}