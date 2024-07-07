using System.Collections;
using UnityEngine;

public class DeathBringerGroundedState : EnemyState
{
    protected EnemyDeathBringer enemy;
    public DeathBringerGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName, EnemyDeathBringer _enemy) : base(_enemyBase, _stateMachine, _animName)
    {
        this.enemy = _enemy;
    }

    public override void Update()
    {
        base.Update();
        if (enemy.IsPlayerDetected() && enemy.CanAttack() && !(enemy.IsFaceWallDetected() || !enemy.IsGroundDetected()))
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
