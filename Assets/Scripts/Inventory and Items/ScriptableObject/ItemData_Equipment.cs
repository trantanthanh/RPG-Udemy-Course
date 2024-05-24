using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "New data Item Equipment", menuName = "Data/Item Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;
}
