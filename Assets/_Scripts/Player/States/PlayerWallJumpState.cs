using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
#if DEBUG
        PrintCallingClass();
#endif
        base.Enter();
        timerState = player.TimeWallJump;
        player.SetVelocity(-player.facingDir * player.X_JumpForceWall, player.JumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }

        if (player.IsFaceWallDetected())
        {
            stateMachine.ChangeState(player.wallSlideState);
            return;
        }

        if (timerState < 0)
        {
            stateMachine.ChangeState(player.airState);
        }
    }
}
