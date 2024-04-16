using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player _player, PlayerSateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(0, 0);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Mathf.Sign(xInput) == Mathf.Sign(player.facingDir) && player.IsFaceWallDetected())
        {
            return;
        }

        if (xInput != 0)
        {
            stateMachine.ChangeState(player.moveState);
        }
    }
}
