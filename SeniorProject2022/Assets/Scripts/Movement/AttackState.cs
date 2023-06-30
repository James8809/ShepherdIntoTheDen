using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * This class simply controls the movement of the character when they are attacking. The animation state machine and
 * the actual weapon class will handle the animations for this and this state will be exited by the animations going
 * back to running/idle? Probs idk need to figure out something because everything is based on animation times.
 *
 * Note: ALL ATTACK ANIMATIONS MUST BE TAGGED "Attack"
 */
public class AttackState : PlayerState
{
    public bool attackAnimationFinished = false;
    public bool canDash = false;
    public bool canAttackAgain = false;
    public bool walkCancel = false;
    private float _moveFactor = 0.0f;
    private float _rotationFactor = 0.0f;
    private bool _dashBuffered = false;
    private bool _attackBuffered = false;
    private Quaternion initialRotation = Quaternion.identity;
    
    public AttackState(PlayerController _player) : base(_player) {}

    public override void EnterState()
    {
        _player._animator.SetBool(_player.attackTriggerHash, true);   // TODO: have this depend on some other "set attack state variable"
        _player._animator.SetBool(_player.walkCancelHash, false);
        attackAnimationFinished = false;
        canDash = false;
        canAttackAgain = false;
        walkCancel = false;
        _dashBuffered = false;
        _attackBuffered = false;
        _player.transform.rotation = initialRotation;
        // SetRotationToInput(_player);  // might not be needed anymore since _player instantly rotates.
    }

    public override void OnDash()
    {
        if (canDash)
        {
            TransitionState(_player.dashState);
        }
        else
        {
            _dashBuffered = true;
        }
    }

    public override void Execute()
    {
        // handle buffered dash:
        if (canDash && _dashBuffered)
        {
            OnDash();
            return; // don't run rest of attack code
        }
        // handle buffered attack, if any SECOND. buffered dashes have priority over any buffered attacks.
        if (canAttackAgain && _attackBuffered)
        {
            // tell animator to do another one
            EnterState();
        }
        
        // handle weapon information as well as updates that may have occured in animation (like updating vars in
        //     weapon code.)
        if (attackAnimationFinished || walkCancel)  // TODO: walk animation cancel code should be in the same spot as walk state change!
        {
            _player._animator.SetBool(_player.walkCancelHash, true);
            TransitionState(_player.runningState);
        }

        Quaternion currentRotation = _player.transform.rotation;
        _player.transform.rotation = Quaternion.Slerp(currentRotation, initialRotation, 
                _player.rotationsPerSecond * Time.deltaTime * _rotationFactor);;

            // handle movement (but slower)
        Vector3 worldMoveDirection = _player.currentMovement;
        _player._characterController.Move(_moveFactor * Time.deltaTime *
                                         _player.speed * worldMoveDirection);
    }

    public void SetAttackState(float moveFactor, float rotationFactor, Quaternion rotation)
    {
        _moveFactor = moveFactor;
        _rotationFactor = rotationFactor;
        initialRotation = rotation;
    }
    
    public override void OnAttack()
    {
        // To determine (should interact with weapon and its "can attack again/can dash rn variable"
        // set attack trigger
        _attackBuffered = true;
    }
}
