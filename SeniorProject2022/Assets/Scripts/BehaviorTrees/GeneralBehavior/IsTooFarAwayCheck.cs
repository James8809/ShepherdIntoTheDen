using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class IsTooFarAwayCheck : EnemyConditional
{
    public float maxDistance = 20.0f;

    // check if distance too far away from spawn, return success if too far away
    public override TaskStatus OnUpdate()
    {
        if (Vector3.Distance(_enemyManager.InitialPosition, transform.position) < maxDistance)
        {
            return TaskStatus.Failure;
        }
        return TaskStatus.Success;
    }
}