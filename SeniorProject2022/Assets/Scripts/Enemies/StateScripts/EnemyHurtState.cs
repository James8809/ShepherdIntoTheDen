using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;

namespace Enemy {
    public class EnemyHurtState : IEnemyState
    {
        public Vector3 knockbackForce;

        public void Enter(EnemyAgent agent)
        {
            hurtSequence(agent);
        }

        public void Exit(EnemyAgent agent)
        {
        }

        public EnemyStateId GetId()
        {
            return EnemyStateId.Hurt;
        }

        public void Update(EnemyAgent agent)
        {
        }

        //TODO: redo hurt sequence
        private async void hurtSequence(EnemyAgent agent)
        {
            try
            {
                agent.animator.SetBool("isHurt", true);
                agent.EnableRigidbody();
                agent.rgbd.AddForce(knockbackForce, ForceMode.Impulse);

                agent.DisableRigidbody();
                agent.animator.SetBool("isHurt", false);
                agent.stateMachine.ChangeState(EnemyStateId.SlimeIdle);
            }
            catch (TaskCanceledException) { }
        }
    }
}
