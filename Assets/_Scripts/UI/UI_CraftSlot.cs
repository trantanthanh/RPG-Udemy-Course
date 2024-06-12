using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    [SerializeField] protected TextMeshProUGUI itemName;
    protected override void Start()
    {
        base.Start();
    }

    public void SetupCraftSlot(ItemData_Equipment_SO _data)
    {
        if (_data == null) return;
        item.data = _data;
        itemImage.sprite = _data.icon;
        itemName.text = _data.itemName;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        //Inventory craft data
        ItemData_Equipment_SO craftData = item.data as ItemData_Equipment_SO;
        if (craftData != null)
        {
            ui.craftWindow.SetupCraftWindow(craftData);
            //if (InventoryManager.Instance.CanCraft(craftData, craftData.craftingMaterials))
            //{
            //    //Inform to user
            //}
        }
    }
}
