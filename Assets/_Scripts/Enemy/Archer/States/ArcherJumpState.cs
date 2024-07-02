using System.Collections;
using UnityEngine;

public class ArcherJumpState : EnemyState
{
    EnemyArcher enemy;
    public ArcherJumpState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName, EnemyArcher _enemy) : base(_enemyBase, _stateMachine, _animName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        rb.velocity = new Vector2(enemy.jumpVelocity.x * -enemy.facingDir, enemy.jumpVelocity.y);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.UpdateTimeNextJump();
    }

    public override void Update()
    {
        base.Update();
        enemy.animator.SetFloat("yVelocity", rb.velocity.y);
        if (rb.velocity.y <= 0 && enemy.IsGroundDetected())
        {
            Debug.Log("jump state to idle");
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
