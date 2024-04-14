using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected Player player;
    protected PlayerSateMachine stateMachine;

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
        player.animator.SetBool(animName, true);
    }

    public virtual void Update()
    {
        xInput = Input.GetAxis("Horizontal");
    }

    public virtual void Exit()
    {
        player.animator.SetBool(animName, false);
    }
}
