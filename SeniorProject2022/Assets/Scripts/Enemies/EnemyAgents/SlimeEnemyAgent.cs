using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class SlimeEnemyAgent : EnemyAgent
    {
        void Start()
        {
            // Set up state machine
            
            // removed attack state.
            IEnemyState[] enemyStates = { new EnemyChasePlayerState(), new EnemySlimeIdleState(), new EnemyHurtState() };
            stateMachine = new EnemyStateMachine(this, EnemyStateId.SlimeIdle, enemyStates);
        }

        public override void AgentTakeDamage(Vector3 knockbackForce)
        {
            var enemyHurtState = stateMachine.GetState(EnemyStateId.Hurt) as EnemyHurtState;
            enemyHurtState.knockbackForce = knockbackForce;
            stateMachine.ChangeState(EnemyStateId.Hurt);
        }

        public override void AgentUpdate()
        {
        }

        public override void AgentAwake()
        {
            
        }
    }
}
