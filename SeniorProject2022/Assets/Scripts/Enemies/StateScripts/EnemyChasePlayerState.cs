using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy {
    public class EnemyChasePlayerState : IEnemyState
    {
        public void Enter(EnemyAgent agent)
        {
            agent.animator.SetBool("isChasingPlayer", true);
        }

        public void Exit(EnemyAgent agent)
        {
            agent.navMeshAgent.destination = agent.transform.position;
            agent.animator.SetBool("isChasingPlayer", false);
        }

        public EnemyStateId GetId()
        {
            return EnemyStateId.ChasePlayer;
        }

        public void Update(EnemyAgent agent)
        {
            agent.navMeshAgent.destination = agent.target.transform.position;
            if (Vector3.Magnitude(agent.transform.position - agent.target.transform.position) < 
                4) { // TODO: this is a random number that should be changed.
                agent.stateMachine.ChangeState(EnemyStateId.Attack);
            }
        }
    }
}
