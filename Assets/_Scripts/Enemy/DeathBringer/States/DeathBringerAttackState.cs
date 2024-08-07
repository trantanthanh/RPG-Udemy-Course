﻿using System.Collections;
using UnityEngine;

public class DeathBringerAttackState : EnemyState
{
    EnemyDeathBringer enemy;
    public DeathBringerAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName, EnemyDeathBringer _enemy) : base(_enemyBase, _stateMachine, _animName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetZeroVelocity();
        enemy.idleState.ResetFreeTime();
    }

    public override void Exit()
    {
        base.Exit();
        enemy.UpdateNextAttack();
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            if (enemy.CanTeleport())
            {
                stateMachine.ChangeState(enemy.teleportState);
            }
            else
            {
                stateMachine.ChangeState(enemy.battleState);
            }
        }
    }
}
