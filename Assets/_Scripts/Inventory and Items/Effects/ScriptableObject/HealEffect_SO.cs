using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal Effect", menuName = "Data/Item Effect/Heal Effect")]
public class HealEffect_SO : ItemEffect_SO
{
    [Range(0f, 1f)]
    [SerializeField] float healPercent;
    public override void ExecuteEffect(Transform _target)
    {
        PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();
        int healAmount = Mathf.RoundToInt(playerStats.GetMaxHealth() * healPercent);

        playerStats.RestoreHealthBy(healAmount);
    }
}
