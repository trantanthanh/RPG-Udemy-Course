using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeDeadState : EnemyState
{
    EnemySlime enemy;
    public SlimeDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName, EnemySlime _enemy) : base(_enemyBase, _stateMachine, _animName)
    {
        this.enemy = _enemy;
    }

    public override void AnimationDoneTrigger()
    {
        base.AnimationDoneTrigger();
        enemy.CheckSpawnSlime();
    }

    public override void Enter()
    {
        base.Enter();
        //enemy.animator.SetBool(enemy.lastAnimBoolName, true);
        //enemy.animator.speed = 0;
        //enemy.capsuleCollider.enabled = false;
        //stateTimer = 0.1f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        //if (stateTimer > 0)
        //{
        //    rb.velocity = new Vector2(0, 10);
        //}
    }
}
