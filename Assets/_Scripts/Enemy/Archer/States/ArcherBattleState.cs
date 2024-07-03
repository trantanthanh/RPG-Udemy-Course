using System.Collections;
using UnityEngine;

public class ArcherBattleState : EnemyState
{
    EnemyArcher enemy;
    Transform player;
    int moveDir = 1;//1 : move right, -1 move left
    public ArcherBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName, EnemyArcher _enemy) : base(_enemyBase, _stateMachine, _animName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.Instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (enemy.UpdateCheckToJumpState()) return;
        if (enemy.transform.position.x < player.position.x)
        {
            moveDir = 1;
        }
        else if (enemy.transform.position.x > player.position.x)
        {
            moveDir = -1;
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
                else
                {
                    stateMachine.ChangeState(enemy.idleState);
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
