using System.Collections;
using System.Collections.Generic;
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
}
