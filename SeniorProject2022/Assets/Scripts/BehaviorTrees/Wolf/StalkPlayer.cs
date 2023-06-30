using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

public class StalkPlayerAction : WolfAction
{
    public float moveSpeed = 2f;
    public float minTimeUntilPounce = 2.0f;
    public float maxTimeUntilPounce = 6.0f;
    public float stalkDistance = 6f;
    public float attackRange = 4f;
    public float outOfRangeDistance = 7f;
    public float numSegmentsInStalkRadius = 32;
    private TaskStatus currentTaskStatus;
    private Tween pounceTimer;
    private Vector3 localTargetPosition;
    private Vector3 worldTargetPosition;
    private float arrivalThreshold = 1.0f;
    private float rotationOffset;
    private int direction;
    private int tooSlowTimer = 0;
    private int maxTooSlowCount = 6;
    int rad = 30;
    int currPoint;

    // called once when node is executed
    public override void OnStart()
    {
        _navmeshAgent.speed = moveSpeed;
        currentTaskStatus = TaskStatus.Running;
        pounceTimer = DOVirtual.DelayedCall(Random.Range(minTimeUntilPounce, maxTimeUntilPounce), () => 
            {
                //Debug.Log("Pounce on player!");
                currentTaskStatus = TaskStatus.Success;
            },
            false);
        currPoint = 0;
        Vector3 towardsPlayer = transform.position - _player.transform.position;
        rotationOffset = Mathf.Atan2(towardsPlayer.z, towardsPlayer.x);
        localTargetPosition = GetNextPoint();
        _animator.SetBool("stalking", true);
        direction = (int) Mathf.Round(Random.value);
        if (direction == 0)
        {
            direction = -1;
        }
    }

    // nodes can run more than one frame while running
    public override TaskStatus OnUpdate()
    {
        float playerDist = (transform.position - _player.transform.position).magnitude;
        if(playerDist <= attackRange)
        {
            //Debug.Log("Bite player!");
            return TaskStatus.Success;
        }
        if (playerDist >= outOfRangeDistance)
        {
            //Debug.Log("Out of range...");
            _animator.SetBool("stalking", false);
            return TaskStatus.Failure;
        }

        // walk a circle around the player
        if ((transform.position - worldTargetPosition).magnitude < arrivalThreshold)
        {
            localTargetPosition = GetNextPoint();
        }
        
        if (_navmeshAgent.velocity.magnitude < 0.5f * moveSpeed)
        {
            //Debug.Log("moving slow.");
            tooSlowTimer++;
            if (tooSlowTimer >= maxTooSlowCount)
            {
                float decision = Random.value;
                if (decision < 0.5f)
                {
                    direction *= -1;
                    localTargetPosition = GetNextPoint();
                }
                else
                {
                    currentTaskStatus = TaskStatus.Success;
                }
                tooSlowTimer = 0;
            }
        }
        else
        {
            tooSlowTimer = 0;
        }

        worldTargetPosition = localTargetPosition + _player.transform.position;

        _navmeshAgent.SetDestination(worldTargetPosition);
        return currentTaskStatus;
    }

    public override void OnEnd()
    {
        pounceTimer?.Kill();
    }

    private Vector3 GetNextPoint()
    {
        currPoint += direction;
        float radian = (currPoint / numSegmentsInStalkRadius) * Mathf.PI * 2 + rotationOffset;
        Vector3 output = new Vector3(stalkDistance * Mathf.Cos(radian), 0, stalkDistance * Mathf.Sin(radian));
        return output;
    }
}