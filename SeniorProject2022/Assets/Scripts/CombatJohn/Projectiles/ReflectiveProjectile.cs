using System;
using Unity.Mathematics;
using UnityEngine;

public class ReflectiveProjectile : ProjectileBehavior
{
    public float speed = 3.0f;
    [SerializeField] private GameObject deathParticles;
    [SerializeField] private GameObject onReflectPlayerProjectile;
    [SerializeField] private LayerMask toReflectMask;
    private GameObject reflectObject = null;
    
    
    public void SetReflectTarget(GameObject rObject)
    {
        reflectObject = rObject;
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(transform.position + Time.fixedDeltaTime * speed * transform.forward);
    }

    private void DestroyAndSpawnParticles()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Enemy/Flora/FloraProjectileImpact", this.transform.gameObject);
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
        if (toReflectMask == (toReflectMask | (1 << other.gameObject.layer)))
        {
            // reflect the projectile towards the enemy
            // spawn new object, delete this one
            var newProj = Instantiate(onReflectPlayerProjectile, transform.position, Quaternion.identity);
            // get the diff, such that the y val is 0 in the forwards vector
            // player -> reflectObject vector
            Vector3 rPos = reflectObject.transform.position;
            rPos.y = newProj.transform.position.y;
            newProj.transform.LookAt(rPos);
            DestroyAndSpawnParticles();
        }
        else
        {
            DestroyAndSpawnParticles();
        }
    }
}