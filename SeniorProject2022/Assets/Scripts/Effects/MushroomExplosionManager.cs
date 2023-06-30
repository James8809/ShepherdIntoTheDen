using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MushroomExplosionManager : MonoBehaviour
{
    [SerializeField] private VisualEffect sparks;

    private CombatEffectManager _combatEffectManager;
    
    private void OnEnable()
    {
        sparks.Stop();
        _combatEffectManager = FindObjectOfType<CombatEffectManager>();
    }

    public void StartExplosion()
    {
        _combatEffectManager.ShakeCamera(3, 1);
        sparks.Play();
    }
}
