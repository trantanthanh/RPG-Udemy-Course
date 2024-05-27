using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    private void OnEnable()
    {
        UpdateSlot(item);
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        //Inventory craft data
        ItemData_Equipment craftData = item.data as ItemData_Equipment;
        if (InventoryManager.Instance.CanCraft(craftData, craftData.craftingMaterials))
        {
            //Inform to user
        }

    }
}
