using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

public class SwipeAction : MyconidAction
{
    public string animationTriggerName;
    public float moveSpeed = 0.0f;
    private bool shouldFacePlayer = false;
    private Tween facePlayerTimer;
    public float timeToFacePlayer = .8f;
    public float rotationFactor = .03f;
        

    // called once when node is executed
    public override void OnStart()
    {
        _animator.SetFloat("speed", 0);
        _myconid.isAnimDone = false;
        _navmeshAgent.speed = moveSpeed;
        _animator.SetTrigger(animationTriggerName);
        _navmeshAgent.enabled = false;
        shouldFacePlayer = true;
        facePlayerTimer = DOVirtual.DelayedCall(timeToFacePlayer, () =>
        {
            shouldFacePlayer = false;
        }, false);
    }

    // nodes can run more than one frame while running
    public override TaskStatus OnUpdate()
    {
        if(shouldFacePlayer)
        {
            // look at player
            Vector3 toPlayer = (_player.transform.position - _myconid.transform.position).normalized;
            toPlayer.y = 0;
            _myconid.transform.rotation = Quaternion.Slerp(_myconid.transform.rotation, Quaternion.LookRotation(toPlayer), 
                rotationFactor);
        }

        _animator.SetFloat("speed", 0);
        if(_myconid.isAnimDone)
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Running;
        }
    }

    public override void OnEnd()
    {
        _navmeshAgent.enabled = true;
        facePlayerTimer?.Kill();
    }
}