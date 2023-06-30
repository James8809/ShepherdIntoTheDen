using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class PatrolAction : EnemyAction
{
    public float moveSpeed = 3.5f;
    public float patrolDist = 12.0f;
    public float distThreshold = 0.4f;
    private TaskStatus currentTaskStatus;
    private Vector3 toMovePoint;
    private Tween waitTween, giveUpTween;
    public float minWaitTime = 7f;
    public float maxWaitTime = 10f;
    public float giveUpTime = 19.0f;
    private bool resetWayPoint = false;
    private bool inPatrolWait = false;
    public int numPatrolPoints = 5;

    public List<Vector3> validPatrolPoints = new List<Vector3>();
    

    public override void OnAwake()
    {
        base.OnAwake();
        // generate patrol points
        int generatedPatrolPoints = 0;
        int attemptedTries = 0;
        while (attemptedTries < 200 && generatedPatrolPoints < numPatrolPoints)
        {
            NavMeshHit hit;
            Vector3 randomPosition = (Random.insideUnitSphere.normalized *
                                      Random.Range(.2f, 1.0f) * patrolDist) + _startingPosition;
            bool gotHit = NavMesh.SamplePosition(randomPosition,
                out hit, 1.0f, _navmeshAgent.areaMask);
            if (gotHit)
            {
                generatedPatrolPoints++;
                validPatrolPoints.Add(hit.position);
            }
            attemptedTries++;
        }
    }

    private void MoveToNewPoint()
    {
        Vector3 toMovePoint = _startingPosition;
        if (validPatrolPoints.Count > 0)
        {
            toMovePoint = validPatrolPoints[Random.Range(0, validPatrolPoints.Count - 1)];
        }

        inPatrolWait = true;
        waitTween = DOVirtual.DelayedCall(Random.Range(minWaitTime, maxWaitTime), () =>
        {
            inPatrolWait = false;
            _navmeshAgent.SetDestination(toMovePoint);
            resetWayPoint = false;
            giveUpTween = DOVirtual.DelayedCall(giveUpTime, () => resetWayPoint = true, false);
        }, false);
    }
    
    // called once when node is executed
    public override void OnStart()
    {
        _navmeshAgent.speed = moveSpeed;
        currentTaskStatus = TaskStatus.Running;
        MoveToNewPoint();
    }

    // nodes can run more than one frame while running
    public override TaskStatus OnUpdate()
    {
        // inform animator of current speed of enemy
        _animator.SetFloat("speed", _navmeshAgent.velocity.magnitude);

        // set navmesh destination to player while chasing
        if (!inPatrolWait && (resetWayPoint || _navmeshAgent.pathStatus == NavMeshPathStatus.PathComplete))
        {
            MoveToNewPoint();
        }
        return currentTaskStatus;
    }

    public override void OnEnd()
    {
        waitTween?.Kill();
        giveUpTween?.Kill();
    }
}