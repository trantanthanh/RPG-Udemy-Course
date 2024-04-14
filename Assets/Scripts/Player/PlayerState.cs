using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected Player player;
    protected PlayerSateMachine stateMachine;

    private string animName;

    public PlayerState(Player _player, PlayerSateMachine _stateMachine, string _animName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animName = _animName;
    }

    public virtual void Enter()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void Exit()
    {

    }
}
