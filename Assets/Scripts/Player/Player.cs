using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Move info")]
    [SerializeField] float moveSpeed = 8f;
    [SerializeField] float jumpForce = 12f;

    [Header("Dash info")]
    [SerializeField] float dashSpeed = 16f;
    [SerializeField] float timeDash = 0.5f;
    [SerializeField] float dashCooldown = 2f;
    float timerDashCooldown = 0f;

    public float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }
    }
    public float DashSpeed
    {
        get
        {
            return dashSpeed;
        }
    }
    public float TimeDash
    {
        get
        {
            return timeDash;
        }
    }

    [Header("Collision check")]
    [SerializeField] GameObject groundCheckStartPoint;
    [SerializeField] GameObject wallCheckStartPoint;
    [SerializeField] float distanceGroundCheck;
    [SerializeField] float distanceWallCheck;
    [SerializeField] LayerMask groundMask;
    public int facingDir { get; private set; } = 1;//-1 left, 1 right
    private bool isFacingRight = true;

    #region Components
    public Animator animator;
    public Rigidbody2D rb { get; private set; }
    #endregion

    #region States
    public PlayerSateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    #endregion

    private void Awake()
    {
        stateMachine = new PlayerSateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "WallJump");
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        stateMachine.Initialize(idleState);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.currentState.Update();
        //Debug.Log($"IsGrounded {IsGroundDetected()}");
        //Debug.Log($"IsFaceWall {IsFaceWallDetected()}");

        CheckDash();
    }

    private void CheckDash()
    {
        timerDashCooldown -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift) && timerDashCooldown < 0 && !IsFaceWallDetected())
        {
            timerDashCooldown = dashCooldown;
            stateMachine.ChangeState(dashState);
        }
    }

    public void Jump()
    {
        SetVelocity(rb.velocity.x, jumpForce);
    }

    public void SetVelocity(float _xVelocity, float yVelocity)
    {
        rb.velocity = new Vector2(_xVelocity, yVelocity);
        FlipController(_xVelocity);
    }

    private void Flip()
    {
        facingDir = facingDir * -1;
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    }

    public void FlipController(float _xVelocity)
    {
        if (_xVelocity < 0 && isFacingRight || _xVelocity > 0 && !isFacingRight)
        {
            Flip();
        }
    }

    public bool IsGroundDetected() => Physics2D.Raycast(groundCheckStartPoint.transform.position, Vector2.down, distanceGroundCheck, groundMask);
    public bool IsFaceWallDetected() => Physics2D.Raycast(wallCheckStartPoint.transform.position, Vector2.right * facingDir, distanceWallCheck, groundMask);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(groundCheckStartPoint.transform.position, new Vector2(groundCheckStartPoint.transform.position.x, groundCheckStartPoint.transform.position.y - distanceGroundCheck));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(wallCheckStartPoint.transform.position, new Vector2(wallCheckStartPoint.transform.position.x + facingDir * distanceWallCheck, wallCheckStartPoint.transform.position.y));
    }
}
