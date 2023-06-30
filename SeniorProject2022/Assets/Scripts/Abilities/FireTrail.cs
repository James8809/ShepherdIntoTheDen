using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FireTrail : MonoBehaviour
{
    private float timeToLive = 4f;
    public Weapon weapon;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<EnemyManager>() != null)
        {
            other.GetComponent<EnemyManager>().ApplyBurn(2,GetComponentInChildren<Weapon>());
        }

        if(other.GetComponent<Enemy.EnemyAgent>() != null)
        {
            other.GetComponent<Enemy.EnemyAgent>().ApplyBurn(2,GetComponentInChildren<Weapon>());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<EnemyManager>() != null)
        {
            other.GetComponent<EnemyManager>().ApplyBurn(2,GetComponentInChildren<Weapon>());
        }
        if(other.GetComponent<Enemy.EnemyAgent>() != null)
        {
            other.GetComponent<Enemy.EnemyAgent>().ApplyBurn(2,GetComponentInChildren<Weapon>());
        }
    }

    public void Create(Vector3 playerTrans, GameObject prefab)
    {
        var fire = GameObject.Instantiate(prefab);
        fire.transform.position = playerTrans;
        DOVirtual.DelayedCall(timeToLive,()=> Destroy(fire), false);
    }
}
