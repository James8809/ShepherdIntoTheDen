using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DashState : PlayerState
{
    private float timeSinceStart = 0.0f;
    private Vector3 initialDashDirection;
    private Vector3 _maxRotatedVectorC, _maxRotatedVectorCC;
    private float _lowestDot;
    private bool _lastValidDirectionCC = false;
    
    public DashState(PlayerController player) : base(player) {}

    public override void EnterState()
    {
        _player._animator.SetBool(_player.isDashingHash, true);
        SetRotationToInput();
        timeSinceStart = 0.0f;
        
        initialDashDirection = Vector3.Normalize(_player.transform.forward);

        _maxRotatedVectorC = Quaternion.Euler(0, -_player.maxTotalDashYRotation, 0) * initialDashDirection;
        _maxRotatedVectorCC = Quaternion.Euler(0, _player.maxTotalDashYRotation, 0) * initialDashDirection;
        _lowestDot = Vector3.Dot(initialDashDirection, _maxRotatedVectorC);

        _player.hurtBox.enabled = false;
        DOVirtual.DelayedCall(_player.dashDuration * 1.3f, () => _player.hurtBox.enabled = true, false);
    }
    
    public override void ExitState()
    {
        _player._animator.SetBool(_player.isDashingHash, false);
    }

    /*
     * Returns the total distance the _player should have travelled given its current time as a form
     * of progress through the dash
     * 
     * use any of the easing functions in EasingFunction.cs 
     */
    float GetCurrentDashDistance(float timeElapsed)
    {
        //return Mathf.SmoothStep(0, dashDistance, timeElapsed / dashDuration);
        //return EasingFunction.EaseInQuad(0, _player.dashDistance, timeElapsed / _player.dashDuration);
        // return EasingFunction.EaseOutQuad(0, _player.dashDistance, timeElapsed / _player.dashDuration);
        return EasingFunction.EaseInSine(0, _player.dashDistance, timeElapsed / _player.dashDuration);
    }

    public override void Execute()
    {
        float newTimeSinceStart = timeSinceStart + Time.deltaTime;

        Quaternion currentRotation = _player.transform.rotation;
        Vector3 worldMoveDirection = _player.currentMovement;
        // allow _player processing input

        // add clamping to rotation to prevent insane turning
        if (worldMoveDirection != Vector3.zero)
        {
            Quaternion toRotate;
            if (Vector3.Dot(initialDashDirection, worldMoveDirection) > _lowestDot)
            {
                // acceptable input angle
                toRotate = Quaternion.LookRotation(worldMoveDirection);
                _lastValidDirectionCC = Vector3.Cross(initialDashDirection, worldMoveDirection).y > 0;
            }
            else
            {
                // input goes beyond maximum angle, cap at furthest vector normalized in that direction
                if (_lastValidDirectionCC)
                {
                    // counter clockwise cap
                    toRotate = Quaternion.LookRotation(_maxRotatedVectorCC);
                }
                else
                {
                    toRotate = Quaternion.LookRotation(_maxRotatedVectorC);
                }
            }
            // this code decreases control over the course of the roll to prevent sped up weirdness with slerping
            _player.transform.rotation = Quaternion.Slerp(currentRotation, toRotate,
                _player.dashSlerpFactor * (1 - newTimeSinceStart / _player.dashDuration));
        }
        
        // I want some sort of exponential distance based on time but also framerate indepenent
        // distance doubles every frame and dash lasts 7 frames (14 frames at 60 fps).

        Vector3 normalizedForward = Vector3.Normalize(_player.transform.forward);
        float totalDistance = GetCurrentDashDistance(newTimeSinceStart);
        float lastDistanceTravelled = GetCurrentDashDistance(timeSinceStart);
        float distanceThisFrame = totalDistance - lastDistanceTravelled;
        _player._characterController.Move(distanceThisFrame * _player.dashDistance *
                                         Vector3.Normalize(normalizedForward) * 
                                         Mathf.Clamp(Mathf.Clamp(Vector3.Dot(initialDashDirection,
                                             normalizedForward), 0.0f, 1.0f),.8f, 1.0f));
        
        timeSinceStart = newTimeSinceStart;
        if (timeSinceStart >= _player.dashDuration)
        {
            ExitDash();
            _player.timeSinceLastDash = 0.0f;    // reset cooldown on _player
        }
    }

    private void ExitDash()
    {
        TransitionState(_player.runningState);
    }
}
