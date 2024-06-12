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
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (currentHealth > 0)
        {
            player.DamageImpact();
            player.DamageEffect();
        }

    }

    protected override void Die()
    {
        base.Die();
        player.Die();
        player.GetComponent<PlayerItemDrop>()?.GenerateDrop();//Drop item if can
    }

    public override void DoDamage(CharacterStats _targetStats)
    {
        base.DoDamage(_targetStats);
    }

    protected override void TakeDamageWithoutEffect(int damage)
    {
        base.TakeDamageWithoutEffect(damage);

        ItemData_Equipment_SO currentArmor = InventoryManager.Instance.GetEquipment(EquipmentType.Armor);
        if (currentArmor != null)
        {
            currentArmor.Effect(player.transform);
        }
    }
}
