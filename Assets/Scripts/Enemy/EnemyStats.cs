using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    Enemy enemy;
    protected override void Start()
    {
        base.Start(); 
        enemy = GetComponent<Enemy>();
    }
    protected override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        enemy.DamageEffect();
    }

    protected override void Die()
    {
        base.Die();
    }

    public override void DoDamage(CharacterStats _targetStats)
    {
        base.DoDamage(_targetStats);
    }
}
