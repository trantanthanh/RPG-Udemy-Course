using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

public enum StatType
{
    strength,
    agility,
    inteligence,
    vitality,

    damage,
    critChance,
    critPower,

    fireDamage,
    iceDamage,
    lightningDamage,

    maxHealth,
    armor,
    evasion,
    magicResistance
}

[CreateAssetMenu(fileName = "Buff Effect", menuName = "Data/Item Effect/Buff Effect")]
public class BuffEffect_SO : ItemEffect_SO
{
    private PlayerStats playerStats;
    [SerializeField] StatType buffType;
    [SerializeField] int buffAmount;
    [SerializeField] float buffDuration;

    public override void ExecuteEffect(Transform _target)
    {
        playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();

        //Ex:
        //playerStats.IncreaseStatBy(buffAmount, buffDuration, playerStats.iceDamage);
        playerStats.IncreaseStatBy(buffAmount, buffDuration, StatToModify(buffType));
    }

    private Stat StatToModify(StatType _statType)
    {
        switch (_statType)
        {
            case StatType.strength:
                {
                    return playerStats.strength;
                }
            case StatType.inteligence:
                {
                    return playerStats.inteligence;
                }
            case StatType.agility:
                {
                    return playerStats.agility;
                }
            case StatType.vitality:
                {
                    return playerStats.vitality;
                }
            case StatType.damage:
                {
                    return playerStats.damage;
                }
            case StatType.critChance:
                {
                    return playerStats.critChance;
                }
            case StatType.critPower:
                {
                    return playerStats.critPower;
                }
            case StatType.fireDamage:
                {
                    return playerStats.fireDamage;
                }
            case StatType.iceDamage:
                {
                    return playerStats.iceDamage;
                }
            case StatType.lightningDamage:
                {
                    return playerStats.lightningDamage;
                }
            case StatType.maxHealth:
                {
                    return playerStats.maxHealth;
                }
            case StatType.armor:
                {
                    return playerStats.armor;
                }
            case StatType.evasion:
                {
                    return playerStats.evasion;
                }
            case StatType.magicResistance:
                {
                    return playerStats.magicResistance;
                }
        }
        return null;
    }
}
