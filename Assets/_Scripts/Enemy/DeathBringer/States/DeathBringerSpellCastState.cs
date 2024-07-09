using System.Collections;
using UnityEngine;

public class DeathBringerSpellCastState : EnemyState
{
    EnemyDeathBringer enemy;
    private int amountOfSpell;
    private float spellCooldown = 0f;
    public DeathBringerSpellCastState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName, EnemyDeathBringer _enemy) : base(_enemyBase, _stateMachine, _animName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.TimeCastSpellDuration;
        amountOfSpell = enemy.AmountOfSpell;
        spellCooldown = 0.5f;//delay for animation CastStart
    }

    public override void Update()
    {
        base.Update();
        spellCooldown -= Time.deltaTime;
        if (CanCast())
        {
            enemy.CastSpell();
        }

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.teleportState);
        }
    }

    private bool CanCast()
    {
        if (amountOfSpell > 0 && spellCooldown <= 0)
        {
            spellCooldown = enemy.SpellCastInterval;
            return true;
        }
        return false;
    }
}
