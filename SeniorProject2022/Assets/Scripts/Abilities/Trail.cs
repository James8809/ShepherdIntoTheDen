using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Trail : AbilityState
{
    public Tween moveTween;
    public Trail(PlayerController player) : base(player){}
    public float count;

    public override void EnterState()
    {
        var weapon = Resources.Load<WeaponObject>("Weapons/FireTrail");
        var manaCost = weapon.manaCost;
        if (!_player.playerManaSystem.UseMana(manaCost)) // if not enough mana
            TransitionState(_player.runningState);
        _player._animator.SetBool("trailing", true);
        //GameObject fire = GameObject.Instantiate(_player.firePrefab, _player.transform.position + Vector3.up * 1f, Quaternion.identity);
        //DOVirtual.DelayedCall(1f, ()=> fire.GetComponent<FireTrail>().Destroy());
        //_player._characterController.Move(Vector3.Normalize(_player.transform.forward));
        //_player.firePrefab.GetComponent<FireTrail>().Create(_player.transform.position, _player.firePrefab);
        DOVirtual.DelayedCall(0.3f, ()=> TransitionState(_player.runningState), false);
        //DOVirtual.DelayedCall(2f, ()=> FireTrail.Destroy(fire)).OnComplete(()=>TransitionState(_player.runningState));
        //DOVirtual.DelayedCall(0.1f, ()=> spawn(), false).OnComplete(()=>DOVirtual.DelayedCall(0.1f, ()=> spawn(), false));
        //Spawn();

    }

    public override void ExitState()
    {
        _player._animator.SetBool("trailing", false);
        moveTween?.Kill();
        count = 0;
    }

    public override void Execute()
    {
        //_player.firePrefab.GetComponent<FireTrail>().Create(_player.transform.position, _player.firePrefab);
        _player._characterController.Move(Vector3.Normalize(_player.transform.forward) * Time.deltaTime * 10f);
        if(count % 5 == 0){
            spawn();
        }
        count++;
    }

    public void spawn()
    {
        _player.firePrefab.GetComponent<FireTrail>().Create(_player.transform.position + Vector3.up* 0.5f, _player.firePrefab);
    }
}
