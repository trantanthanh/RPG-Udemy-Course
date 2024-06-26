using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        timerState = player.TimeDash;
        //player.skills.clone.CreateClone(player.transform, Vector3.zero);
        player.skills.dash.CreateCloneOnDashStart();
    }

    public override void Exit()
    {
        base.Exit();
        player.skills.dash.CreateCloneOnDashOver();
    }

    public override void Update()
    {
        base.Update();

        if (timerState < 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
        else
        {
            if (xInput != 0)
            {
                player.SetVelocity(xInput * player.DashSpeed, 0);
                player.fx.CreateAfterImage();
            }
            else
            {
                stateMachine.ChangeState(player.idleState);
            }
        }
    }
}
