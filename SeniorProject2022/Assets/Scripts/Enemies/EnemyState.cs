using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Enemy { 
    public enum EnemyStateId
    {
        // Slime
        SlimeIdle,
        ChasePlayer,
        Hurt,
        Attack,

        // Patrolling
        Idle,
        Patrol,

        // Mushroom Baby
        MushroomChasePlayer,
        MushroomFalling,
        MushroomFlee,
        MushroomReturn
    }

    public interface IEnemyState
    {
        EnemyStateId GetId();
        void Enter(EnemyAgent agent);
        void Update(EnemyAgent agent);
        void Exit(EnemyAgent agent);
    }
}
