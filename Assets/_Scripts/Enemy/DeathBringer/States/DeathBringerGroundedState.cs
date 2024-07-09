using System.Collections;
using UnityEngine;

public class DeathBringerGroundedState : EnemyState
{
    protected EnemyDeathBringer enemy;
    protected static float freeTime;

    public DeathBringerGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName, EnemyDeathBringer _enemy) : base(_enemyBase, _stateMachine, _animName)
    {
        this.enemy = _enemy;
    }

    public void ResetFreeTime()
    {
        freeTime = 0;
    }

    public override void Update()
    {
        freeTime += Time.deltaTime;
        if (freeTime >= enemy.TimeCannotAttackLong)
        {
            ResetFreeTime();
            stateMachine.ChangeState(enemy.teleportState);
            return;
        }
        base.Update();
        if (enemy.IsPlayerDetected() && enemy.CanAttack() && !(enemy.IsFaceWallDetected() || !enemy.IsGroundDetected()))
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
