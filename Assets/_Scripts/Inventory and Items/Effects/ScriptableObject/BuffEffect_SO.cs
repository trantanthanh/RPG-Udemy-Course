using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

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
        playerStats.IncreaseStatBy(buffAmount, buffDuration, playerStats.GetBaseStat(buffType));
    }
}
