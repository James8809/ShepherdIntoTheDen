using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

public class SetAnimTrigger : WolfAction
{
    public string triggerName;
    TaskStatus currentTaskStatus;

    // called once when node is executed
    public override void OnStart()
    {
        _animator.SetTrigger(triggerName);
        currentTaskStatus = TaskStatus.Success;
    }

    // nodes can run more than one frame while running
    public override TaskStatus OnUpdate()
    {
        return currentTaskStatus;
    }

    public override void OnEnd()
    {

    }
}