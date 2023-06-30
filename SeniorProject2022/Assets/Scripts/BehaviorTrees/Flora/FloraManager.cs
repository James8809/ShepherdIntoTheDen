using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloraManager : EnemyManager
{
    [SerializeField] private Transform attackSpawn;    // this will be made an array after prototype
    [SerializeField] private GameObject floraProjectile;
    [SerializeField] private Animator _animator;

    [HideInInspector] public bool isAttackingPlayer;
    
    public GameObject SpawnProjectile()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Enemy/Flora/FloraAttack", this.transform.gameObject);
        var projectile =  Instantiate(floraProjectile, attackSpawn.position, transform.rotation);
        projectile.GetComponent<ReflectiveProjectile>().SetReflectTarget(gameObject);
        return projectile;
    }

    public override Animator GetAnimator()
    {
        return _animator;
    }
}
