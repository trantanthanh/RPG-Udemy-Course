using System;


//this class for item show in inventory
//have data info of item and number stack of that item
[Serializable]
public class InventoryItem
{
    public ItemData data;
    public int stackSize;

    public InventoryItem(ItemData _newItemData)
    {
        data = _newItemData;
        //TODO: add to stack
        AddStack();//for 1st collect
    }

    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
}
