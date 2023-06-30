using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class WolfJumpAttack : WolfAction
{
    public string animationTriggerName;
    public float moveSpeed = 0.0f;
    private bool shouldFacePlayer = false;
    public float rotationFactor = .03f;
    public float landingOffset = 1f;
    private Tween jumpTween;
    private Tween rbMoveTween;
    private Tween damageTween;
    private Tween colorTween;
    public float jumpWait = .33f;
    public float jumpHeight = 4.0f;
    public float jumpDuration = 1.33f;
    public float damageDelay = 1f;
    private TaskStatus status;

    private Vector3 jumpTarget;


    // called once when node is executed
    public override void OnStart()
    {
        _animator.SetTrigger("lungeAttack");
        //_animator.SetTrigger(animationTriggerName);
        _animator.speed = 1f;
        _navmeshAgent.enabled = false;
        shouldFacePlayer = true;
        status = TaskStatus.Running;

        colorTween = _enemyManager.LerpToColor(Color.white, jumpDuration).OnComplete(() => _enemyManager.SetToonAddColor(Color.clear));

        // jump sequence move
        jumpTween = DOVirtual.DelayedCall(jumpWait, () =>
        {
            _rigidbody.isKinematic = false;
            _wolf.enemyBody.enabled = false;
            //shouldFacePlayer = false;
            // get a place to jump to around the player
            NavMeshHit hit;
            Vector3 towardsPlayer = (_player.transform.position - transform.position).normalized;
            Vector3 finalDestination = _player.transform.position - landingOffset * (towardsPlayer);
            bool gotHit = NavMesh.SamplePosition(finalDestination,
                out hit, 1.0f, _navmeshAgent.areaMask);
            if (!gotHit)
            {
                status = TaskStatus.Failure;    // nowhere on the navmesh to jump to
                return;
            }

            jumpTarget = hit.position;
            rbMoveTween = _rigidbody.DOJump(jumpTarget, jumpHeight, 1,
                jumpDuration, false).OnComplete(() =>
                {
                    // change this to checking to see if he has landed on a navmesh in update after boolean change
                    status = TaskStatus.Success;
                });
            _wolf.beginLunge();
        }, false);

        damageTween = DOVirtual.DelayedCall(damageDelay, () =>
        {
            _wolf.lungeLanding();
        }, false);
    }

    // nodes can run more than one frame while running
    public override TaskStatus OnUpdate()
    {
        if (shouldFacePlayer)
        {
            // look at player
            Vector3 toPlayer = (_player.transform.position - _wolf.transform.position).normalized;
            toPlayer.y = 0;
            _wolf.transform.rotation = Quaternion.Slerp(_wolf.transform.rotation, Quaternion.LookRotation(toPlayer),
                rotationFactor);
        }

        if (_wolf.lungeInterrupted)
        {
            status = TaskStatus.Failure;
        }

        _animator.SetFloat("speed", 0);
        return status;
    }

    public override void OnEnd()
    {
        _wolf.endLunge();
        _wolf.lungeInterrupted = false;
        _animator.speed = 1f;
        _rigidbody.isKinematic = true;
        _wolf.enemyBody.enabled = true;
        _navmeshAgent.Warp(jumpTarget);
        _navmeshAgent.enabled = true;
        jumpTween?.Kill();
        damageTween?.Kill();
        rbMoveTween?.Kill();
        colorTween?.Kill();
        _enemyManager.SetToonAddColor(Color.clear);
    }
}