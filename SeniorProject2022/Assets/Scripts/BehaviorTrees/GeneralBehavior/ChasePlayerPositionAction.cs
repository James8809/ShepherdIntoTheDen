using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;

public class ChasePlayerPositionAction : EnemyAction
{
    public float moveSpeed = 3.5f;
    public float giveUpTime = 5.0f;    // number of seconds before task fails
    public float closeEnoughRange = 1.1f;
    private TaskStatus currentTaskStatus;
    private Tween giveUpTimer;
        

    // called once when node is executed
    public override void OnStart()
    {
        _navmeshAgent.speed = moveSpeed;
        currentTaskStatus = TaskStatus.Running;
        giveUpTimer = DOVirtual.DelayedCall(giveUpTime, () => 
            {
                currentTaskStatus = TaskStatus.Failure;
            },
            false);
    }

    // nodes can run more than one frame while running
    public override TaskStatus OnUpdate()
    {
        // inform animator of current speed of enemy
        _animator.SetFloat("speed", _navmeshAgent.velocity.magnitude);

        // if close to player, we have succeeded
        float playerDist = (transform.position - _player.transform.position).magnitude;
        if(playerDist <= closeEnoughRange)
        {
            return TaskStatus.Success;
        }

        // set navmesh destination to player while chasing
        _navmeshAgent.SetDestination(_player.transform.position);
        return currentTaskStatus;
    }

    public override void OnEnd()
    {
        giveUpTimer?.Kill();
    }
}