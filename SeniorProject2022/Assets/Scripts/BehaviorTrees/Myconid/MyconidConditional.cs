using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class MyconidConditional : EnemyConditional
{
    protected MyconidBehaviorTreeManager _myconid;

    public override void OnAwake()
    {
        base.OnAwake();
        _myconid = GetComponent<MyconidBehaviorTreeManager>();
    }
}