using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MyconidBehaviorTreeManager : EnemyManager
{
    [HideInInspector] public bool isAnimDone;   // reset by one-shot animations in behavior tree, reset by events
    public ParticleSystem stompParticles;
    public GameObject deathParticles;    // set in inspector
    [SerializeField] public Collider enemyBody;

    protected override void Die()
    {
        GameObject vfx = Instantiate(deathParticles, transform.position, Quaternion.identity) as GameObject;
        OnDeath?.Invoke(this);
        Destroy(vfx, 1.0f);
        Destroy(gameObject);
    }

    public void CompleteAnim()
    {
        isAnimDone = true;
    }

    public void PlayStompParticles()
    {
        stompParticles.Play();
    }
}
