using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Enemy
{
    public class EnemyStateMachine
    {
        public EnemyAgent agent;

        private IEnemyState[] states;
        private EnemyStateId currentState;

        public EnemyStateMachine(EnemyAgent agent, EnemyStateId initialState, IEnemyState[] enemyStates)
        {
            this.agent = agent;
            int numStates = System.Enum.GetNames(typeof(EnemyStateId)).Length;
            states = new IEnemyState[numStates];
            this.currentState = initialState;
            foreach(var state in enemyStates) { 
                RegisterState(state);
            }
            GetState(currentState)?.Enter(agent);
        }

        private void RegisterState(IEnemyState state)
        {
            int index = (int)state.GetId();
            states[index] = state;
        }

        public EnemyStateId GetCurrentState()
        {
            return currentState;
        }

        public IEnemyState GetState(EnemyStateId stateId)
        {
            int index = (int)stateId;
            return states[index];
        }

        public void Update()
        {
            GetState(currentState)?.Update(agent);
            //Debug.Log(currentState);
        }

        public void ChangeState(EnemyStateId newState)
        {
            GetState(currentState)?.Exit(agent);
            currentState = newState;
            GetState(currentState)?.Enter(agent);
        }
    }
}