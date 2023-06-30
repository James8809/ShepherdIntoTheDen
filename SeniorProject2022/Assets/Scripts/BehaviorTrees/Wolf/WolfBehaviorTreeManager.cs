using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfBehaviorTreeManager : EnemyManager
{
    public bool isPackMember = false;
    [HideInInspector] public bool inCombat = false;
    [HideInInspector] public WolfPackManager packManager;
    [HideInInspector] public bool isAnimDone;   // reset by one-shot animations in behavior tree, reset by events
    public Animator animator;
    public GameObject deathParticles;    // set in inspector
    public Collider biteCollider;    // set in inspector
    public Collider lungeHurtbox;    // set in inspector
    public Collider lungeCollider;    // set in inspector
    public float stunDuration = 2f;
    public ParticleSystem stunnedParticle;
    public ParticleSystem biteParticle;
    public ParticleSystem lungeParticle;
    [HideInInspector] public bool lungeInterrupted;
    [HideInInspector] public bool lunging;
    [SerializeField] public Collider enemyBody;
    private Tween disableBiteParticle;
    private Tween stunnedParticleTween;
    private Health _health;


    protected override void Die()
    {
        base.Die();
        if (isPackMember)
        {
            packManager.WolfDown();
        }
        GameObject vfx = Instantiate(deathParticles, transform.position, transform.rotation) as GameObject;
        if (stunnedParticleTween != null)
            stunnedParticleTween.Kill();
        Destroy(vfx, 1.0f);
        Destroy(gameObject);
    }

    public void CompleteAnim()
    {
        isAnimDone = true;
    }

    public void beginBite()
    {
        biteCollider.enabled = true;
    }

    public void BiteParticle()
    {
        biteParticle.gameObject.SetActive(true);
    }

    public void endBite()
    {
        biteCollider.enabled = false;
        disableBiteParticle = DOVirtual.DelayedCall(0.5f, () =>
        {
            biteParticle.gameObject.SetActive(false);
        }, false);
    }

    public void beginLunge()
    {
        lunging = true;
        //lungeHurtbox.enabled = true;
    }

    public void InterruptLunge()
    {
        lungeInterrupted = true;
        biteCollider.enabled = false;
        stunnedParticle.gameObject.SetActive(true);
        animator.SetBool("stunned", true);
        stunnedParticleTween = DOVirtual.DelayedCall(stunDuration, () =>
        {
            stunnedParticle.gameObject.SetActive(false);
            animator.SetBool("stunned", false);
        }, false);
    }

    public void lungeLanding()
    {
        //lungeHurtbox.enabled = false;
        lunging = false;
        biteCollider.enabled = true;
        lungeParticle.gameObject.SetActive(true);
    }

    public void endLunge()
    {
        lunging = false;
        biteCollider.enabled = false;
        //lungeParticle.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        stunnedParticleTween.Kill();
        disableBiteParticle.Kill();
    }


    private void Start()
    {
        _health = GetComponent<Health>();
    }
    private void Update()
    {
        if (!inCombat)
        {
            if (_health.GetCurrentHealth() < _health.GetMaxHealth())
            {
                inCombat = true;
                if (isPackMember)
                {
                    packManager.EnterCombat();
                }
            }
        }
    }
}
