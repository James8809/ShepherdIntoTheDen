using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using UnityEngine;
using UnityEngine.AI;

public class FloraAction : EnemyAction
{
    protected FloraManager _flora;

    public override void OnAwake()
    {
        base.OnAwake();
        _flora = GetComponent<FloraManager>();
    }
}