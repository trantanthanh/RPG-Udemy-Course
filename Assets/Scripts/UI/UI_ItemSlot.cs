using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//update info of item to slot in inventory
public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI itemText;

    public InventoryItem item;

    public void UpdateSlot(InventoryItem _newItem)
    {
        item = _newItem;
        itemImage.color = Color.white;
        if (item != null)
        {
            itemImage.sprite = item.data.icon;
            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
        else
        {
            itemText.text = "";
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (item != null)
        {
            if (item.data.itemType == ItemType.Equipment)
            {
                Debug.Log("Equiped new item - " + item.data.itemName);
                InventoryManager.Instance.EquipItem(item.data);
            }
        }
    }
}