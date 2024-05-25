using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (player.IsFaceWallDetected() && xInput == player.facingDir)
        {
            stateMachine.ChangeState(player.wallSlideState);
            return;
        }

        if (rb.velocity.y == 0)//grounded or stand on something
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }

        if (xInput != 0)
        {
            player.SetVelocity(xInput * 0.8f * player.MoveSpeed, rb.velocity.y);
        }
    }
}
