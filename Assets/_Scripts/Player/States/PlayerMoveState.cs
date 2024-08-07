using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
#if DEBUG
        PrintCallingClass();
#endif
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (player.isBusy) { 
            return;
        }

        if (xInput != 0)
        {
            player.SetVelocity(xInput * player.CurrentMoveSpeed, rb.velocity.y);
            //player.fx.CreateAfterImage();
        }

        if (xInput == 0 || player.IsFaceWallDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
