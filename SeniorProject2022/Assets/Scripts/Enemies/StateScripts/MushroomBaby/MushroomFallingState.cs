using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine.AI;


namespace Enemy
{
    public class MushroomFallingState : IEnemyState
    {
        private MushroomBabyEnemyAgent _mushAgent;
        private Tween fallingTween;

        public MushroomFallingState(MushroomBabyEnemyAgent agent)
        {
            _mushAgent = agent;
        }

        public void Enter(EnemyAgent agent)
        {
            _mushAgent.navMeshAgent.enabled = false;
            fallingTween = null;
            
            agent.animator.SetTrigger("fallingTrigger");
            _mushAgent.canAimAtPlayer = true;
            // dive as a last ditch effort towards the last player position (maybe offset by pl
        }

        public void Exit(EnemyAgent agent)
        {
            fallingTween?.Kill();
            ExitSequence(agent);
        }

        public EnemyStateId GetId()
        {
            return EnemyStateId.MushroomFalling;
        }
        
        // issue: somehow the mushroom is going straight to idle, without doing the
        
        // fall mushroom baby animation, which is the one w/ the shi so the return thing
        // is being triggered.....
        public void Update(EnemyAgent agent)
        {
            Vector3 mushPos = _mushAgent.transform.position;
            Vector3 towardsPlayer = (agent.target.transform.position - mushPos).normalized;
            if (_mushAgent.canAimAtPlayer)
            {
                // Lerp rotation of mushroom towards player.
                agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, 
                    Quaternion.LookRotation(towardsPlayer), .1f);
            }
            else if(fallingTween == null)
            {
                NavMeshHit hit;
                bool foundPos = NavMesh.SamplePosition(mushPos + towardsPlayer * _mushAgent.dashDistance,
                    out hit, 2, NavMesh.AllAreas);
                // if nearest point is found: 
                if (foundPos)
                {
                    fallingTween = agent.transform.DOJump(hit.position,
                        1.2f, 1, .4f, false).OnComplete(() =>
                        DOVirtual.DelayedCall(.8f, () => { }, false).OnComplete(() =>
                    {
                        agent.animator.SetTrigger("getUp");

                        _mushAgent.navMeshAgent.enabled = true;
                        DOVirtual.DelayedCall(.4f, () => agent.stateMachine.ChangeState(EnemyStateId.MushroomFlee), false);
                    }));
                }
                else
                {
                    agent.stateMachine.ChangeState(EnemyStateId.ChasePlayer);
                }
            }
        }

        private void ExitSequence(EnemyAgent agent)
        {
            agent.navMeshAgent.speed = agent.initialSpeed;
        }
    }
}
