using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;

    public void Show(ItemData_Equipment_SO _item)
    {
        if (_item == null) return;//can't explicit material (ItemData_SO) to ItemData_Equipment_SO, it'll be null
        itemNameText.text = _item.itemName;
        itemTypeText.text = _item.equipmentType.ToString();
        itemDescriptionText.text = _item.GetDescription();

        gameObject.SetActive(true);
    }

    public void Hide() => gameObject.SetActive(false);
}
