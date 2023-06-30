using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    HerdController herdController;
    List<Transform> nearbyEnemies = new List<Transform>();
    public Vector3 safeDir;
    int numEnemies;

    // Start is called before the first frame update
    void Start()
    {
        herdController = GetComponentInParent<HerdController>();
    }

    public bool EnemiesPresent()
    {
        return nearbyEnemies.Count > 0;
    }

    public Vector3 GetSafeDirection()
    {
        return safeDir;
    }

    // Update is called once per frame
    void Update()
    {
        if (!EnemiesPresent())
            return;
        safeDir = Vector3.zero;
        foreach (Transform t in nearbyEnemies)
        {
            if (t == null)
            {
                nearbyEnemies.Remove(t);
                break;
            }
            safeDir += transform.position - t.position;
        }
        safeDir *= -1;
        safeDir.Normalize();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            nearbyEnemies.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (nearbyEnemies.Contains(other.transform))
            {
                nearbyEnemies.Remove(other.transform);
            }
            //Debug.Log(other.gameObject.name + " leaving detection radius");
        }
    }
}