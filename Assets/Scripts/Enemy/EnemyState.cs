using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected Enemy enemy;
    protected EnemyStateMachine stateMachine;
    protected Rigidbody2D rb;

    private string animName;

    public EnemyState(Enemy _enemy, EnemyStateMachine _stateMachine,  string _animName)
    {
        this.enemy = _enemy;
        this.stateMachine = _stateMachine;
        this.animName = _animName;
    }

    public virtual void Enter()
    { 
    }

    public virtual void Update()
    { 
    }

    public virtual void Exit()
    {

    }
}
