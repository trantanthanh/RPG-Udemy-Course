using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Major stats")]
    public Stat strength;//1 point increase damage by 1 and critical power by 1%
    public Stat agility;//1 point increase evasion by 1% and crit chance by 1%
    public Stat inteligence;//1 point increase magic damage by 1 and magic resistance by 3
    public Stat vitality;//1 point increase health by 3 or 5 points

    [Header("Defensive stats")]
    public Stat maxHealth;
    public Stat evasion;
    public Stat armor;

    public Stat damage;
    protected int currentHealth;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentHealth = maxHealth.GetValue();
    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {

        if (CheckTargetCanAvoidAttack(_targetStats)) return;

        int totalDamage = damage.GetValue() + strength.GetValue();
        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        _targetStats.TakeDamage(totalDamage);//Apply damage to target
    }

    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        totalDamage -= _targetStats.armor.GetValue();
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    private bool CheckTargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalTargetEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();
        if (Random.Range(0, 100) < totalTargetEvasion)
        {
            return true;
        }
        return false;
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    protected virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    protected virtual void Die()
    {

    }
}
