using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
#if DEBUG
        PrintCallingClass();
#endif
        base.Enter();
        player.Jump();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        //if (CanWallSlide()) return;

        if (rb.velocity.y <= 0)//falling
        { 
            stateMachine.ChangeState(player.airState);
        }
    }
}
