using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected Player player;
    protected PlayerSateMachine stateMachine;
    protected Rigidbody2D rb;

    private string animName;

    protected float xInput;
    public PlayerState(Player _player, PlayerSateMachine _stateMachine, string _animName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animName = _animName;
    }

    public virtual void Enter()
    {
        rb = player.rb;
        player.animator.SetBool(animName, true);
    }

    public virtual void Update()
    {
        xInput = Input.GetAxis("Horizontal");
        player.animator.SetFloat("yVelocity", rb.velocity.y);
    }

    public virtual void Exit()
    {
        player.animator.SetBool(animName, false);
    }
}
