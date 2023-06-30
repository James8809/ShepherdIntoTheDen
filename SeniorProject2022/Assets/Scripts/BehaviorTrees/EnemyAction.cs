using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAction : Action
{
    protected Rigidbody _rigidbody;
    protected Animator _animator;
    protected PlayerController _player;
    protected NavMeshAgent _navmeshAgent;
    protected Health _health;
    protected Vector3 _startingPosition;
    protected EnemyManager _enemyManager;
    
    protected Collider _enemyCollider;

    public override void OnAwake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _player = PlayerController.Instance;
        _navmeshAgent = GetComponent<NavMeshAgent>();
        _health = GetComponent<Health>();
        _enemyCollider = GetComponent<Collider>();    // prevents player from walking into enemy
        _enemyManager = GetComponent<EnemyManager>();    // required for damage taking/color
        _animator = _enemyManager.GetAnimator();

        _startingPosition = transform.position;
    }
}