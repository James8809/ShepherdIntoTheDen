using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UltraRun : AbilityState
{
    public Tween delayTween;
    public UltraRun(PlayerController player) : base(player){}
    public int manaCost = 10;
    public float count = 0;

    public override void EnterState()
    {
        checkMana();

    }
    public override void ExitState()
    {
        Debug.Log("out of ultra run");
        //_player._animator.SetBool("ultraRun", false);
        _player._animator.SetFloat("ultraSpeed", -2f);
        delayTween?.Kill();
    }

    public override void Execute()
    {
        Quaternion currentRotation = _player.transform.rotation;
        Vector3 worldMoveDirection = _player.currentMovement;
        if (worldMoveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(worldMoveDirection);
            _player.transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, 
                _player.rotationsPerSecond * Time.deltaTime);;
        }
        _player._characterController.Move(Time.deltaTime * _player.speed * worldMoveDirection * 3f);
        _player._animator.SetFloat("ultraSpeed", _player.currentMovement.magnitude);

        if (delayTween == null){
            delayTween = DOVirtual.DelayedCall(2f, ()=> checkMana());
        }
    }

    public override void OnAbilityEnded()
    {
        TransitionState(_player.runningState);
    }
    public override void OnDash()
    {
        TransitionState(_player.dashState);
    }

    public override void OnAttack()
    {
        if (_player.attackState != null)
        {
            TransitionState(_player.attackState);
        }
    }

    public void checkMana()
    {
        if (!_player.playerManaSystem.UseMana(manaCost)) // if not enough mana
                TransitionState(_player.runningState);
        delayTween = null;
    }
}
