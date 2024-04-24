using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected Enemy enemyBase;
    protected EnemyStateMachine stateMachine;
    protected Rigidbody2D rb;

    private string animName;
    protected float stateTimer;
    protected bool triggerCalled = false;

    public EnemyState(Enemy _enemyBase, EnemyStateMachine _stateMachine,  string _animName)
    {
        this.enemyBase = _enemyBase;
        this.stateMachine = _stateMachine;
        this.animName = _animName;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        rb = enemyBase.rb;
        enemyBase.animator.SetBool(animName, true);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        enemyBase.animator.SetBool(animName, false);
    }

    public virtual void AnimationDoneTrigger()
    {
        triggerCalled = true;
    }
}
