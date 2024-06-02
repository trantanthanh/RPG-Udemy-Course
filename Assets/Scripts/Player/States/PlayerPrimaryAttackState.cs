using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public int comboCounter { get; private set; } = 0;
    private int numOfCombo = 3;
    private float timeCombo= 1f;//next attack must below this value to increase comboCounter;
    private float timeNextAttack = 0f;

    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        timerState = 0.1f;

        comboCounter %= numOfCombo;
        if (Time.time > timeNextAttack)
        {
            //Over time to do combo
            comboCounter = 0;
        }
        timeNextAttack = Time.time + timeCombo;
        player.animator.SetInteger("ComboCounter", comboCounter);

        float attackDir = player.facingDir;
        if (xInput != 0)
            attackDir = xInput;

        player.SetVelocity(player.attackMovements[comboCounter].x * attackDir, player.attackMovements[comboCounter].y);
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine(player.BusyFor(0.15f));
    }

    public override void Update()
    {
        base.Update();
        if (timerState < 0)
        {
            player.SetZeroVelocity();
        }
        if (triggerCalled)
        {
            comboCounter++;
            stateMachine.ChangeState(player.idleState);
        }
    }
}
