using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : SkeletonGroundedState
{
    public SkeletonMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName, EnemySkeleton _enemy) : base(_enemyBase, _stateMachine, _animName, _enemy)
    {
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
