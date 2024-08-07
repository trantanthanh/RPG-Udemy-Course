﻿using System.Collections;
using UnityEngine;

public class ArcherDeadState : EnemyState
{
    EnemyArcher enemy;
    public ArcherDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName, EnemyArcher _enemy) : base(_enemyBase, _stateMachine, _animName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetupForDead();
        stateTimer = enemy.DeadTimeFlyUp;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer > 0)
        {
            rb.velocity = enemy.DeadVelocityFallDown;
        }
    }
}
