using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityState : PlayerState
{
    // common ability variables.
    // I am thinking some sort of ID or key into a dict/list?
    public AbilityState(PlayerController player) : base(player) {}

    
    [HideInInspector] public AbilityState stompAbility;
    [HideInInspector] public AbilityState summonAbility;
    [HideInInspector] public AbilityState trailAbility;
    [HideInInspector] public AbilityState ultraRunAbility;
    [HideInInspector] public AbilityState waterGunAbility;

    private Dictionary<AbilityState, bool> abilityDic = new Dictionary<AbilityState, bool>();

    public void InitializeAbility()
    {
        stompAbility = new Stomp(_player);
        trailAbility = new Trail(_player);
        ultraRunAbility = new UltraRun(_player);
        waterGunAbility = new WaterGun(_player);
        summonAbility = new Summon(_player, _player.herdController.sheepPrefab);
        abilityDic.Add(stompAbility, false);
        abilityDic.Add(trailAbility, false);
        abilityDic.Add(ultraRunAbility, false);
        abilityDic.Add(waterGunAbility, false);
        abilityDic.Add(summonAbility, false);
    }
    public virtual void SetWaterGunState (float moveFactor, float rotationFactor, Quaternion rotation)
    {
    }
    // Abilities

    void OnStompAbilityStarted(InputAction.CallbackContext context)
    {
        _player.currentState.OnAbilityStarted(stompAbility);
    }

    void OnStompAbilityEnded(InputAction.CallbackContext context)
    {
        _player.currentState.OnAbilityEnded();
    }

    void OnRunAbilityStarted(InputAction.CallbackContext context)
    {
        _player.currentState.OnAbilityStarted(ultraRunAbility);
    }
    void OnRunAbilityEnded(InputAction.CallbackContext context)
    {
        _player.currentState.OnAbilityEnded();
    }
    void OnWaterGunAbilityStarted(InputAction.CallbackContext context)
    {
        // Debug.Log("water starteddddddddddd");
        // _playerInput.CharacterControls.Melee.started -= OnMelee;
        // _playerInput.CharacterControls.ClickMelee.started -= OnClickMelee;
        _player.currentState.OnAbilityStarted(waterGunAbility);
    }
    void OnWaterGunAbilityEnded(InputAction.CallbackContext context)
    {
        // Debug.Log("water finished");
        // _playerInput.CharacterControls.Melee.started += OnMelee;
        // _playerInput.CharacterControls.ClickMelee.started += OnClickMelee;
        _player.currentState.OnAbilityEnded();
    }
    
    void OnSummonAbilityStarted(InputAction.CallbackContext context)
    {
        _player.currentState.OnAbilityStarted(summonAbility);
    }

    void OnSummonAbilityEnded(InputAction.CallbackContext context)
    {
        _player.currentState.OnAbilityEnded();
    }
    // void OnCommandHerd(InputAction.CallbackContext context)
    // {
    //     if (herdController == null)
    //     {
    //         Debug.Log("Herd Controller Component is missing, cannot command herd.");
    //         return;
    //     }
    //     herdController.StartCharge();
    // }

    
    void OnFireTrailAbilityStarted(InputAction.CallbackContext context)
    {
        _player.currentState.OnAbilityStarted(trailAbility);
    }

    void OnFireTrailAbilityEnded(InputAction.CallbackContext context)
    {
        _player.currentState.OnAbilityEnded();
    }
    void OnBombThrowStarted(InputAction.CallbackContext context)
    {
        _player.currentState.OnAbilityStarted(trailAbility);
    }

    void OnBombThrowEnded(InputAction.CallbackContext context)
    {
        _player.currentState.OnAbilityEnded();
    }
    void OnDaggerThrowStarted(InputAction.CallbackContext context)
    {
        _player.currentState.OnAbilityStarted(trailAbility);
    }

    void OnDaggerThrowEnded(InputAction.CallbackContext context)
    {
        _player.currentState.OnAbilityEnded();
    }
    void OnWaterGunAttackReleased(InputAction.CallbackContext context)
    {
        _player.isWaterGunning = false;
    }

    void OnWaterGunAttackHold(InputAction.CallbackContext context)
    {
        // _player.isWaterGunning = true;
        // waterGunAbility.SetWaterGunState(_player.meleeMoveFactor, _player.meleeRotationFactor,
        //     Quaternion.LookRotation(_player.DirectionToMouseFromPlayer()));
    }

    // callbacks

    public void keyMapEnable(string actionName, string abilityName)
    {
        var action = _player._playerInput.FindAction(actionName);
        Debug.Log("enable ability on ability state script, " + actionName + ", " + abilityName);
        switch (abilityName)
        {
            case "stomp":
                EnableStompAbility(action);
                break;
            case "summon":
                EnableSummonAbility(action);
                break;
            case "herd":
                EnableSummonAbility(action);
                break;
            case "firetrail":
                EnableFireTrailAbility(action);
                break;
            case "ultrarun":
                Debug.Log("enable super dash ability on ability state script, " + actionName + ", " + abilityName);
                EnableSuperDashAbility(action);
                break;
            case "throwbomb":
                EnableBombThrow(action);
                break;
            case "throwdagger":
                EnableDaggerThrow(action);
                break;
            default:
                break;
        }
    }

    public void keyMapDisable(string actionName, string abilityName)
    {
        var action = _player._playerInput.FindAction(actionName);
        switch (abilityName)
        {
            case "stomp":
                DisableStompAbility(action);
                break;
            case "summon":
                DisableSummonAbility(action);
                break;
            case "herd":
                DisableSummonAbility(action);
                break;
            case "firetrail":
                DisableFireTrailAbility(action);
                break;
            case "superdash":
                DisableSuperDashAbility(action);
                break;
            case "throwbomb":
                DisableBombThrow(action);
                break;
            case "throwdagger":
                DisableDaggerThrow(action);
                break;
            default:
                break;
        }
    }




    public void EnableStompAbility(InputAction action)
    {
        action.started += OnStompAbilityStarted;
        action.canceled += OnStompAbilityEnded;
    }

    public void DisableStompAbility(InputAction action)
    {
        action.started -= OnStompAbilityStarted;
        action.canceled -= OnStompAbilityEnded;
    }
    
    public void EnableSummonAbility(InputAction action)
    {
        Debug.Log("enablinb summoner abaility");
        action.started += OnSummonAbilityStarted;
        action.canceled += OnSummonAbilityEnded;
    }

    public void DisableSummonAbility(InputAction action)
    {
        action.started -= OnSummonAbilityStarted;
        action.canceled -= OnSummonAbilityEnded;
    }
    
    public void EnableFireTrailAbility(InputAction action)
    {
        action.started += OnFireTrailAbilityStarted;
        action.canceled += OnFireTrailAbilityEnded;
    }

    public void DisableFireTrailAbility(InputAction action)
    {
        action.started -= OnFireTrailAbilityStarted;
        action.canceled -= OnFireTrailAbilityEnded;
    }
    public void EnableSuperDashAbility(InputAction action)
    {
        action.started += OnRunAbilityStarted;
        action.canceled += OnRunAbilityEnded;
    }

    public void DisableSuperDashAbility(InputAction action)
    {
        action.started -= OnRunAbilityStarted;
        action.canceled -= OnRunAbilityEnded;
    }

    // for potentially changing bomb and dagger key bind
    public void EnableBombThrow(InputAction action)
    {
        action.started += OnRunAbilityStarted;
        action.canceled += OnRunAbilityEnded;
    }

    public void DisableBombThrow(InputAction action)
    {
        action.started -= OnRunAbilityStarted;
        action.canceled -= OnRunAbilityEnded;
    }

    public void EnableDaggerThrow(InputAction action)
    {
        action.started += OnRunAbilityStarted;
        action.canceled += OnRunAbilityEnded;
    }

    public void DisableDaggerThrow(InputAction action)
    {
        action.started -= OnRunAbilityStarted;
        action.canceled -= OnRunAbilityEnded;
    }


    void OnCommandHerd(InputAction.CallbackContext context)
    {
        if (_player.herdController == null)
        {
            Debug.Log("Herd Controller Component is missing, cannot command herd.");
            return;
        }
        _player.herdController.StartCharge();
    }



}
