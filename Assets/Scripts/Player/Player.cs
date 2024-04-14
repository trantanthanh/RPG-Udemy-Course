using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Move info")]
    [SerializeField] float baseMoveSpeed = 8f;
    public float moveSpeed { get; private set; }

    #region Components
    public Animator animator;
    public Rigidbody2D rb { get; private set; }
    #endregion

    #region States
    public PlayerSateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    #endregion

    private void Awake()
    {
        stateMachine = new PlayerSateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
    }

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = baseMoveSpeed;
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        stateMachine.Initialize(idleState);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.currentState.Update();
    }

    public void SetVelocity(float _xVelocity, float yVelocity)
    {
        rb.velocity = new Vector2(_xVelocity, yVelocity);
        CheckFlipSprite();
    }

    private void CheckFlipSprite()
    {
        if (rb.velocity.x != 0)
            transform.localScale = new Vector3(Mathf.Sign(rb.velocity.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y);
    }
}
