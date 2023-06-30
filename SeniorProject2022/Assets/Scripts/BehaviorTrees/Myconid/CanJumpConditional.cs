using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class CanJumpConditional : MyconidConditional
{
    public float minJumpRange = 3.0f;
    public float maxJumpRange = 10.0f;

    public override TaskStatus OnUpdate()
    {
        NavMeshHit hit;
        float distToPlayer = (_player.transform.position - transform.position).magnitude;
        // check if the myconid can see the player and is in range
        if(distToPlayer >= minJumpRange && distToPlayer <= maxJumpRange &&
           !_navmeshAgent.Raycast(_player.transform.position, out hit))
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}