using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HerdController : MonoBehaviour
{
    public float followStopDistance;
    [SerializeField] GameObject destinationPrefab;
    public int herdSize;
    public GameObject sheepPrefab;
    float runAwayMultiplier = 3;
    float scareCooldown = 1;
    [SerializeField] int ScanRayCount;
    [SerializeField] float ScanRayThickness;
    [SerializeField] float ScanRayAngle;
    public float chargeSpeed;
    public float moveSpeed;
    public float maxChargeRange;
    public float maxChargeTime = 5f;
    public LayerMask enemyLayer;
    public LayerMask environmentLayer;
    PlayerController player;
    List<SheepController> herd;
    [HideInInspector] public bool scared;
    [HideInInspector] public bool isFollowing;
    [HideInInspector] public bool isCharging;
    float currCooldown;
    Transform EnemyTarget;
    Vector3 FinalChargeDestination;
    GameObject destinationInstance;
    int trackedSheepIndex;
    //EnemyDetection enemyDetection;
    bool quitting;
    Tutorial tutorial;

    // DEBUGGING
    Vector3 debugDest = new Vector3(-2, 0, 2);

    // Start is called before the first frame update
    void Start()
    {
        herd = new List<SheepController>();
        player = GetComponent<PlayerController>();
        //int prevHerdSize = PlayerPrefs.GetInt("herdSize");
        int prevHerdSize = 1;

        NavMeshHit myNavHit;
        Vector3 herdSpawnPoint = Vector3.zero;
        if (NavMesh.SamplePosition(transform.position, out myNavHit, 100, -1))
        {
            herdSpawnPoint = myNavHit.position;
        }
        tutorial = FindObjectOfType<Tutorial>();
        /*
        for (int i = 0; i < prevHerdSize; i++)
        {
            herd.Add(Instantiate(sheepPrefab, herdSpawnPoint, Quaternion.identity).GetComponent<SheepController>());
            herd[herd.Count - 1].controller = this;
            herd[herd.Count - 1].EnablePickup();
        }
        herdSize = prevHerdSize;
        /*
        for (int i = 0; i < herdSize; i++)
        {
            herd.Add(Instantiate(sheepPrefab, spawnPoint).GetComponent<SheepController>());
            herd[herd.Count - 1].gameObject.transform.position = new Vector3(spawnPoint.position.x + i* spawnSpacing, spawnPoint.position.y, spawnPoint.position.z);
            herd[herd.Count - 1].controller = this;
        }
        */
        //SetSafeZone(spawnPoint.position);
    }


    public void StartCharge()
    {
        if (herdSize == 0 || isCharging)
        {
            return;
        }

        EnemyTarget = ScanForEnemy(Vector3.zero);
        Vector3 chargeDirection = transform.forward;
        //FinalChargeDestination = FindChargeDestination();
        isCharging = true;
        SetFollowing(false);
        // begin tracking the closest sheep to the player
        trackedSheepIndex = herd.IndexOf(ClosestSheepToPoint(transform.position));

        if (EnemyTarget != null)
        {
            NavMeshHit myNavHit;
            if (NavMesh.SamplePosition(EnemyTarget.position, out myNavHit, 100, -1))
            {
                //destinationInstance = Instantiate(destinationPrefab, myNavHit.position, Quaternion.identity);
                foreach (SheepController sheep in herd)
                {
                    sheep.Charge(myNavHit.position);
                }
            }
        }
        else
        {
            foreach (SheepController sheep in herd)
            {
                sheep.ChargeWithoutTarget(transform.forward);
            }
        }

        DOVirtual.DelayedCall(maxChargeTime, () =>
            ReachedDestination()
        , false);
    }

    public void StartCharge(Vector3 destination)
    {
        if (herdSize == 0 || isCharging)
        {
            return;
        }

        EnemyTarget = ScanForEnemy(destination);
        Vector3 chargeDirection = transform.forward;
        //FinalChargeDestination = FindChargeDestination();
        isCharging = true;
        SetFollowing(false);
        // begin tracking the closest sheep to the player
        trackedSheepIndex = herd.IndexOf(ClosestSheepToPoint(transform.position));

        if (EnemyTarget != null)
        {
            NavMeshHit myNavHit;
            if (NavMesh.SamplePosition(EnemyTarget.position, out myNavHit, 100, -1))
            {
                //destinationInstance = Instantiate(destinationPrefab, myNavHit.position, Quaternion.identity);
                foreach (SheepController sheep in herd)
                {
                    sheep.Charge(myNavHit.position);
                }
            }
        }
        else
        {
            foreach (SheepController sheep in herd)
            {
                sheep.ChargeWithoutTarget(transform.forward);
            }
        }

        DOVirtual.DelayedCall(maxChargeTime, () =>
            ReachedDestination()
        , false);
    }

    // OLD START CHARGE METHOD
    /*
    public void StartCharge()
    {
        if (herdSize == 0 || isCharging)
        {
            return;
        }

        EnemyTarget = ScanForEnemy();
        FinalChargeDestination = FindChargeDestination();

        NavMeshHit myNavHit;
        if (NavMesh.SamplePosition(FinalChargeDestination, out myNavHit, 100, -1))
        {
            isCharging = true;
            SetFollowing(false);
            destinationInstance = Instantiate(destinationPrefab, myNavHit.position, Quaternion.identity);
            foreach (SheepController sheep in herd)
            {
                sheep.Charge(myNavHit.position);
            }
            destinationInstance = Instantiate(destinationPrefab, myNavHit.position, Quaternion.identity);
            
            // begin tracking the closest sheep to the player
            trackedSheepIndex = herd.IndexOf(ClosestSheepToPoint(transform.position));
        }
        else
        {
            Debug.Log("Failed to find adequate charge destination on NavMesh");
        }
        
    }
    */
    Transform ScanForEnemy(Vector3 dest)
    {
        Transform aimTransform = transform;
        if (dest != Vector3.zero)
        {
            aimTransform.LookAt(dest);
            Vector3 eulerRotation = aimTransform.rotation.eulerAngles;
            aimTransform.rotation = Quaternion.Euler(0, eulerRotation.y, eulerRotation.z);
        }
        Transform enemyPos = null;
        Vector3 closestHit = Vector3.positiveInfinity;
        Vector3 origin = aimTransform.position;
        RaycastHit hit;
        float subScanRayAngle = ScanRayAngle / (ScanRayCount - 1);
        // Lefthand rays
        for (int i = 0; i < Mathf.Floor(ScanRayCount / 2); i++)
        {
            // calculate vector that corresponds to the desired angle
            float angle = (i - Mathf.Floor(ScanRayCount / 2)) * subScanRayAngle;
            Vector3 dir = Vector3.RotateTowards(aimTransform.TransformDirection(Vector3.forward), aimTransform.TransformDirection(Vector3.left), Mathf.Deg2Rad * angle, 1f);
            if (Physics.SphereCast(origin, ScanRayThickness, dir, out hit, 10f, enemyLayer.value))
            {
                if (EnemyIsCloser(closestHit, hit.point))
                {
                    enemyPos = hit.collider.transform;
                    closestHit = hit.point;
                }
                    
            }
        }

        //Center Ray
        if (Physics.SphereCast(origin, ScanRayThickness, aimTransform.TransformDirection(Vector3.forward), out hit, 10f, enemyLayer.value))
        {
            if (EnemyIsCloser(closestHit, hit.point))
            {
                enemyPos = hit.collider.transform;
                closestHit = hit.point;
            }
        }

        // RightHand rays
        for (int i = 0; i < Mathf.Floor(ScanRayCount / 2); i++)
        {
            // calculate vector that corresponds to the desired angle
            float angle = (i - Mathf.Floor(ScanRayCount / 2)) * subScanRayAngle;
            Vector3 dir = Vector3.RotateTowards(aimTransform.TransformDirection(Vector3.forward), aimTransform.TransformDirection(Vector3.right), Mathf.Deg2Rad * angle, 1f);
            if (Physics.SphereCast(origin, ScanRayThickness, dir, out hit, 10f, enemyLayer.value))
            {
                if (EnemyIsCloser(closestHit, hit.point))
                {
                    enemyPos = hit.collider.transform;
                    closestHit = hit.point;
                }
            }
        }
        //if (enemyPos)
        //    Debug.DrawLine(origin, enemyPos.position, Color.red);
        //Debug.DrawLine(origin, closestHit, Color.white);
        return enemyPos;
    }


    bool EnemyIsCloser(Vector3 oldEnemy, Vector3 newEnemy)
    {
        if (oldEnemy == null)
        {
            return true;
        }
        else if (Vector3.Distance(oldEnemy, transform.position) > Vector3.Distance(newEnemy, transform.position))
        {
            return true;
        }
        return false;
    }

    Vector3 FindChargeDestination()
    {
        RaycastHit hit;
        Vector3 offset = new Vector3(0f, 0.5f, 0f);
        Vector3 loc = transform.position;
        if (Physics.Raycast(transform.position + offset, transform.forward, out hit, 200f, environmentLayer.value))
        {
            loc = hit.point;
        }
        return loc;
    }

    public void SetSafeZone(Vector3 zone)
    {
        foreach (SheepController sheep in herd)
        {
            sheep.SetNewDestination(zone);
        }
    }

    public void SetFollowing(bool follow)
    {
        isFollowing = follow;
        foreach (SheepController sheep in herd)
        {
            // Increase stopping dist if the destination is the player
            sheep.SetStoppingDistance(isFollowing ? followStopDistance : 0.5f);
            //Debug.Log("New stoping dist: " + sheep.agent.stoppingDistance);
        }
    }

    public void ReachedDestination()
    {
        Debug.Log("reached destination");
        if (destinationInstance != null)
        {
            Destroy(destinationInstance);
            destinationInstance = null;
        }

        if (!isCharging)
        {
            return;
        }

        foreach (SheepController sheep in herd)
        {
            sheep.StopCharge();
        }
        isCharging = false;
        SetFollowing(true);
    }

    public void ChargeHasHitEnemy()
    {
        Debug.Log("hit enemy");
        EnemyTarget = null;
        if (isCharging)
        {
            foreach (SheepController sheep in herd)
            {
                sheep.ChargeWithoutTarget(transform.forward);
            }
        }
        //SetSafeZone(FinalChargeDestination);
    }

    // Update is called once per frame
    void Update()
    {
        // Do nothing if there is no herd
        if (herd.Count == 0)
        {
            return;
        }
        if (isCharging)
        {
            if (Vector3.Distance(herd[trackedSheepIndex].transform.position, transform.position) > maxChargeRange)
            {
                ReachedDestination();
            }
            
            if (EnemyTarget != null)
            {
                SetSafeZone(EnemyTarget.position);
            }
            return;
        }
        if (scared)
        {
            currCooldown += Time.deltaTime;
            if (currCooldown >= scareCooldown)
            {
                scared = false;
            }
        }
        if (isFollowing)
        {
            //Debug.Log("Enemies present: " + enemyDetection.EnemiesPresent());
            SetSafeZone(player.transform.position);
        }
    }

    public void AddSheep(SheepController sheep)
    {
        if (herdSize == 0)
        {
            if (tutorial != null)
            {
                StartCoroutine(tutorial.HerdingTutorial());
            }
        }
        herd.Add(sheep);
        herdSize++;

        // TEMP: AUTO-CHARGE WHEN SHEEP ADDED FOR TESTING
        
        /*
        DOVirtual.DelayedCall(3f, () =>
        {
            StartCharge(debugDest);
        }, false);

        DOVirtual.DelayedCall(5f, ()=> HideSheep(), false);
        */
    }

    // public void HideSheep()
    // {
    //     // This function is broken, not sure why it exists.
    //     // Call Destroy() on the sheep if you want to get rid of it.
    //     // - Stryker
    //     return;
        
    //     while(isCharging)
    //     {

    //     }
    //     foreach(SheepController sheep in herd)
    //     {
    //         sheep.agent.isStopped = true;
    //         sheep.gameObject.SetActive(false);
    //         //sheep.GetComponent<Transform>().position = new Vector3(100f,100f,100f);
    //     }
    // }

    // public void MoveSheepBack(Vector3 pos)
    // {
    //     foreach(SheepController sheep in herd)
    //     {
    //         sheep.gameObject.SetActive(true);
    //         sheep.agent.isStopped = false;
    //         //sheep.GetComponent<Transform>().position = pos;
    //     }
        
    //     DOVirtual.DelayedCall(5f, ()=> HideSheep(), false);
    // }

    public void RemoveSheep(SheepController sheep)
    {
        herd.Remove(sheep);
        herdSize--;
    }

    SheepController ClosestSheepToPoint(Vector3 point)
    {
        SheepController closestSheep = null;
        float currDist = 9999f;
        float dist;
        foreach (SheepController sheep in herd)
        {
            dist = Vector3.Distance(sheep.transform.position, point);
            if (dist <= currDist)
            {
                closestSheep = sheep;
                currDist = dist;
            }
        }
        return closestSheep;
    }


    public void DetectThreat(Vector3 threatPos)
    {
        if (isCharging)
        {
            return;
        }
        SetFollowing(false);
        if (!scared)
        {
            scared = true;
            currCooldown = 0f;
        }

        // Find closest sheep to threat
        SheepController closestSheep = null;
        float currDist = 9999f;
        float dist;
        foreach (SheepController sheep in herd)
        {
            dist = Vector3.Distance(sheep.transform.position, threatPos);
            if (dist <= currDist)
            {
                closestSheep = sheep;
                currDist = dist;
            }
        }

        Vector3 goal = runAwayMultiplier * (closestSheep.transform.position - threatPos);
        NavMeshHit myNavHit;
        if (NavMesh.SamplePosition(goal, out myNavHit, 100, -1))
        {
            SetSafeZone(myNavHit.position);
        }
    }

    private void OnDestroy()
    {
        if (!quitting)
            PlayerPrefs.SetInt("herdSize", herdSize);
    }

    void OnApplicationQuit()
    {
        quitting = true;
        PlayerPrefs.SetInt("herdSize", 0);
    }
}