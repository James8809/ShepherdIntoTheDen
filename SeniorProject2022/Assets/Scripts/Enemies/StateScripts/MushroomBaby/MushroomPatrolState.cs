using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class MusroomPatrolState : IEnemyState
    {
        public Vector3 patrolDestination;
        private Vector3 initialPosition;
        private bool initialPositionSet = false;
        private float patrolRadius = 4.2f;

        private MushroomBabyEnemyAgent _mushAgent;
        private int lastUsedPatrolIndex = 0;
        private int numPatrolPoints = 5;
        private float patrolDist = 10.0f;
        public List<Vector3> validPatrolPoints = new List<Vector3>();

        private Tween patrolTween;

        public MusroomPatrolState(MushroomBabyEnemyAgent agent)
        {
            _mushAgent = agent;
            int generatedPatrolPoints = 0;
            int attemptedTries = 0;
            while (attemptedTries < 200 && generatedPatrolPoints < numPatrolPoints)
            {
                NavMeshHit hit;
                Vector3 randomPosition = (Random.insideUnitSphere.normalized *
                                          Random.Range(.2f, 1.0f) * patrolDist) + _mushAgent.transform.position;
                bool gotHit = NavMesh.SamplePosition(randomPosition,
                    out hit, 1.0f, _mushAgent.navMeshAgent.areaMask);
                if (gotHit)
                {
                    generatedPatrolPoints++;
                    validPatrolPoints.Add(hit.position);
                }
                attemptedTries++;
            }
        }

        public void Enter(EnemyAgent agent)
        {
            agent.animator.ResetTrigger("attackTrigger");
            agent.animator.ResetTrigger("runTrigger");
            agent.navMeshAgent.speed = _mushAgent.walkingSpeed;
            patrolDestination = GetPatrolPoint();
            patrolTween = DOVirtual.DelayedCall(Random.Range(1, 4),
                () => agent.navMeshAgent.SetDestination(patrolDestination), false);
        }

        public Vector3 GetPatrolPoint()
        {
            int numPatrolPoints = validPatrolPoints.Count;
            if(numPatrolPoints == 0)
            {
                return _mushAgent.transform.position;
            }
            else if (numPatrolPoints == 1)
            {
                return validPatrolPoints[0];
            }
            else
            {
                Vector3 nextPoint;
                int indexToPatrol;
                do
                {
                    indexToPatrol = Random.Range(0, validPatrolPoints.Count);
                } while (indexToPatrol == lastUsedPatrolIndex);

                lastUsedPatrolIndex = indexToPatrol;
                return validPatrolPoints[indexToPatrol];
            }
        }

        public void Exit(EnemyAgent agent)
        {
            patrolTween?.Kill();
        }

        public EnemyStateId GetId()
        {
            return EnemyStateId.Patrol;
        }

        public void Update(EnemyAgent agent)
        {
            // check if player is nearby and in sight, and if so chase them
            Vector3 patrolHorizontalDiff = agent.transform.position - patrolDestination;
            Vector3 horizontalDiff = agent.transform.position - _mushAgent.target.transform.position;
            horizontalDiff.y = 0;
            patrolHorizontalDiff.y = 0;
            float dot = Vector3.Dot(agent.transform.forward, horizontalDiff.normalized);
            bool isTargetInFOV = dot > 0.3f && horizontalDiff.magnitude < 4.8f;
            bool isPlayerReallyClose = horizontalDiff.magnitude < 2f;

            if (isTargetInFOV || isPlayerReallyClose)
            {
                agent.stateMachine.ChangeState(EnemyStateId.MushroomChasePlayer);
            }
            // Set destination to random point
            else if (Vector3.Magnitude(patrolHorizontalDiff) < 1.7)
            {
                agent.stateMachine.ChangeState(EnemyStateId.Patrol);
            }
        }
    }
}