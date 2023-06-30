using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

public class BitePlayer : WolfAction
{
    public float moveSpeed = 2f;
    public float turnSpeed = 0.5f;
    public float attackRange = 4f;
    public float particleDelay = 0.5f;
    public float damageDelay = 0.25f;
    public float totalAttackDuration = 1.0f;
    public float damageDuration = 0.1f;
    private TaskStatus currentTaskStatus;
    private Tween damageTimer;
    private Tween endDamageTimer;
    private Tween particleTimer;
    private Tween colorTween;
    private Tween endAttackTween;
    private bool shouldFacePlayer = true;

    // called once when node is executed
    public override void OnStart()
    {
        currentTaskStatus = TaskStatus.Running;
        shouldFacePlayer = true;
        _navmeshAgent.speed = moveSpeed;
        float prevAcc = _navmeshAgent.acceleration;
        float prevStopDist = _navmeshAgent.stoppingDistance;
        _navmeshAgent.acceleration = 999f;
        _navmeshAgent.stoppingDistance = 1.5f;
        //float prevRotAcc = _navmeshAgent.angularSpeed;
        //_navmeshAgent.angularSpeed = 1000f;
        _animator.SetTrigger("biteAttack");
        
        colorTween = _enemyManager.LerpToColor(Color.white, damageDelay).OnComplete(() => 
        {
            DOVirtual.DelayedCall(damageDuration, () =>
            {
                _enemyManager.SetToonAddColor(Color.clear);
            },
            false);
        });

        damageTimer = DOVirtual.DelayedCall(damageDelay, () => 
            {
                shouldFacePlayer = false;
                _wolf.beginBite();
                _navmeshAgent.speed = 0f;

                endDamageTimer = DOVirtual.DelayedCall(damageDuration, () =>
                {
                    _wolf.endBite();
                },
                false);
            },
            false);

        particleTimer = DOVirtual.DelayedCall(particleDelay, () =>
        {
            _wolf.BiteParticle();
        },
            false);

        endAttackTween = DOVirtual.DelayedCall(totalAttackDuration, () =>
        {
            _navmeshAgent.acceleration = prevAcc;
            _navmeshAgent.stoppingDistance = prevStopDist;
            currentTaskStatus = TaskStatus.Success;
        },
            false);
    }

    // nodes can run more than one frame while running
    public override TaskStatus OnUpdate()
    {
        
        //_wolf.transform.rotation = Quaternion.RotateTowards(_wolf.transform.rotation, Quaternion.LookRotation(_wolf.transform.position - _player.transform.position).normalized, Time.deltaTime * turnSpeed);
        //_wolf.transform.rotation = Quaternion.RotateTowards(_wolf.transform.rotation, Quaternion.Euler((_wolf.transform.position - _player.transform.position).normalized), turnSpeed);
        if (shouldFacePlayer)
        {
            _wolf.transform.LookAt(_player.transform.position);
        }
        
        _navmeshAgent.SetDestination(_player.transform.position);
        return currentTaskStatus;
    }

    public override void OnEnd()
    {
        damageTimer?.Kill();
        endDamageTimer?.Kill();
        particleTimer?.Kill();
        colorTween?.Kill();
        endAttackTween?.Kill();
    }
}