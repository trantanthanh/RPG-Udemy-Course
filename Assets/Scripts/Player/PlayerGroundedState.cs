using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerSateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
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

        if (player.IsGroundDetected())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                player.stateMachine.ChangeState(player.jumpState);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                player.stateMachine.ChangeState(player.primaryAttack);
                return;
            }
        }
        else
        {
            player.stateMachine.ChangeState(player.airState);
        }
    }
}
