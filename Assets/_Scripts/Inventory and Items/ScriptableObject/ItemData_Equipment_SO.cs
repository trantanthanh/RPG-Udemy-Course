using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "New data Item Equipment", menuName = "Data/Item Equipment")]
public class ItemData_Equipment_SO : ItemData_SO
{
    public EquipmentType equipmentType;

    public ItemEffect_SO[] itemEffects;

    [Header("Item cooldown")]
    public float cooldown;
    [TextArea]
    [SerializeField] string itemEffectDescription;

    [Header("Major stats")]
    public int strength;
    public int agility;
    public int inteligence;
    public int vitality;

    [Header("Magic damage stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightningDamage;

    [Header("Offensive stats")]
    public int damage;
    public int critChance;
    public int critPower;//default value is 150%

    [Header("Defensive stats")]
    public int health;
    public int armor;
    public int evasion;
    public int magicResistance;

    [Header("Craft requirements")]
    public List<InventoryItem> craftingMaterials;

    public void Effect(Transform _target)
    {
        foreach (var item in itemEffects)
        {
            item.ExecuteEffect(_target);
        }
    }

    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier(this.strength);
        playerStats.agility.AddModifier(this.agility);
        playerStats.inteligence.AddModifier(this.inteligence);
        playerStats.vitality.AddModifier(this.vitality);

        playerStats.damage.AddModifier(this.damage);
        playerStats.critChance.AddModifier(this.critChance);
        playerStats.critPower.AddModifier(this.critPower);

        playerStats.fireDamage.AddModifier(this.fireDamage);
        playerStats.iceDamage.AddModifier(this.iceDamage);
        playerStats.lightningDamage.AddModifier(this.lightningDamage);

        playerStats.maxHealth.AddModifier(this.health);
        playerStats.armor.AddModifier(this.armor);
        playerStats.evasion.AddModifier(this.evasion);
        playerStats.magicResistance.AddModifier(this.magicResistance);
    }

    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifier(this.strength);
        playerStats.agility.RemoveModifier(this.agility);
        playerStats.inteligence.RemoveModifier(this.inteligence);
        playerStats.vitality.RemoveModifier(this.vitality);

        playerStats.damage.RemoveModifier(this.damage);
        playerStats.critChance.RemoveModifier(this.critChance);
        playerStats.critPower.RemoveModifier(this.critPower);

        playerStats.maxHealth.RemoveModifier(this.health);
        playerStats.armor.RemoveModifier(this.armor);
        playerStats.evasion.RemoveModifier(this.evasion);
        playerStats.magicResistance.RemoveModifier(this.magicResistance);
    }

    public override string GetDescription()
    {
        stringBuilder.Clear();

        AddItemEffectDescription(itemEffectDescription);

        AddItemDescription(strength, "Strength");
        AddItemDescription(agility, "Agility");
        AddItemDescription(inteligence, "Inteligence");
        AddItemDescription(vitality, "Vitality");

        AddItemDescription(damage, "Damage");
        AddItemDescription(critChance, "CritChance");
        AddItemDescription(critPower, "CritPower");

        AddItemDescription(fireDamage, "Fire Damage");
        AddItemDescription(iceDamage, "Ice Damage");
        AddItemDescription(lightningDamage, "Lightning Damage");

        AddItemDescription(health, "Health");
        AddItemDescription(armor, "Armor");
        AddItemDescription(evasion, "Evasion");
        AddItemDescription(magicResistance, "Magic Resistance");

        return stringBuilder.ToString();
    }

    private void AddItemEffectDescription(string _text)
    {
        if (itemEffectDescription == null || itemEffectDescription.Length == 0) return;

        if (stringBuilder.Length > 0)
        {
            stringBuilder.AppendLine();
        }
        stringBuilder.AppendLine(_text);
    }

    private void AddItemDescription(int _value, string _name)
    {
        if (_value != 0)
        {
            if (stringBuilder.Length > 0)
            {
                stringBuilder.AppendLine();
            }

            stringBuilder.AppendLine($"+ {_value} {_name}");
        }
    }
}
