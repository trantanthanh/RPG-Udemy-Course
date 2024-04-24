using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    EnemySkeleton enemy;
    Transform player;
    int moveDir = 1;//1 : move right, -1 move left
    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName, EnemySkeleton enemy) : base(_enemyBase, _stateMachine, _animName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = GameObject.Find("Player").transform;
    }

    public override void Exit()
    {
        base.Exit();
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

        enemy.SetVelocity(enemy.MoveSpeed * moveDir, rb.velocity.y);
    }
}
