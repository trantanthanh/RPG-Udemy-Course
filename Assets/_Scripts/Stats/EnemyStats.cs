using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    [SerializeField] private int level = 1;//start from level 1
    [Range(0f, 1f)]
    [SerializeField] private float percentageLevelModifier = 0.4f;//increase 40% per level

    [HideInInspector]
    public Stat soulsDropAmout;

    Enemy enemy;
    protected override void Start()
    {
        soulsDropAmout.SetDefaultValue(100);
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
        Modify(this.soulsDropAmout);
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
        enemy.GetComponent<ItemDrop>()?.GenerateDrop();//Drop item if can
        PlayerManager.Instance.UpdateCurrency(this.soulsDropAmout.GetValue());

        Destroy(gameObject, 5f);//Destroy after 5s
    }

    //public override void DoDamage(CharacterStats _targetStats, float amplifierDamagePercent = 0)
    //{
    //    base.DoDamage(_targetStats, amplifierDamagePercent);
    //}
}
