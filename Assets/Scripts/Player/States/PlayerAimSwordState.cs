using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.skills.swordThrow.DotsActive(true);
    }

    public override void Exit()
    {
        base.Exit();
        player.skills.swordThrow.DotsActive(false);
    }

    public override void Update()
    {
        base.Update();
        player.SetZeroVelocity();
        if (Input.GetKeyUp(KeyCode.Mouse1))//release right mouse button
        {
            stateMachine.ChangeState(player.idleState);
        }
        else
        {
            //check flip player
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mousePosition.x < player.transform.position.x && player.IsFacingRight || mousePosition.x > player.transform.position.x && !player.IsFacingRight)
            {
                player.Flip();
            }
        }
    }
}
