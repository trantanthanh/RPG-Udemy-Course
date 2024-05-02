using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Entity
{
    [Header("Dash info")]
    [SerializeField] float dashSpeed = 16f;
    [SerializeField] float timeDash = 0.5f;

    [Header("Wall slide jump info")]
    [SerializeField] float yVelocitySlideMulti = 0.7f;
    [Tooltip("Can control after period time wall jump")]
    [SerializeField] float timeWallJump = 0.4f;
    [SerializeField] float xJumpForceWall = 5f;

    [Header("Counter attack info")]
    [SerializeField] float counterAttackDuration = 0.2f;

    #region Property
    public float CounterAttackDuration
    {
        get
        {
            return counterAttackDuration;
        }
    }
    public float X_JumpForceWall
    {
        get
        {
            return xJumpForceWall;
        }
    }

    public float TimeWallJump
    {
        get
        {
            return timeWallJump;
        }
    }
    public float VelocitySlideMulti
    {
        get
        {
            return yVelocitySlideMulti;
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
    #endregion

    #region Components
    public SkillManager skills { get; private set; }
    #endregion
    public GameObject sword { get; private set; }

    #region States
    public PlayerSateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    public PlayerCounterAttackState counterAttackState {  get; private set; }
    public PlayerCatchSwordState catchSwordState { get; private set; }
    public PlayerAimSwordState aimSwordState {  get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerSateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        catchSwordState = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        aimSwordState = new PlayerAimSwordState(this, stateMachine, "AimSword");

        skills = SkillManager.Instance;
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
        base.Update();
        stateMachine.currentState.Update();
        //Debug.Log($"IsGrounded {IsGroundDetected()}");
        //Debug.Log($"IsFaceWall {IsFaceWallDetected()}");

        CheckDash();
    }

    private void CheckDash()
    {
        if (IsFaceWallDetected()) return;
        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.Instance.dash.CanUseSkil())
        {
            skills.dash.UseSkill();
            stateMachine.ChangeState(dashState);
        }
    }

    public void Jump()
    {
        SetVelocity(rb.velocity.x, jumpForce);
    }

    public void AnimationDoneTrigger() => stateMachine.currentState.AnimationDoneTrigger();

    public override void Damage()
    {
        base.Damage();

        Debug.Log("Player has damaged");
    }

    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSwordState);
        Destroy(sword);
    }
}
