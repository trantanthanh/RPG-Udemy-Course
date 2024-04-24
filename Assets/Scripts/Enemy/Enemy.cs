using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{

    #region States
    public EnemyStateMachine stateMachine {  get; private set; }
    public EnemyIdleState idleState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        idleState = new EnemyIdleState(this, stateMachine, "Idle");
        
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    // Update is called once per frame
    protected override void Update()
    {
        stateMachine.currentState.Update();
    }

    public void AnimationDoneTrigger() => stateMachine.currentState.AnimationDoneTrigger();

}
