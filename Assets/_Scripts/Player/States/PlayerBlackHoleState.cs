using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackHoleState : PlayerState
{
    private float flyTime = 0.4f;
    private bool skillUsed = false;
    private float gravityBackup;
    public PlayerBlackHoleState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        SoundManager.Instance.PlaySFX(SFXDefine.sfx_bankai);
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
                if (player.skills.blackHole.CanUseSkill())
                {
                    Debug.Log("Cast black hole");
                    SoundManager.Instance.PlaySFX(SFXDefine.sfx_chronosphere);
                    PlayerManager.Instance.uiIngame.SetBlackHoleCooldown();
                    player.skills.blackHole.UseSkill();
                    skillUsed = true;
                }
            }
        }

        if (player.skills.blackHole.IsSkillCompleted())
        {
            stateMachine.ChangeState(player.airState);
        }
    }
}
