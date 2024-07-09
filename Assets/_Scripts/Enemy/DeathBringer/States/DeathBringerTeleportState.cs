using System.Collections;
using UnityEngine;

public class DeathBringerTeleportState : EnemyState
{
    EnemyDeathBringer enemy;
    private int state = 0;
    private const int EXIT_STATE = 0;//disappear
    private const int ENTER_STATE = 1;//appear
    public DeathBringerTeleportState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName, EnemyDeathBringer _enemy) : base(_enemyBase, _stateMachine, _animName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        state = EXIT_STATE;
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
            if (enemy.CanDoSpellCast())
            {
                stateMachine.ChangeState(enemy.spellCastState);
            }
            else
            {
                stateMachine.ChangeState(enemy.battleState);
            }
        }
    }

    public void Appear()
    {
        enemyBase.animator.SetBool(animName, false);
    }

    public override void AnimationDoneTrigger()
    {
        //base.AnimationDoneTrigger();
        if (state == EXIT_STATE)
        {
            enemy.fx.MakeTransparent(true);
            enemy.StartFindNewPos();
            state++;
        }
        else if (state == ENTER_STATE)
        {
            triggerCalled = true;
            state++;
        }
    }
}
