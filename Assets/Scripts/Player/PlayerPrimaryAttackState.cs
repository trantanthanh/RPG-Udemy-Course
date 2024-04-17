using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private int comboCounter = 0;
    private int numOfCombo = 3;
    private float timeCombo= 1f;//next attack must below this value to increase comboCounter;
    private float timeNextAttack = 0f;

    public PlayerPrimaryAttackState(Player _player, PlayerSateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
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
            player.SetVelocity(0, 0);
        }
        if (triggerCalled)
        {
            comboCounter++;
            stateMachine.ChangeState(player.idleState);
        }
    }
}
