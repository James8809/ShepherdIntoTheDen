using System;
using Enemy;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class SheepController : MonoBehaviour
{
    public bool healingBoy;
    public int healAmount;
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] ParticleSystem runParticles;
    public HerdController controller;
    PlayerController pc = null;
    public float sensorLength = 0.5f;
    public SphereCollider damageBox;
    public bool charging;
    public bool chargingWithoutTarget;
    public int tempHealth;
    public GameObject sheepMesh;
    public MeshRenderer bodyMesh;
    public Collider detectionCollider;
    public Animator animator;
    float chargeDirMultiplier = 3f;
    Vector3 chargeDir;

    public static Action OnSheepCollected;

    // Stryker feel free to replace this bool its just 2 am lol
    private bool hasFoundPlayer = false;

    private Health health;

    // Start is called before the first frame update
    void Start()
    {
        damageBox.enabled = false;
        //agent = GetComponent<NavMeshAgent>();
        //agent.speed = controller.moveSpeed;
        tempHealth = 5;
        if (healingBoy)
        {
            SetBodyColor(new Color(0, 1, 0));
        }

        agent.autoTraverseOffMeshLink = false;
        health = GetComponent<Health>();
    }
    
    // handle off mesh link because off mesh links are dumb dumb
    void HandleOffMeshLinks()
    {
        if (agent.isOnOffMeshLink)
        {
            OffMeshLinkData data = agent.currentOffMeshLinkData;

            //calculate the final point of the link
            Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;

            //Move the agent to the end point
            agent.transform.position = Vector3.MoveTowards(agent.transform.position, endPos, agent.speed * Time.deltaTime);

            //when the agent reach the end point you should tell it, and the agent will "exit" the link and work normally after that
            if (agent.transform.position == endPos)
            {
                agent.CompleteOffMeshLink();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("speed", agent.velocity.magnitude);
        HandleOffMeshLinks();
        if (chargingWithoutTarget)
        {
            ContinueCharge(transform.forward);
        }
    }

    public void EnablePickup()
    {
        agent.enabled = true;
        detectionCollider.enabled = true;
    }

    public void SetNewDestination(Vector3 dest)
    {
        agent.SetDestination(dest);
        agent.isStopped = false;
    }

    public void Charge (Vector3 dest)
    {
        //Debug.Log(name + " is charging");
        agent.speed = controller.chargeSpeed;
        agent.SetDestination(dest);
        agent.isStopped = false;
        damageBox.enabled = true;
        runParticles.Play();
        charging = true;
    }

    public void ChargeWithoutTarget(Vector3 dir)
    {
        chargeDir = dir;
        chargingWithoutTarget = true;
        Charge(transform.position + chargeDir * chargeDirMultiplier);
    }

    void ContinueCharge(Vector3 dir)
    {
        agent.SetDestination(transform.position + chargeDir * chargeDirMultiplier);
    }

    public void StopCharge()
    {
        chargingWithoutTarget = false;
        agent.speed = controller.moveSpeed;
        runParticles.Stop();
        damageBox.enabled = false;
        charging = false;
    }

    public void Die()
    {
        if (controller != null)
        {
            controller.RemoveSheep(this);
            Destroy(gameObject);
        }
        
    }

    private void OnDestroy()
    {
        if (controller != null)
        {
            controller.RemoveSheep(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(hasFoundPlayer)
        {
            return;
        }
        if (other.gameObject.CompareTag("Player"))
        {
            // Trigger on sheep collected event
            OnSheepCollected?.Invoke();

            hasFoundPlayer = true;
            pc = other.GetComponent<PlayerController>();

            if (controller == null)
            {
                controller = FindObjectOfType<HerdController>();
                controller.AddSheep(this);
            }

            if (healingBoy)
            {
                SetBodyColor(Color.white);
                var pHealth = pc.GetComponentInChildren<PlayerHealthSystem>();
                if(pHealth != null)
                {
                    pHealth.RestoreHealth(healAmount);
                }
        
                // heal player
            }

            if (!controller.isCharging)
            {
                controller.SetFollowing(true);
            }
        }
        else if (other.gameObject.GetComponent<EnemyAgent>() != null && controller != null)
        {
            //controller.DetectThreat(other.transform.position);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Destination"))
        {
            if (controller)
                controller.ReachedDestination();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pc = null;
        }
    }
    public void TakeDamage(int damage)
    {
        // For now:
        tempHealth -= damage;
        if (tempHealth <= 0)
            Die();
        else
        {
            StartCoroutine(Hop(1, 0.5f));
        }
        // reduce health, should be used once enemy damage is working
        health.ModifyHealth(-damage);
        if (health.GetCurrentHealth() <= 0)
           Die();
    }

    public IEnumerator Hop(float h, float t)
    {
        float startHeight = sheepMesh.transform.position.y;
        float time = 0f;
        while (time <= t)
        {
            time += Time.deltaTime;
            float yPos = startHeight + ((-h) / (Mathf.Pow(t / 2, 2))) * Mathf.Pow(time - t / 2, 2) + h;
            sheepMesh.transform.position = new Vector3(sheepMesh.transform.position.x, yPos, sheepMesh.transform.position.z);
            
            yield return new WaitForEndOfFrame();
        }
        sheepMesh.transform.position = new Vector3(sheepMesh.transform.position.x, startHeight, sheepMesh.transform.position.z);
        yield return null;
    }

    public void SetStoppingDistance(float dist)
    {
        agent.stoppingDistance = dist;
    }

    public void SetBodyColor(Color _color)
    {
        if (_color == Color.white)
        {
            // Restore to original color
            bodyMesh.material.SetInt(bodyMesh.material.shader.GetPropertyName(4), 1);
            bodyMesh.material.SetColor(bodyMesh.material.shader.GetPropertyName(5), _color);
        }
        else
        {
            bodyMesh.material.SetInt(bodyMesh.material.shader.GetPropertyName(4), 0);
            bodyMesh.material.SetColor(bodyMesh.material.shader.GetPropertyName(5), _color);
        }
    }
}