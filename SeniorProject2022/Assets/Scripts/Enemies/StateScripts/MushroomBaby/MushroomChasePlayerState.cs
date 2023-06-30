using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

namespace Enemy
{
    public class MushroomChasePlayerState : IEnemyState
    {
        bool fallTimerSet;
        
        private MushroomBabyEnemyAgent _mushAgent;

        private Tween getUpTween;

        public MushroomChasePlayerState(MushroomBabyEnemyAgent agent)
        {
            _mushAgent = agent;
        }
        
        public void Enter(EnemyAgent agent)
        {
            agent.navMeshAgent.speed = _mushAgent.runningSpeed;
            agent.animator.SetTrigger("attackTrigger");
            fallTimerSet = false;
            getUpTween = DOVirtual.DelayedCall(1.0f, () => fallTimerSet = true);
        }

        public void Exit(EnemyAgent agent)
        {
            getUpTween?.Kill();
        }

        public EnemyStateId GetId()
        {
            return EnemyStateId.MushroomChasePlayer;
        }

        public void Update(EnemyAgent agent)
        {

            agent.navMeshAgent.SetDestination(agent.target.transform.position);

            if (fallTimerSet && Vector3.Distance(agent.target.transform.position, agent.transform.position) < 
                _mushAgent.mushroomAttackDistance)
            {
                agent.stateMachine.ChangeState(EnemyStateId.MushroomFalling);
            }
        }
    }
}
