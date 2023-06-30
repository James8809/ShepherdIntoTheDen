using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Accessibility;

[CreateAssetMenu(menuName = "Weapon", fileName = "Weapon", order = 2)]
public class WeaponObject : ScriptableObject
{
    public float knockback = .3f;
    public int damage = 20;
    public int manaCost = 0;

    private float multiplier = 1;
    
    // should add some sort of weapontype for animation data + other things like projectiles
    public void OnHit()
    {
        //hitObj.
    }

    public void SetMultiplier(float mult)
    {
        Debug.Log(mult);
        multiplier = mult;
    }

    public void ResetMultiplier() { multiplier = 1; }

    public int GetDamage()
    {
        return Mathf.RoundToInt(damage * multiplier);
    }
}
