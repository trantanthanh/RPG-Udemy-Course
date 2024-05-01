using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerSateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        timerState = player.TimeDash;
        player.skill.clone.CreateClone(player.transform);
    }

    public override void Exit()
    {
        base.Exit();
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
            }
            else
            {
                stateMachine.ChangeState(player.idleState);
            }
        }
    }
}
