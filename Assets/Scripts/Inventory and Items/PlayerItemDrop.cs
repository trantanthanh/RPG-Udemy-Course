using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player's Item drop")]
    [Range(0, 100)]
    [SerializeField] private int chanceToLostItem;

    public override void GenerateDrop()
    {
        Debug.Log("Player drop item");
        InventoryManager inventoryManager = InventoryManager.Instance;
        List<InventoryItem> currentEquiptment = inventoryManager.GetEquipmentList();
        List<InventoryItem> itemsToUnequipt = new List<InventoryItem>();

        foreach (InventoryItem inventoryItem in currentEquiptment)
        {
            if (Random.Range(0, 100) <= chanceToLostItem)
            {
                DropItem(inventoryItem.data);
                itemsToUnequipt.Add(inventoryItem);
            }
        }

        foreach (InventoryItem itemUnequipt in itemsToUnequipt)
        {
            inventoryManager.UnEquipItem(itemUnequipt.data as ItemData_Equipment, false);
        }
    }
}
