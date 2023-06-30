using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class CanSeePlayerConditional : EnemyConditional
{
    public float maxDistance = 6f;

    // nodes can run more than one frame while running
    public override TaskStatus OnUpdate()
    {
        NavMeshHit hit;
        // check if direct navmesh path to player.
        if (_navmeshAgent.isActiveAndEnabled && !_navmeshAgent.Raycast(_player.transform.position, out hit))
        {
            if (Vector3.Distance(transform.position, _player.transform.position) < maxDistance)
                return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}