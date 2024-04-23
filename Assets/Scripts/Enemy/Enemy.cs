using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Components
    public Rigidbody2D rb {  get; private set; }
    public Animator animator { get; private set; }
    #endregion

    #region States
    public EnemyStateMachine stateMachine {  get; private set; }
    public EnemyIdleState idleState { get; private set; }
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

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
