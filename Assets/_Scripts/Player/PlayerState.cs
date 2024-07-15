using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected Rigidbody2D rb;
    protected float timerState = -1f;
    protected bool triggerCalled = false;

    private string animName;

    protected float xInput;
    protected float yInput;

#if DEBUG
    private bool IS_LOG_NAME_CLASS = false;
#endif
    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animName = _animName;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        rb = player.rb;
        player.animator.SetBool(animName, true);
    }

    public virtual void Update()
    {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
        player.animator.SetFloat("yVelocity", rb.velocity.y);
        timerState -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        player.animator.SetBool(animName, false);
    }

    public virtual void AnimationDoneTrigger()
    {
        triggerCalled = true;
    }

    protected bool CanWallSlide()
    {
        if (player.IsFaceWallDetected() && xInput == player.facingDir)
        {
            stateMachine.ChangeState(player.wallSlideState);
            return true;
        }

        if (rb.velocity.y == 0)//grounded or stand on something
        {
            stateMachine.ChangeState(player.idleState);
            return true;
        }

        if (xInput != 0)
        {
            player.SetVelocity(xInput * 0.8f * player.CurrentMoveSpeed, rb.velocity.y);
        }
        return false;
    }

#if DEBUG
    protected void PrintCallingClass()
    {
        if (!IS_LOG_NAME_CLASS) return;
        StackTrace stackTrace = new StackTrace();
        StackFrame frame = stackTrace.GetFrame(1); 
        string callingClass = frame.GetMethod().DeclaringType.Name;
        player.fx.CreatePopupText("\n" + callingClass);
    }
#endif
}
