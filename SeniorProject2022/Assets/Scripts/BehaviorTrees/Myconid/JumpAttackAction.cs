using System.Data;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class JumpAttackAction : MyconidAction
{
    public string animationTriggerName;
    public float moveSpeed = 0.0f;
    private bool shouldFacePlayer = false;
    public float rotationFactor = .03f;
        
    private Tween jumpTween;
    private Tween _attackColorTween;
    public float jumpWait = .33f;
    public float jumpHeight = 4.0f;
    public float jumpDuration = 1.33f;
    private TaskStatus status;
    public float shakeAmount = 2.0f;

    private Vector3 jumpTarget;
        

    // called once when node is executed
    public override void OnStart()
    {
        _animator.SetFloat("speed", 0);
        _animator.SetTrigger(animationTriggerName);
        _navmeshAgent.enabled = false;
        shouldFacePlayer = true;
        status = TaskStatus.Running;
        _rigidbody.isKinematic = false;
        _myconid.enemyBody.enabled = false;

        _attackColorTween = _enemyManager.LerpToColor(Color.white,
            jumpWait).OnComplete(() => _enemyManager.SetToonAddColor(Color.clear));

        // jump sequence move
        jumpTween = DOVirtual.DelayedCall(jumpWait, () => 
        {
            shouldFacePlayer = false;
            // get a place to jump to around the player
            NavMeshHit hit;
            bool gotHit = NavMesh.SamplePosition(_player.transform.position,
                out hit, _navmeshAgent.height * 2.0f, _navmeshAgent.areaMask);
            if (!gotHit)
            {
                jumpTarget = transform.position;
            }
            
            jumpTarget = hit.position;
                
            // TODO: THIS NEEDS TO STOP IF CANCELLED!
            _rigidbody.DOJump(jumpTarget, jumpHeight, 1,
                jumpDuration, false).OnComplete(() => 
            {
                // change this to checking to see if he has landed on a navmesh in update after boolean change
                status = TaskStatus.Success;
                _enemyManager._referenceManager.EffectManager.ShakeCamera(35, shakeAmount);
            });
        }, false);
    }

    // nodes can run more than one frame while running
    public override TaskStatus OnUpdate()
    {
        if(shouldFacePlayer)
        {
            // look at player
            Vector3 toPlayer = _player.transform.position - _myconid.transform.position;
            toPlayer.y = 0;
            toPlayer = toPlayer.normalized;
            if (toPlayer.magnitude != 0)
            {
                _myconid.transform.rotation = Quaternion.Slerp(_myconid.transform.rotation,
                    Quaternion.LookRotation(toPlayer),
                    rotationFactor);
            }
        }

        _animator.SetFloat("speed", 0);
        return status;
    }

    public override void OnEnd()
    {
        _rigidbody.isKinematic = true;
        _navmeshAgent.Warp(jumpTarget);
        _navmeshAgent.enabled = true;
        DOVirtual.DelayedCall(.1f, () => _myconid.enemyBody.enabled = true, false);
        jumpTween?.Kill();
        _attackColorTween?.Kill();
        _enemyManager.SetToonAddColor(Color.clear);
    }
}