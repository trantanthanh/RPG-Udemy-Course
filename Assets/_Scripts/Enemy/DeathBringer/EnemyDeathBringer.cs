using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathBringer : Enemy
{
    #region States
    public DeathBringerIdleState idleState { get; private set; }
    public DeathBringerMoveState moveState { get; private set; }
    public DeathBringerBattleState battleState { get; private set; }
    public DeathBringerAttackState attackState { get; private set; }
    public DeathBringerDeadState deadState { get; private set; }
    #endregion

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            //do nothing
            return true;
        }
        return false;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        idleState = new DeathBringerIdleState(this, stateMachine, "Idle", this);
        moveState = new DeathBringerMoveState(this, stateMachine, "Move", this);
        battleState = new DeathBringerBattleState(this, stateMachine, "Move", this);
        attackState = new DeathBringerAttackState(this, stateMachine, "Attack", this);
        deadState = new DeathBringerDeadState(this, stateMachine, "Dead", this);
        stateMachine.Initialize(idleState);
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }
}
