using System.Collections;
using UnityEngine;

public class DeathBringerTeleportState : EnemyState
{
    EnemyDeathBringer enemy;
    public DeathBringerTeleportState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName, EnemyDeathBringer _enemy) : base(_enemyBase, _stateMachine, _animName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = 1f;
        enemy.StartFindNewPos();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0f)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
