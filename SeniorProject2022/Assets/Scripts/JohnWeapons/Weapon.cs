using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;

public class Weapon : MonoBehaviour
{
    public WeaponObject weaponType;
    public void SwitchWeapon(WeaponObject weaponObject){
        weaponType = weaponObject;
    }
}
