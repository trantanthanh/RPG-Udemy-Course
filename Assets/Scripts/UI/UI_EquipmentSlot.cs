using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

//4 Slots equipment on hero
public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType slotType;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item != null && item.data != null)
        {
            Debug.Log("Unequip item - " + item.data.itemName);
            InventoryManager.Instance.UnEquipItem(item.data as ItemData_Equipment_SO);
        }
        ui.itemTooltip.Hide();
    }

    private void OnValidate()
    {
        gameObject.name = "Equipment slot - " + slotType.ToString();
    }
}
