using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player's Item drop")]
    [Range(0, 100)]
    [SerializeField] private int chanceToLostItemsEquipped;
    
    [Range(0, 100)]
    [SerializeField] private int chanceToLostMaterials;

    public override void GenerateDrop()
    {
        CheckDropEquipment();
        CheckDropMaterials();
    }

    private void CheckDropMaterials()
    {
        List<InventoryItem> materialsLost = new List<InventoryItem>();

        foreach (InventoryItem material in InventoryManager.Instance.GetStashList())
        {
            if (Random.Range(0, 100) <= chanceToLostMaterials)
            {
                DropItem(material.data);
                materialsLost.Add(material);
            }
        }
        foreach (InventoryItem material in materialsLost)
        {
            InventoryManager.Instance.RemoveItem(material.data);
        }
    }

    private void CheckDropEquipment()
    {
        List<InventoryItem> itemsToUnequipt = new List<InventoryItem>();
        foreach (InventoryItem inventoryItem in InventoryManager.Instance.GetEquipmentList())
        {
            if (Random.Range(0, 100) <= chanceToLostItemsEquipped)
            {
                DropItem(inventoryItem.data);
                itemsToUnequipt.Add(inventoryItem);
            }
        }

        foreach (InventoryItem itemUnequipt in itemsToUnequipt)
        {
            InventoryManager.Instance.UnEquipItem(itemUnequipt.data as ItemData_Equipment, false);
        }
    }
}
