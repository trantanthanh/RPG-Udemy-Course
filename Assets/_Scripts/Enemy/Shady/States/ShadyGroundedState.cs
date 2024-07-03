using System.Collections;
using UnityEngine;

public class ShadyGroundedState : EnemyState
{
    protected EnemyShady enemy;
    public ShadyGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName, EnemyShady _enemy) : base(_enemyBase, _stateMachine, _animName)
    {
        this.enemy = _enemy;
    }

    public override void Update()
    {
        base.Update();
        if (enemy.IsPlayerDetected() && enemy.CanAttack())
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
