using System.Collections;
using UnityEngine;

public class DeathBringerBattleState : EnemyState
{
    EnemyDeathBringer enemy;
    Transform player;
    int moveDir = 1;//1 : move right, -1 move left
    public DeathBringerBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName, EnemyDeathBringer _enemy) : base(_enemyBase, _stateMachine, _animName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.Instance.player.transform;
    }

    public override void Update()
    {
        base.Update();

        if (enemy.transform.position.x < player.position.x)
        {
            moveDir = 1;
        }
        else if (enemy.transform.position.x > player.position.x)
        {
            moveDir = -1;
        }

        if (enemy.IsFaceWallDetected() || !enemy.IsGroundDetected())
        {
            enemy.SetZeroVelocity();
            stateMachine.ChangeState(enemy.idleState);
            return;
        }

        enemy.SetVelocity(enemy.CurrentMoveSpeed * moveDir, rb.velocity.y);
        RaycastHit2D hit = enemy.IsPlayerDetected();
        if (hit)//saw player
        {
            stateTimer = enemy.BattleTime;
            if (hit.distance < enemy.DistanceAttack)
            {
                enemy.SetZeroVelocity();//stop moving when in attack range
                if (enemy.CanAttack())
                {
                    stateMachine.ChangeState(enemy.attackState);
                }
            }
        }
        else
        {
            if (stateTimer < 0)
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }
    }
}
