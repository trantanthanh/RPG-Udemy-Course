using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : EnemyState
{
    private EnemySkeleton enemy;
    public SkeletonIdleState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animName, EnemySkeleton enemy) : base(_enemy, _stateMachine, _animName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = 1f;//Idle for 1s
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }
}
