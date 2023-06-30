using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collided with " + other.name);
        GetComponentInParent<Tutorial>().StartAttackTutorial();
        Destroy(this);
    }
}
