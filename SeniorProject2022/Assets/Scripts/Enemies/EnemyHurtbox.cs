using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyHurtbox : MonoBehaviour
{
    private IDamageReciever _damageReciever;

    private void Awake()
    {
        _damageReciever = GetComponentInParent<IDamageReciever>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // only weapons that can collide with this system should be registered
        var weapon = other.GetComponent<Weapon>();
        Debug.Log(weapon.weaponType);
        if (weapon == null)
        {
            //Debug.Log("Hurtbox collided with non-weapon object. Check the layer" + other.name);
            return;
        }
        _damageReciever.TakeDamage(weapon);
    }
}
