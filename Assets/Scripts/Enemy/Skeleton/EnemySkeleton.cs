using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : Enemy
{
    #region States
    public SkeletonIdleState idleState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    #endregion

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        idleState = new SkeletonIdleState(this, stateMachine, "Idle", this);
        moveState = new SkeletonMoveState(this, stateMachine, "Move", this);
        stateMachine.Initialize(idleState);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
