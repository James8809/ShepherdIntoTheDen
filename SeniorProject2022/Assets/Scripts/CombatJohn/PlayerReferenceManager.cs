using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerReferenceObject", fileName = "PlayerReferenceObject", order = 3)]
public class PlayerReferenceManager : ScriptableObject
{
    private CombatEffectManager _effectManager;
    public ManagerUI uiManager;
    
    public CombatEffectManager EffectManager
    {
        get => _effectManager;
        set => _effectManager = value;
    }
}
