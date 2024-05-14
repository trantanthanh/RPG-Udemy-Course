using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : Enemy, IEnemyDead
{
    #region States
    public SkeletonIdleState idleState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonBattleState battleState { get; private set; }
    public SkeletonAttackState attackState { get; private set; }
    public SkeletonStunnedState stunnedState { get; private set; }
    public SkeletonDeadState deadState { get; private set; }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }

    public override void DamageEffect()
    {
        base.DamageEffect();
        Debug.Log("Skeleton has damaged");
    }
    #endregion

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        idleState = new SkeletonIdleState(this, stateMachine, "Idle", this);
        moveState = new SkeletonMoveState(this, stateMachine, "Move", this);
        battleState = new SkeletonBattleState(this, stateMachine, "Move", this);
        attackState = new SkeletonAttackState(this, stateMachine, "Attack", this);
        stunnedState = new SkeletonStunnedState(this, stateMachine, "Stunned", this);
        deadState = new SkeletonDeadState(this, stateMachine, "Dead", this);
        stateMachine.Initialize(idleState);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //if (Input.GetKeyDown(KeyCode.U))
        //{
        //    stateMachine.ChangeState(stunnedState);
        //}
    }

    void IEnemyDead.DeadAction()
    {
        stateMachine.ChangeState(deadState);
    }
}
