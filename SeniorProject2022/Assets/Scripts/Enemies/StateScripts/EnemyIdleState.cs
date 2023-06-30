using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;


namespace Enemy
{
    public class EnemyIdleState : IEnemyState
    {
        private float idleWaitTime;
        public void Enter(EnemyAgent agent)
        {
            // idleWaitTime = Random.Range(agent.data.idleWaitRange.x, agent.data.idleWaitRange.y);
            agent.StartCoroutine(IdleSequence(agent));
        }

        public void Exit(EnemyAgent agent)
        {
            agent.StopCoroutine("IdleSequence");
        }

        public EnemyStateId GetId()
        {
            return EnemyStateId.Idle;
        }

        public void Update(EnemyAgent agent)
        {
        }

        private IEnumerator IdleSequence(EnemyAgent agent)
        {
            yield return new WaitForSeconds(idleWaitTime);
            agent.stateMachine.ChangeState(EnemyStateId.Patrol);
        }
    }
}
