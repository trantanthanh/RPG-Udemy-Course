using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public EnemyStateMachine stateMachine {  get; private set; }
    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        stateMachine.currentState.Update();
    }

    public void AnimationDoneTrigger() => stateMachine.currentState.AnimationDoneTrigger();

}
