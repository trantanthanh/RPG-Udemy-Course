using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : EnemyState
{
    private EnemySkeleton enemy;
    public SkeletonMoveState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animName, EnemySkeleton enemy) : base(_enemy, _stateMachine, _animName)
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
        if (enemy.IsFaceWallDetected() || !enemy.IsGroundDetected())
        {
            stateMachine.ChangeState(enemy.idleState);
            enemy.Flip();
            return;
        }

        enemy.SetVelocity(enemy.facingDir * enemy.MoveSpeed, rb.velocity.y);
    }
}
