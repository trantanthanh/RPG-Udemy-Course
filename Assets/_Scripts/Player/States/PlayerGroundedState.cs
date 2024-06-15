using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
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
            if (player.skills.clone.blackHoleUnlocked && Input.GetKeyDown(KeyCode.R) && player.skills.blackHole.CanUseSkill())
            {
                player.stateMachine.ChangeState(player.blackHoleState);
                return;
            }

            if (player.skills.parry.parryUnlocked && player.skills.parry.CanUseSkill())
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    player.skills.parry.UseSkill();
                    player.stateMachine.ChangeState(player.counterAttackState);
                    return;
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                player.stateMachine.ChangeState(player.jumpState);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))//left mouse click
            {
                player.stateMachine.ChangeState(player.primaryAttackState);
                return;
            }

            if (player.skills.swordThrow.throwSwordUnlocked && Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword())//right mouse click
            {
                player.stateMachine.ChangeState(player.aimSwordState);
                return;
            }
        }
        else
        {
            player.stateMachine.ChangeState(player.airState);
        }
    }

    private bool HasNoSword()
    {
        if (!player.sword)
        {
            return true;
        }

        player.sword.GetComponent<SwordSkillController>().ReturnSword();
        return false;
    }
}
