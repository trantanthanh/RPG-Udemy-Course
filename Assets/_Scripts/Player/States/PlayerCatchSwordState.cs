using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private Transform sword;
    public PlayerCatchSwordState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetZeroVelocity();
        sword = player.sword.transform;
        player.DestroySword();
        if (sword.position.x < player.transform.position.x && player.IsFacingRight || sword.position.x > player.transform.position.x && !player.IsFacingRight)
        {
            player.Flip();
        }

        rb.velocity = new Vector2(player.SwordReturnImpact * -player.facingDir, rb.velocity.y);
        if (player.IsGroundDetected())
        {
            player.fx.PlayDustFx();
        }

        player.fx.ShakeScreen();
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
