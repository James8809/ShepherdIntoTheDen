using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyPatrolState : IEnemyState
    {
        public Vector3 patrolDestination;

        public void Enter(EnemyAgent agent)
        {
            // Set new patrol destination
            //patrolDestination = GetRandomPoint(agent.transform.position, agent.data.enemyPatrolRadius);
            agent.navMeshAgent.SetDestination(patrolDestination);
        }

        public void Exit(EnemyAgent agent)
        {
        }

        public EnemyStateId GetId()
        {
            return EnemyStateId.Patrol;
        }

        public void Update(EnemyAgent agent)
        {
            // Set destination to random point
            if (Vector3.Magnitude(agent.transform.position - patrolDestination) < 0.2)
            {
                agent.stateMachine.ChangeState(EnemyStateId.Idle);
            }
        }

        // Get Random Point on a Navmesh surface
        private static Vector3 GetRandomPoint(Vector3 center, float maxDistance)
        {
            Vector3 randomPos = Random.insideUnitSphere * maxDistance + center;

            NavMeshHit hit; // NavMesh Sampling Info Container

            NavMesh.SamplePosition(randomPos, out hit, maxDistance, NavMesh.AllAreas);
            return hit.position;
        }
    }
}
