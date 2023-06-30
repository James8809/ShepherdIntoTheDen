using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class EnemyConditional : Conditional
{
    protected Rigidbody _rigidbody;
    protected Animator _animator;
    protected PlayerController _player;
    protected NavMeshAgent _navmeshAgent;
    protected Health _health;
    protected EnemyManager _enemyManager;

    public override void OnAwake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _player = PlayerController.Instance;
        _navmeshAgent = GetComponent<NavMeshAgent>();
        _health = GetComponent<Health>();
        _enemyManager = GetComponent<EnemyManager>();    // required for damage taking/color
        _animator = _enemyManager.GetAnimator();
    }
}