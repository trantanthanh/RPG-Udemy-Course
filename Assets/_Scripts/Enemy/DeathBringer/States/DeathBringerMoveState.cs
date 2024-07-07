using System.Collections;
using UnityEngine;

public class DeathBringerMoveState : DeathBringerGroundedState
{
    public DeathBringerMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName, EnemyDeathBringer _enemy) : base(_enemyBase, _stateMachine, _animName, _enemy)
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

        enemy.SetVelocity(enemy.facingDir * enemy.CurrentMoveSpeed, rb.velocity.y);
    }
}
