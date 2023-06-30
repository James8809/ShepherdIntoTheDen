using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;
using Enemy;

public class ResourceDropper : MonoBehaviour
{
    private float hitStrength = 110; //80
    public int numResourcesSpawned;
    
    [System.Serializable]
    public class ResourceData
    {
        public GameObject resourcePrefab;
        public int percentage;
    }

    public List<ResourceData> resourcePrefabs;

    public void Start()
    {
        // sort list
        resourcePrefabs = resourcePrefabs.OrderBy(o => o.percentage).ToList();
    }

    protected void SpawnResources()
    {
        for (int i = 0; i < numResourcesSpawned; i++)
        {
            SpawnResource();
        }
    }

    private void SpawnResource()
    {
        int r = Random.Range(0, 100);
        int curPercentage = 0;
        for (int i = 0; i < resourcePrefabs.Count; i++)
        {
            var p = resourcePrefabs[i].percentage;
            var resourcePrefab = resourcePrefabs[i].resourcePrefab;
            curPercentage += p;
            if (r > curPercentage)
                continue;
            // Spawn 
            var euler = Quaternion.identity; // Random y rotation
            euler.y = Random.Range(0f, 1f);
            var resource = Instantiate(resourcePrefab, transform.position, euler);
            resource.transform.rotation = euler;
            resource.GetComponent<Rigidbody>()
                .AddForce(new Vector3(UnityEngine.Random.Range(-1f, 1f), 
                    2f, UnityEngine.Random.Range(-1f, 1f)) * hitStrength);
            return;
        }
    }
}
