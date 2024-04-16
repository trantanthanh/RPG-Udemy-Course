using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerSateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
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
        }

        if (rb.velocity.y == 0)//grounded
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (rb.velocity.x != 0)
        {
            player.SetVelocity(xInput * 0.8f * player.MoveSpeed, rb.velocity.y);
        }
    }
}
