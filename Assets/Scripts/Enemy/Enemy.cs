using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region States
    public EnemyStateMachine stateMachine {  get; private set; }
    public EnemyIdleState idleState { get; private set; }
    #endregion

    private void Awake()
    {
        stateMachine = new EnemyStateMachine();
        idleState = new EnemyIdleState(this, stateMachine, "Idle");
        stateMachine.Initialize(idleState);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.currentState.Update();
    }
}
