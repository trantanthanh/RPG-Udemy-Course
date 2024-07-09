using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerIdleState : DeathBringerGroundedState
{
    public DeathBringerIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName, EnemyDeathBringer _enemy) : base(_enemyBase, _stateMachine, _animName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
    }

    public override void Update()
    {
        base.Update();
        if (!enemy.bossFightBegun && Vector2.Distance(enemy.transform.position, PlayerManager.Instance.player.transform.position) < 7)
        {
            enemy.bossFightBegun = true;
            stateMachine.ChangeState(enemy.battleState);
            return;
        }
        if (stateTimer < 0 && enemy.bossFightBegun)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }
}
