using System.Collections;
using UnityEngine;

public class ArcherAttackState : EnemyState
{
    EnemyArcher enemy;
    Transform player;
    int moveDir = 1;//1 : move right, -1 move left
    public ArcherAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName, EnemyArcher _enemy) : base(_enemyBase, _stateMachine, _animName)
    {
        this.enemy = _enemy;
    }

    public override void AnimationDoneTrigger()
    {
        base.AnimationDoneTrigger();
        enemy.UpdateNextAttack();
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetZeroVelocity();
        player = PlayerManager.Instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
        enemy.UpdateNextAttack();
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            if (enemy.transform.position.x < player.position.x)
            {
                moveDir = 1;
            }
            else if (enemy.transform.position.x > player.position.x)
            {
                moveDir = -1;
            }

            RaycastHit2D hit = enemy.IsPlayerDetected();
            if (hit)//saw player
            {
                if (hit.distance < enemy.DistanceAttack)
                {
                    if (enemy.CanAttack())
                    {
                        triggerCalled = false;
                        enemyBase.animator.Play(animName, -1, 0f);
                    }
                }
            }
            else
            {
                stateMachine.ChangeState(enemy.battleState);
            }
        }
    }
}
