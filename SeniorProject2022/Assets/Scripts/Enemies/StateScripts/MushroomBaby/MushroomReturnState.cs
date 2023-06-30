using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

namespace Enemy
{
    public class MushroomReturnState : IEnemyState
    {
        private MushroomBabyEnemyAgent _mushAgent;
        private Vector3 returnPoint;

        public MushroomReturnState(MushroomBabyEnemyAgent agent)
        {
            _mushAgent = agent;
        }
        
        public void Enter(EnemyAgent agent)
        {
            agent.animator.SetTrigger("runTrigger");
            agent.navMeshAgent.speed = _mushAgent.fleeSpeed;
            // this better have a patrol point or we have a mega problem that is bigger than this code lol
            returnPoint = _mushAgent.GetPatrolPoints()[0];
        }

        public void Exit(EnemyAgent agent)
        {
            agent.animator.SetTrigger("returnTrigger");
        }

        public EnemyStateId GetId()
        {
            return EnemyStateId.MushroomReturn;
        }

        public void Update(EnemyAgent agent)
        {
            // this logic is flawed if we have stairs lolol but idk i don't want to cast onto the navmesh so
            Vector3 horizontalDiff = agent.transform.position - returnPoint;
            horizontalDiff.y = 0;
            // check to see if close enough to the return point, if so exit the state and return.
            if (Vector3.Magnitude(horizontalDiff) < 2)
            {
                agent.stateMachine.ChangeState(EnemyStateId.Patrol);
                return;
            }
            agent.navMeshAgent.SetDestination(returnPoint);
        }
    }
}