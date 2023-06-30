using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Enemy
{
    public class MushroomBabyEnemyAgent : EnemyAgent
    {
        [HideInInspector] public MushroomAnimManager _animManager;
        public float fleeTime = 5.0f;
        public float fleelookDist = 3.0f;
        public float fleeSpeed = 2.0f;
        public float mushroomAttackDistance = .7f;  // distance to trigger the dash
        public float runningSpeed = 5.0f;
        public float dashDistance = 5.0f;
        public float walkingSpeed = .4f;
        [SerializeField] private float maxDistanceFromSpawn = 30.0f;

        private Collider _collider;
        public Material flashMat;
        public Material blackMat;
        
        private List<SkinnedMeshRenderer> _mushMeshes;   // must be set in inspector.
        private List<Material> toonMats;    // no more untextured enemies :(
        private Vector3 startPos;

        
        [HideInInspector] public bool canAimAtPlayer = true;

        public override void AgentAwake()
        {
            _animManager = GetComponentInChildren<MushroomAnimManager>();
            _animManager.ActivateCap();
            _collider = GetComponent<Collider>();
            
            _mushMeshes = new List<SkinnedMeshRenderer>(GetComponentsInChildren<SkinnedMeshRenderer>());
            toonMats = new List<Material>(_mushMeshes.Select(x => x.material));

            startPos = transform.position;
        }

        public List<Vector3> GetPatrolPoints()
        {
            return (stateMachine.GetState(EnemyStateId.Patrol) as MusroomPatrolState).validPatrolPoints;
        }

        void Start()
        {
            // i hate this code and I hope this issue never comes up again lololol
            target = PlayerController.Instance;
            // Set up state machine
            IEnemyState[] enemyStates = { new MusroomPatrolState(this),
                new MushroomChasePlayerState(this), new MushroomFallingState(this),
                new MushroomFleeState(this), new MushroomReturnState(this), 
            };
            stateMachine = new EnemyStateMachine(this, EnemyStateId.Patrol, enemyStates);
        }

        public override void AgentTakeDamage(Vector3 knockbackForce)
        {
            if (stateMachine.GetCurrentState() == EnemyStateId.Patrol)
            {
                stateMachine.ChangeState(EnemyStateId.MushroomChasePlayer);
            }
            StartCoroutine(HurtSequence());
        }
        
        private void SetMaterials(Material mat)
        {
            foreach(SkinnedMeshRenderer mesh in _mushMeshes)
            {
                mesh.material = mat;
            }
        }

        private void SetBaseMaterial()
        {
            for(int i = 0; i < _mushMeshes.Count; i++)
            {
                _mushMeshes[i].material = toonMats[i];
            }
        }
        
        private IEnumerator HurtSequence()
        {
            float prevSpeed = navMeshAgent.speed;
            navMeshAgent.speed = 0;
            SetMaterials(flashMat);
            yield return null;
            SetMaterials(blackMat);
            yield return null;
            yield return null;
            SetMaterials(flashMat);
            yield return null;
            yield return null;
            SetMaterials(blackMat);
            yield return null;
            yield return null;
            yield return null;
            SetMaterials(flashMat);
            yield return null;
            yield return null;
            SetBaseMaterial();
            yield return new WaitForSeconds(.13f);
            navMeshAgent.speed = prevSpeed;
        }

        public void Explode()
        {
            _animManager.Explode();
            // go into fleeing state
        }

        public override void AgentUpdate()
        {
            // check distance from initial position.
            Vector3 horizontalDiff = transform.position - startPos;
            horizontalDiff.y = 0;
            var state = stateMachine.GetCurrentState();
            if (Vector3.Magnitude(horizontalDiff) > maxDistanceFromSpawn &&
                state != EnemyStateId.MushroomReturn &&
                navMeshAgent.enabled == true)
            {
                stateMachine.ChangeState(EnemyStateId.MushroomReturn);
            }
            animator.SetFloat("speed", navMeshAgent.velocity.magnitude);
            animator.SetFloat("rotation", Vector3.Cross(navMeshAgent.velocity.normalized, transform.forward).y);
        }

        public void SetActiveCollider(bool active)
        {
            _collider.enabled = active;
        }
    }
}
