using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class WolfAction : EnemyAction
{
    protected WolfBehaviorTreeManager _wolf;

    public override void OnAwake()
    {
        base.OnAwake();
        _wolf = GetComponent<WolfBehaviorTreeManager>();
    }
}