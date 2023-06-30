using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using UnityEngine;
using DG.Tweening;

public class FloraShoot : FloraAction
{
    public float moveSpeed = 0.0f;
    private bool shouldFacePlayer = false;
    public float rotationFactor = .1f;
    
    private Tween _attackColorTween;
    private Tween _attackTween;
    public float buildUpTime = .5f;
    public float attackDuration = 1.0f;
    private TaskStatus status;

    // called once when node is executed
    public override void OnStart()
    {
        _navmeshAgent.enabled = false;
        shouldFacePlayer = true;
        status = TaskStatus.Running;

        _attackColorTween = _enemyManager.LerpToColor(Color.white,
            buildUpTime).OnComplete(() => _enemyManager.SetToonAddColor(Color.clear));

        // entire shoot sequence is called from this method
        _attackTween = DOVirtual.DelayedCall(buildUpTime, () =>
        {
            shouldFacePlayer = false;
            // instantiate the projectile
            var projectile = _flora.SpawnProjectile();
        }, false).OnComplete(() =>
        {
            DOVirtual.DelayedCall(attackDuration, () =>
            {
                status = TaskStatus.Success;
            }, false);
        });
    }

    // nodes can run more than one frame while running
    public override TaskStatus OnUpdate()
    {
        if(shouldFacePlayer)
        {
            // look at player
            Vector3 toPlayer = _player.transform.position - _flora.transform.position;
            toPlayer.y = 0;
            toPlayer = toPlayer.normalized;
            if (toPlayer.magnitude != 0)
            {
                _flora.transform.rotation = Quaternion.Slerp(_flora.transform.rotation,
                    Quaternion.LookRotation(toPlayer),
                    rotationFactor);
            }
        }
        return status;
    }

    public override void OnEnd()
    {
        _rigidbody.isKinematic = true;
        _navmeshAgent.enabled = true;
        _attackTween?.Kill();
        _attackColorTween?.Kill();
        _enemyManager.SetToonAddColor(Color.clear);
    }
}
