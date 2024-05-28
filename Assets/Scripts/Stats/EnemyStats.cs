using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    [SerializeField] private int level = 1;//start from level 1
    [Range(0f, 1f)]
    [SerializeField] private float percentageLevelModifier = 0.4f;//increase 40% per level
    Enemy enemy;
    protected override void Start()
    {
        ApplyModifiers();

        base.Start();
        enemy = GetComponent<Enemy>();
    }

    private void ApplyModifiers()
    {
        //Modify(this.strength);
        //Modify(this.agility);
        //Modify(this.inteligence);
        //Modify(this.vitality);

        Modify(this.fireDamage);
        Modify(this.iceDamage);
        Modify(this.lightningDamage);

        Modify(this.damage);
        Modify(this.critChance);
        Modify(this.critPower);

        Modify(this.maxHealth);
        Modify(this.armor);
        Modify(this.evasion);
        Modify(this.magicResistance);
    }

    private void Modify(Stat _stat)
    {
        for (int i = 1; i < level; i++)
        {
            float modifier = _stat.GetValue() * percentageLevelModifier;
            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (IsAlive())
        {
            enemy.DamageImpact();
            enemy.DamageEffect();
        }
    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();
        enemy.GetComponent<ItemDrop>()?.DropItem();//Drop item if can
    }

    public override void DoDamage(CharacterStats _targetStats)
    {
        base.DoDamage(_targetStats);
    }
}
