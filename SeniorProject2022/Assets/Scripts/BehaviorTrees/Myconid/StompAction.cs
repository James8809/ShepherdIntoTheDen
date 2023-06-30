using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;

public class StompAction : MyconidAction
{
    public string animationTriggerName;
    public float stompDuration;
        
        

    // called once when node is executed
    public override void OnStart()
    {
        _animator.SetTrigger(animationTriggerName);
    }

    // nodes can run more than one frame while running
    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
            
    }
}