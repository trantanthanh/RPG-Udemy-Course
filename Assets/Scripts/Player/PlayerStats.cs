using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;
    protected override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
    }
    protected override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        player.DamageEffect();
    }

    protected override void Die()
    {
        base.Die();
        player.stateMachine.ChangeState(player.deadState);
    }

    public override void DoDamage(CharacterStats _targetStats)
    {
        base.DoDamage(_targetStats);
    }
}
