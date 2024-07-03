using System.Collections;
using UnityEngine;

public class ShadyStunnedState : EnemyState
{
    EnemyShady enemy;
    public ShadyStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName, EnemyShady _enemy) : base(_enemyBase, _stateMachine, _animName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.stunDuration;
        rb.velocity = new Vector2(-enemy.facingDir * enemy.stunDirection.x, enemy.stunDirection.y);
        enemy.fx.RedColorBlinkWithInterval(0.1f);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.fx.Invoke(nameof(enemy.fx.CancelColorChange), 0f);
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
