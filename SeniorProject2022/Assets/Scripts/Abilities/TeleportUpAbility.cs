using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TeleportUpAbility : AbilityState
{
    private Tween moveTween;
    private float cooldown;
    public TeleportUpAbility(PlayerController player) : base(player){}

    public override void EnterState()
    {
        // _player._characterController.Move( Vector3.up * 20.0f);
        // TransitionState(_player.runningState);
        // moveTween = DOVirtual.DelayedCall(4.0f, () => TransitionState(_player.runningState),
        //             false);

    }

    public override void ExitState()
    {
        moveTween?.Kill();
    }

    public override void Execute(){
        Quaternion currentRotation = _player.transform.rotation;
        Vector3 worldMoveDirection = _player.currentMovement;
        if (worldMoveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(worldMoveDirection);
            _player.transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, 
                _player.rotationsPerSecond * Time.deltaTime);;
        }
        _player._characterController.Move(Time.deltaTime * _player.speed * worldMoveDirection * 4.0f);
    }

}

