using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected Enemy enemy;
    protected EnemyStateMachine stateMachine;
    protected Rigidbody2D rb;

    private string animName;
    protected float stateTimer;
    protected bool triggerCalled = false;

    public EnemyState(Enemy _enemy, EnemyStateMachine _stateMachine,  string _animName)
    {
        this.enemy = _enemy;
        this.stateMachine = _stateMachine;
        this.animName = _animName;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        rb = enemy.rb;
        enemy.animator.SetBool(animName, true);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        enemy.animator.SetBool(animName, false);
    }

    public virtual void AnimationDoneTrigger()
    {
        triggerCalled = true;
    }
}
