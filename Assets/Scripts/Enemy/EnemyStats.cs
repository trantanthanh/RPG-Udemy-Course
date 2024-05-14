using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    Enemy enemy;
    IEnemyDead deadInterface;
    protected override void Start()
    {
        base.Start();
        enemy = GetComponent<Enemy>();
        deadInterface = GetComponent<IEnemyDead>();
    }
    protected override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (currentHealth > 0)
        {
            enemy.DamageEffect();
        }
    }

    protected override void Die()
    {
        base.Die();
        deadInterface.DeadAction();
    }

    public override void DoDamage(CharacterStats _targetStats)
    {
        base.DoDamage(_targetStats);
    }
}
