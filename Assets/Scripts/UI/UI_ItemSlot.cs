using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//update info of item to slot in inventory
public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI itemCountText;

    public InventoryItem item;
    private UI ui;

    private void Start()
    {
        ui = GetComponentInParent<UI>();
    }

    public void UpdateSlot(InventoryItem _newItem)
    {
        item = _newItem;
        itemImage.color = Color.white;
        if (item != null)
        {
            itemImage.sprite = item.data.icon;
            if (item.stackSize > 1)
            {
                itemCountText.text = item.stackSize.ToString();
            }
            else
            {
                itemCountText.text = "";
            }
        }
        else
        {
            itemCountText.text = "";
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item != null && item.data != null)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                InventoryManager.Instance.RemoveItem(item.data);
                return;
            }

            if (item.data.itemType == ItemType.Equipment)
            {
                Debug.Log("Equipped new item - " + item.data.itemName);
                InventoryManager.Instance.EquipItem(item.data);
            }
        }
    }

    public void CleanupSlot()
    {
        item = null;
        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemCountText.text = "";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null || item.data == null) return;
        Debug.Log("Show item info");
        ui.itemTooltip.Show(item.data as ItemData_Equipment_SO);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Hide item info");
        ui.itemTooltip.Hide();
    }
}
