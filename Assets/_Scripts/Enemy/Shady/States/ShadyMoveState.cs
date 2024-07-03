using System.Collections;
using UnityEngine;

public class ShadyMoveState : ShadyGroundedState
{
    public ShadyMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName, EnemyShady _enemy) : base(_enemyBase, _stateMachine, _animName, _enemy)
    {
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
