using System.Collections;
using UnityEngine;

public class DeathBringerSpellCastState : EnemyState
{
    EnemyDeathBringer enemy;
    public DeathBringerSpellCastState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animName, EnemyDeathBringer _enemy) : base(_enemyBase, _stateMachine, _animName)
    {
        this.enemy = _enemy;
    }
}
