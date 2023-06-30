using UnityEngine;

public abstract class PlayerState
{
    protected PlayerController _player;
    protected PlayerState(PlayerController player)
    {
        _player = player;
    }
    
    // states should change themselves to current State by using enterState!
    // i.e. idle calls runState.EnterState(player) to change states
    public virtual void EnterState() {}
    public virtual void ExitState() {}
    // note, if this gets more complex, execute can be broken up into multiple methods called in Update()
    public virtual void Execute() {}
    public virtual void OnDash() {}
    public virtual void OnAttack() {}
    public virtual void OnAbilityStarted(AbilityState ability) {}
    public virtual void OnConsumableStarted() {}
    public virtual void OnAbilityEnded() {}
    public virtual void OnConsumableEnded() {}
    public virtual void OnConsumableTriggered() {}

    // might want to change this to ID? or nah since you can just pass it in (prob in a big list).
    public void TransitionState(PlayerState toState)
    {
        ExitState();
        // set state of player here.
        _player.currentState = toState;
        toState.EnterState();
    }

    protected void SetRotationToInput()
    {
        Vector3 lookdir;
        // if no input, assume player wants to travel in facing direction
        if (_player.currentMovement.sqrMagnitude != 0)
        {
            lookdir = Vector3.Normalize(_player.currentMovement);
        }
        else
        {
            lookdir = _player.transform.forward;
            lookdir.y = 0;
            lookdir = Vector3.Normalize(lookdir);
        }
        _player.transform.rotation = Quaternion.LookRotation(lookdir);
    }
}
