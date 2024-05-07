using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackHoleState : PlayerState
{
    private float flyTime = 0.4f;
    private bool skillUsed = false;
    private float gravityBackup;
    public PlayerBlackHoleState(Player _player, PlayerSateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        skillUsed = false;
        timerState = flyTime;
        gravityBackup = rb.gravityScale;
        rb.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = gravityBackup;
    }

    public override void Update()
    {
        base.Update();

        if (timerState > 0)
        {
            rb.velocity = new Vector2(0, 15);
        }
        else if (timerState < 0)
        {
            rb.velocity = new Vector2(0, -0.1f);

            if (!skillUsed)
            {
                if (player.skills.blackHole.CanUseSkil())
                {
                    Debug.Log("Cast black hole");
                    player.skills.blackHole.UseSkill();
                    skillUsed = true;
                }
            }
        }
    }
}
