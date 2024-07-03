using System.Collections;
using UnityEngine;

public class ShadyDeadState : EnemyState
{
    EnemyShady enemy;
    public ShadyDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName, EnemyShady _enemy) : base(_enemyBase, _stateMachine, _animName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetupForDead();
        stateTimer = enemy.DeadTimeFlyUp;
    }

    public override void Exit()
    {
        base.Exit();
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
