using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WaterGun : AbilityState
{

    public Tween delayTween;
    public WaterGun(PlayerController player) : base(player){}
    private float _moveFactor = 0.5f;
    private float _rotationFactor = 0.0f;
    private Quaternion initialRotation = Quaternion.identity;
    private ParticleSystem water;

    public override void EnterState()
    {
        //_player._animator.SetBool("ultraRun", true);
        //GameObject fire = GameObject.Instantiate(_player.firePrefab, _player.transform.position + Vector3.up * 1f, Quaternion.identity);
        //DOVirtual.DelayedCall(1f, ()=> fire.GetComponent<FireTrail>().Destroy());
        //_player._characterController.Move(Vector3.Normalize(_player.transform.forward));
        //_player.firePrefab.GetComponent<FireTrail>().Create(_player.transform.position, _player.firePrefab);
        delayTween = DOVirtual.DelayedCall(4f, ()=> TransitionState(_player.runningState), false);
        //_player.waterParticle.SetActive(true);
        _player.waterParticle.SetActive(true);
        water = _player.waterParticle.GetComponent<ParticleSystem>();
        //DOVirtual.DelayedCall(2f, ()=> FireTrail.Destroy(fire)).OnComplete(()=>TransitionState(_player.runningState));
        //DOVirtual.DelayedCall(0.1f, ()=> spawn(), false).OnComplete(()=>DOVirtual.DelayedCall(0.1f, ()=> spawn(), false));
        //Spawn();

        //_player._animator.SetFloat("ultraSpeed", _player.currentMovement.magnitude);
    }

    public override void ExitState()
    {
        //_player.waterParticle.SetActive(false);
        _player._animator.SetBool("waterGun", false);
        if(water.isEmitting)
        {
            water.Stop();
        }
        delayTween?.Kill();
    }

    public override void Execute()
    {
        Quaternion currentRotation = _player.transform.rotation;
        Vector3 worldMoveDirection = _player.currentMovement;
        Debug.Log(water.isEmitting);

        _player._characterController.Move(Time.deltaTime * _player.speed * worldMoveDirection);
        if (_player.isWaterGunning)
        {    
            // handle movement (but slower)
            worldMoveDirection = _player.currentMovement;
            _player._characterController.Move(_moveFactor * Time.deltaTime *
                                            _player.speed * worldMoveDirection);
            //_player.waterParticle.SetActive(true);
            if (!water.isEmitting){
                water.Play();
            }
            _player._animator.SetBool("waterGun", true);
            
            // _player.transform.rotation = Quaternion.Slerp(currentRotation, initialRotation, 
            //         _player.rotationsPerSecond * Time.deltaTime * _rotationFactor);
        }
        else
        {
    
            _player._animator.SetBool("waterGun", false);
            if (worldMoveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(worldMoveDirection);
                _player.transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, 
                    _player.rotationsPerSecond * Time.deltaTime);;
            }
            if (water.isEmitting)
            {
                water.Stop();
            }
        }

    }

    public override void SetWaterGunState(float moveFactor, float rotationFactor, Quaternion rotation)
    {
        _moveFactor = moveFactor;
        _rotationFactor = rotationFactor;
        initialRotation = rotation;
    }


    // public override void OnAbilityStarted(AbilityState ability)
    // {
    //     _player._pl
    //     _player._playerInput.CharacterControls.Melee.started += OnMelee;
    //     _playerInput.CharacterControls.ClickMelee.started += OnClickMelee;
    // }
    // public override void OnDash()
    // {
    //     TransitionState(_player.dashState);
    // }

    // public override void OnAttack()
    // {
    //     if (_player.attackState != null)
    //     {
    //         TransitionState(_player.attackState);
    //     }
    // }

}
