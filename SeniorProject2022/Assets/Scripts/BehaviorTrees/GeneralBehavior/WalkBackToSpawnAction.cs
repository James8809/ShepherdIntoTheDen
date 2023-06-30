using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class WalkBackToSpawnAction : EnemyAction
{
    public float closeEnoughRange = 3.5f;
    public float movebackSpeed = 7.0f;
    private TaskStatus currentTaskStatus;
    private Vector3 goalPosition;

    public override void OnAwake()
    {
        base.OnAwake();
        // get a valid sample pos for returning back to:
        int attemptedTries = 0;
        while (attemptedTries < 200)
        {
            NavMeshHit hit;
            bool gotHit = NavMesh.SamplePosition(_navmeshAgent.transform.position,
                out hit, _navmeshAgent.height * 2.0f, _navmeshAgent.areaMask);
            if (gotHit)
            {
                goalPosition = hit.position;
                break;
            }
            attemptedTries++;
        }

        if (attemptedTries == 200)
        {
            Debug.Log("Something has gone horribly wrong, I have tried 200 times to get a valid navmesh near me");
        }
    }

    // called once when node is executed
    public override void OnStart()
    {
        _navmeshAgent.speed = movebackSpeed;
        currentTaskStatus = TaskStatus.Running;
        _navmeshAgent.SetDestination(goalPosition);
    }

    // nodes can run more than one frame while running
    public override TaskStatus OnUpdate()
    {
        // inform animator of current speed of enemy
        _animator.SetFloat("speed", _navmeshAgent.velocity.magnitude);

        // if close to player, we have succeeded
        float startDist = (transform.position - goalPosition).magnitude;
        if(startDist <= closeEnoughRange)
        {
            currentTaskStatus = TaskStatus.Success;
        }
        return currentTaskStatus;
    }
}