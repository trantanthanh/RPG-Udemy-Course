using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private Transform sword;
    public PlayerCatchSwordState(Player _player, PlayerSateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetZeroVelocity();
        sword = player.sword.transform;
        if (sword.position.x < player.transform.position.x && player.IsFacingRight || sword.position.x > player.transform.position.x && !player.IsFacingRight)
        {
            player.Flip();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
