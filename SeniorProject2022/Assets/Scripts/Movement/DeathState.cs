using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : PlayerState
{
    public DeathState(PlayerController player) : base(player) {}

    public override void EnterState()
    {
        _player._animator.SetBool("isDead", true);
        _player.ReferenceManager.uiManager.EnableDeathUI();
    }
    
    public override void ExitState()
    {
        // reset the players animation back to idle
        _player._animator.Rebind();
        _player._animator.Update(0f);
        _player._animator.SetBool("isDead", false);
        _player.ReferenceManager.uiManager.DisableDeathUI();
    }
}
