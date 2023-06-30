using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class FloraConditional : EnemyConditional
{
    protected FloraManager _flora;

    public override void OnAwake()
    {
        base.OnAwake();
        _flora = GetComponent<FloraManager>();
    }
}