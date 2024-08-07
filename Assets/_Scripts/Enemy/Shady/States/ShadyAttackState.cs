﻿using System.Collections;
using UnityEngine;

public class ShadyAttackState : EnemyState
{
    EnemyShady enemy;
    public ShadyAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName, EnemyShady enemy) : base(_enemyBase, _stateMachine, _animName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
