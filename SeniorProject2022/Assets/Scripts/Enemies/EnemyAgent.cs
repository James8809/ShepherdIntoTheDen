using UnityEngine;
using UnityEngine.AI;
// James needed this for status burning
using System.Collections;
using System.Collections.Generic;

namespace Enemy
{
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class EnemyAgent : DestroyableEnemy, IDamageReciever
    {
        [HideInInspector] public NavMeshAgent navMeshAgent;
        [HideInInspector] public EnemyStateMachine stateMachine;
        [HideInInspector] public Rigidbody rgbd;
        [HideInInspector] public Animator animator;
        [HideInInspector] public PlayerController target;
        [SerializeField] private GameObject hitTextObject;

        public GameObject deathParticles;
        [HideInInspector] public float initialSpeed;
        
        // James fire trail status list
        public List<int> burnTickTimers = new List<int>();

        protected override void EnemyAwake()
        {
            // Getting components
            rgbd = GetComponent<Rigidbody>();
            animator = GetComponentInChildren<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            initialSpeed = navMeshAgent.speed;
            
            AgentAwake();
        }

        #region Updates + Util

        void Update()
        {
            stateMachine.Update();
            AgentUpdate();
        }

        void OnDestroy()
        {
            
        }

        public void EnableRigidbody()
        {
            navMeshAgent.enabled = false;
            rgbd.isKinematic = false;
        }

        public void DisableRigidbody()
        {
            navMeshAgent.enabled = true;
            rgbd.isKinematic = true;
        }

        public abstract void AgentUpdate();

        #endregion


        #region Damage

        protected void TriggerEnemyDeath()
        {
            GameObject vfx = Instantiate(deathParticles, transform.position, transform.rotation) as GameObject;
            OnDeath?.Invoke(this);
            Destroy(vfx, 1.0f);
            gameObject.SetActive(false);
        }
        

        public void TakeDamage(Weapon weapon)
        {
            // reduce health
            var weaponObject = weapon.weaponType;
            Debug.Log(weaponObject);
            health.ModifyHealth(-weaponObject.GetDamage());
            // spawn a text : )
            var text = Instantiate(hitTextObject, transform.position + Vector3.up * 2.0f, Quaternion.Euler(0.0f, 40f, 0.0f));
            text.GetComponent<HitText>().SetText(weapon.weaponType.GetDamage());
            if (health.GetCurrentHealth() <= 0)
            {
                TriggerEnemyDeath();
                return;
            }
            // TODO: determine how knockback direction is rlly determined
            var knockBackdir = (transform.position - weapon.transform.position).normalized;
            AgentTakeDamage(weaponObject.knockback * knockBackdir);
        }

        public abstract void AgentTakeDamage(Vector3 knockbackForce);

        public abstract void AgentAwake();

        #endregion
        public void ApplyBurn(int ticks, Weapon obj)
        {
            if(burnTickTimers.Count <= 0)
            {
                burnTickTimers.Add(ticks);
                StartCoroutine(Burn(obj));
            }
            else
            {
                burnTickTimers.Add(ticks);
            }
        }

        // going through the list add do damage
        IEnumerator Burn(Weapon weapon)
        {
            while(burnTickTimers.Count >0)
            {
                for(int i = 0; i < burnTickTimers.Count; i++)
                {
                    burnTickTimers[i]--;
                }
                TakeDamage(weapon);
                burnTickTimers.RemoveAll(number => number == 0);
                yield return new WaitForSeconds(0.75f);
            }
        }

    }
}
