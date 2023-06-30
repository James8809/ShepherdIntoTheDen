using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningState : PlayerState
{
    public RunningState(PlayerController player) : base(player) {}
    public override void OnDash()
    {
        TransitionState(_player.dashState);
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
        _player._characterController.Move(Time.deltaTime * _player.speed * worldMoveDirection * _player.speedMultiplier);
    }

    public override void OnAttack()
    {
        if (_player.attackState != null)
        {
            TransitionState(_player.attackState);
        }
    }
    
    // consumable/ability callbacks
    public override void OnAbilityStarted(AbilityState currAbility)
    {
        TransitionState(currAbility);
    }

    public override void OnConsumableStarted()
    {
        // get current consumable, if any, from the player and transition to it if possible
        if (_player.currConsumable != null)
        {
            TransitionState(_player.currConsumable);
        }
    }
}
