using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepChargeDamage : MonoBehaviour
{
    public WeaponObject weaponType;
    public SheepController controller;
    bool canDamage = true;
    public float damageBuffer = 1f;

    
    private void OnTriggerEnter(Collider other)
    {
        controller.controller.ChargeHasHitEnemy();
        /*
        if (other.CompareTag("Enemy"))
        {
            EnemyAgent agent = other.GetComponent<EnemyAgent>();
            if (controller.charging)
            {
                if (canDamage)
                {
                    // TODO: replace sheep attacking code with new system
                    // agent.TakeDamage(weaponType.knockback * Vector3.Normalize(transform.forward), weaponType.damage);
                    StartCoroutine(StopDamageForSeconds(damageBuffer));
                    controller.controller.ChargeHasHitEnemy();
                }
            }
            else
            {
                controller.TakeDamage(1);
            }
        }*/
    }


    IEnumerator StopDamageForSeconds(float sec)
    {
        canDamage = false;
        yield return new WaitForSeconds(sec);
        canDamage = true;
    }
}
