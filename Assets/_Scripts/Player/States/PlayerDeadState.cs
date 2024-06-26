using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void AnimationDoneTrigger()
    {
        base.AnimationDoneTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        player.SetZeroVelocity();
        PlayerManager.Instance.ui.Die();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
