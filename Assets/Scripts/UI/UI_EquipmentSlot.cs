using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType slotType;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item != null)
        {
            Debug.Log("Unequip item - " + item.data.itemName);
            ItemData_Equipment newItemEquipment = item.data as ItemData_Equipment;
            InventoryManager.Instance.UnEquipItem(newItemEquipment);
        }
    }

    private void OnValidate()
    {
        gameObject.name = "Equipment slot - " + slotType.ToString();
    }
}
