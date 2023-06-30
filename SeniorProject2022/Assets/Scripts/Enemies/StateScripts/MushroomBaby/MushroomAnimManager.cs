using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using Unity.Mathematics;
using UnityEngine;

public class MushroomAnimManager : MonoBehaviour
{
    public GameObject explosion;
    public ParticleSystem sparks;
    
    [SerializeField] private SkinnedMeshRenderer capMesh;
    [SerializeField][ColorUsage(true, true)] private Color activeColor, inactiveColor;

    private MushroomBabyEnemyAgent _mushAgent;
    [SerializeField] private PlayerReferenceManager _referenceManager;

    private void Awake()
    {
        _mushAgent = GetComponentInParent<MushroomBabyEnemyAgent>();
    }

    public void ActivateCap()
    {
        // TODO: lerp this change
        sparks.gameObject.SetActive(true);
        capMesh.material.SetColor("_FallbackColor", activeColor);
    }
    
    public void Explode()
    {
        _referenceManager.EffectManager.ShakeCamera(10, 1.5f);
        capMesh.material.SetColor("_FallbackColor", inactiveColor);   // is this a good place to disable hat?
        sparks.gameObject.SetActive(false);
        var explosionref = Instantiate(explosion,
            transform.position, quaternion.identity, transform);
        explosionref.transform.localScale *= 8.5f;
        Destroy(explosionref, 3.0f);
    }

    public void SetMushSpeed(float speed)
    {
        _mushAgent.navMeshAgent.speed = speed;
    }

    public void SetAngularAcceleration(float speed)
    {
        _mushAgent.navMeshAgent.angularSpeed = speed;
    }

    public void StopAimingAtPlayer()
    {
        _mushAgent.canAimAtPlayer = false;
    }

    public void SetActiveCollider(int active)
    {
        _mushAgent.SetActiveCollider(active != 0);
    }
}
