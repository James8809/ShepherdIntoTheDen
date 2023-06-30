using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.VFX;

public class Stomp : AbilityState
{
    public float cooldown = 3.0f;
    public Stomp(PlayerController player) : base(player){}
    private const float _jumpHeight = 1.0f;
    private const float _jumpDist = 4.0f;
    private Vector3 _initForward;
    private GameObject collider;

    private bool startForward = false;
    private float oldAnimSpeed;
    private int manaCost;
    
    public override void EnterState()
    {
        collider = _player.abilityCollider;
        oldAnimSpeed = _player._animator.speed;
        _player._animator.SetTrigger("stompTrigger");
        collider.GetComponent<Weapon>().weaponType = Resources.Load<WeaponObject>("Weapons/Stomp");
        manaCost = collider.GetComponent<Weapon>().weaponType.manaCost;
        _player.playerManaSystem.UseMana(manaCost);
        _initForward = Vector3.Normalize(_player.transform.forward); // + Vector3.up * 10;
        _initForward.y = 0;
        // set positive y velocity
        // TODO: replace this with code that uses _player.CharacterController.Move() in Execute!!!
        //moveTween = DOVirtual.DelayedCall(.4f, () =>
        //   _player.transform.DOJump(_initForward * _jumpDist + _player.transform.position,
        //   _jumpHeight, 1, _duration, false).OnComplete(() => TransitionState(_player.runningState)));
        //_player._characterController.Move(_initForward * 100f * Time.deltaTime);
        //target = Vector3.SmoothDamp(_player.transform.position, _player.transform.position + _initForward * _jumpDist,ref _initForward, 4f);
        //_player._characterController.Move(Vector3.down * _player.downwardsMovement * Time.deltaTime);
        // use like -10 in downwards movement
        //_player._characterController.Move(velocity);
        //_player._characterController.Move(_initForward * Time.deltaTime * 10.0f);
        //DOVirtual.DelayedCall(0.4f, () => _player._characterController.Move(_initForward * Time.deltaTime * 10.0f), false);

        // the exit time
        DOVirtual.DelayedCall(1.4f, () => TransitionState(_player.runningState), false);

        // handle forward
        DOVirtual.DelayedCall(0.4f, ()=> startForward = true, false);
        DOVirtual.DelayedCall(1.2f, ()=> startForward = false, false);

        //handle upward movement
        _player.gravity = 26F;
        DOVirtual.DelayedCall(0.4f, ()=> _player._characterController.Move(Vector3.up * Time.deltaTime), false);
        DOVirtual.DelayedCall(0.4f, ()=> _player.downwardsMovement = -10f, false);

        //change animator speed;
        DOVirtual.DelayedCall(0.8f, ()=> _player._animator.speed = 0.7f, false);

        //play vfx
        Debug.Log(_player.transform.position);
        //_player.stompVfx.SetVector3("charPos", _player.transform.position);
        //DOVirtual.DelayedCall(1.2f, ()=> _player.stompVfx.SetVector3("charPos", _player.transform.position), false).OnComplete(()=>_player.stompVfx.Play());
        DOVirtual.DelayedCall(1.2f, ()=> _player.stompVfx.GetComponent<VisualEffect>().SetVector3("charPos", _player.transform.position + _initForward* 1.2F), false);//.OnComplete(()=>_player.stompVfx.Play());
        DOVirtual.DelayedCall(1.2f, ()=> _player.stompVfx.transform.position = _player.transform.position + _initForward* 1.2F);
        DOVirtual.DelayedCall(1.2F, ()=> _player.stompVfx.GetComponent<VisualEffect>().Play());

        Debug.Log(_player.GetComponent<CapsuleCollider>()==null);
    }

    public override void ExitState()
    {
        _player._animator.ResetTrigger("stompTrigger");
        _player.gravity = 9.82F;
        _player._animator.speed = oldAnimSpeed;
        startForward = false;
        //moveTween?.Kill();
    }

    public override void Execute()
    {
        // hard part: getting this to look good with the animation
        // changing the animation speed, and duration
        //target = Vector3.SmoothDamp(_initForward* Time.deltaTime, _initForward* Time.deltaTime * _jumpDist,ref velocity, 1.6f);
        //target = Vector3.SmoothDamp(_player.transform.position, _player.transform.position + _initForward * _jumpDist,ref velocity, 1.6f);
        //target = Vector3.SmoothDamp(_player.transform.position, _player.transform.position + _initForward * _jumpDist,ref velocity, 1.6f);
        if (startForward)
        {
            _player._characterController.Move(_initForward * Time.deltaTime * 5.0F);
        }
        
        //DOVirtual.DelayedCall(0.4f, () => _player._characterController.Move(_initForward * Time.deltaTime * 5.0f), false);
        //_player._characterController.Move(_initForward * Time.deltaTime * 5.0f);
        //_player._characterController.Move(velocity);
        //_player._animator.speed = 1.2f;
        // _player.CharacterController.Move(forward * Time.deltatime * speed)
        // mess with initial downwards movement values for getting height.
        // dont worry about particles for now
        // worry about its hitbox, modify the hitbox using the animation window
        // TODO: make a general ability collider that gets animated. Change out the weapon reference through code based on ability
        // work on ability animation transitions
        // TODO: have a wait timer for a few-tenths of a second. after the wait timer, then start your jumping forwards and upwards movement
        //  DOVirtual.DelayedCall(;asdfkjasd).OnComplete();
        
        // switch over to character controller instead of transform. Must do to make patch not go through walls.
        // maybe look into kinematic body controller for Patch. might make movement easier in the future.
        // https://docs.unity3d.com/ScriptReference/CharacterController.html
    }
}
