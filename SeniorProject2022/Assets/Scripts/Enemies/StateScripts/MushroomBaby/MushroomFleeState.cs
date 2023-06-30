using System.Collections;
using UnityEngine;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine.AI;

namespace Enemy
{
    public class MushroomFleeState : IEnemyState
    {
        private MushroomBabyEnemyAgent _mushAgent;
        private Tween fleeTween;

        public MushroomFleeState(MushroomBabyEnemyAgent agent)
        {
            _mushAgent = agent;
        }
        public void Enter(EnemyAgent agent)
        {
            agent.navMeshAgent.speed = _mushAgent.fleeSpeed;
            agent.animator.SetTrigger("runTrigger");

            fleeTween = DOVirtual.DelayedCall(_mushAgent.fleeTime, () =>
            {
                // enable his lil hat again
                _mushAgent._animManager.ActivateCap();
                agent.stateMachine.ChangeState(EnemyStateId.MushroomChasePlayer);
            }, false);
        }

        public void Exit(EnemyAgent agent)
        {
            // enable his lil hat again
            _mushAgent._animManager.ActivateCap();
            fleeTween?.Kill();
        }

        public EnemyStateId GetId()
        {
            return EnemyStateId.MushroomFlee;
        }

        public void Update(EnemyAgent agent)
        {
            Vector3 mushPos = agent.transform.position;
            // TODO: make this update less frequently and get a smarter position.
            Vector3 awayPos = mushPos + (mushPos - agent.target.transform.position) *
                    _mushAgent.fleelookDist;
            NavMeshHit hit;
            bool foundPos = NavMesh.SamplePosition(awayPos,
                out hit, 4, NavMesh.AllAreas);
            if (foundPos)
            {
                agent.navMeshAgent.SetDestination(hit.position);
            }
        }
    }
}