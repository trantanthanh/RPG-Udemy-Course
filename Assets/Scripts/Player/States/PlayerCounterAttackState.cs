using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private bool canCreateClone;
    public PlayerCounterAttackState(Player _player, PlayerSateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        canCreateClone = true;
        timerState = player.CounterAttackDuration;
        player.animator.SetBool("SuccessfulCounterAttack", false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        player.SetZeroVelocity();
        CheckAttackCounterSuccess();

        if (timerState < 0 || triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    private void CheckAttackCounterSuccess()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (Collider2D collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (enemy.CanBeStunned())
                {
                    timerState = 10f;//make sure animation SuccessfulCounterAttack can be done
                    player.animator.SetBool("SuccessfulCounterAttack", true);
                    if (canCreateClone)
                    {
                        canCreateClone = false;
                        player.skills.clone.CreateCloneOnCounterAttack(enemy.transform);
                    }
                }
            }
        }
    }
}
