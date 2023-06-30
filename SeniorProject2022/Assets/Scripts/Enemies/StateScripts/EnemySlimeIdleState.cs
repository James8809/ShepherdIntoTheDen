using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy {
    public class EnemySlimeIdleState : IEnemyState
    {
        public void Enter(EnemyAgent agent)
        {
        }

        public void Exit(EnemyAgent agent)
        {
        }

        public EnemyStateId GetId()
        {
            return EnemyStateId.SlimeIdle;
        }

        public void Update(EnemyAgent agent)
        {
            //if (Vector3.Distance(agent.transform.position, agent.target.transform.position) < agent.data.distanceToChase)
            //    agent.stateMachine.ChangeState(EnemyStateId.ChasePlayer);
        }
    }
}
