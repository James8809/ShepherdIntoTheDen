using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using UnityEngine;
using DG.Tweening;
using UnityEngine.VFX;

public class Summon : AbilityState
{
    public Summon(PlayerController player, GameObject sheepPrefab) : base(player)
    {
        this.sheepPrefab = sheepPrefab;
    }
    private int manaCost;
    private GameObject sheepPrefab;
    
    public override void EnterState()
    {
        
        if (_player.herdController == null)
        {
            Debug.Log("Herd Controller Component is missing, cannot command herd.");
            return;
        }
        if (_player.herdController.herdSize < 0 || _player.herdController.isCharging)
        {
            Debug.Log("not finish");
            TransitionState(_player.runningState);
            return;
        }


        //Use mana
        var weapon = Resources.Load<WeaponObject>("Weapons/Summon");
        manaCost = weapon.manaCost;
        if (!_player.playerManaSystem.UseMana(manaCost)) // if not enough mana
        {    
            TransitionState(_player.runningState);
            return;
        }
        //_player.herdController.MoveSheepBack(_player.transform.position + new Vector3(Random.Range(1, 2), 0, Random.Range(1, 2)));
        _player.herdController.StartCharge(_player.WorldPositionToMouseFromPlayer()); 
    }

    public override void ExitState()
    {
       
    }

    public override void Execute()
    {
        TransitionState(_player.runningState);
    }

    public void SummonSheep()
    {

    }

    public void SetSheepCharge()
    {

    }
}
