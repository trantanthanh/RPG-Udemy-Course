using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Image[] materialsImage;

    [SerializeField] private Button craftButton;

    public void SetupCraftWindow(ItemData_Equipment_SO _data)
    {
        craftButton.onClick.RemoveAllListeners();

        for (int i = 0; i < materialsImage.Length; i++)
        {
            //Hide icon and text amount
            materialsImage[i].color = Color.clear;
            materialsImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }

        for (int i = 0; i < _data.craftingMaterials.Count; i++)
        {
            if (_data.craftingMaterials.Count > materialsImage.Length)
            {
                Debug.Log("You have more materials than you have materials slots in craft window");
            }

            materialsImage[i].sprite = _data.craftingMaterials[i].data.icon;
            materialsImage[i].color = Color.white;//show

            TextMeshProUGUI materialText = materialsImage[i].GetComponentInChildren<TextMeshProUGUI>();
            materialText.text = _data.craftingMaterials[i].stackSize.ToString();
            materialText.color = Color.white;//show
        }

        itemIcon.sprite = _data.icon;
        itemName.text = _data.itemName;
        itemDescription.text = _data.GetDescription();

        craftButton.onClick.AddListener(() => InventoryManager.Instance.CanCraft(_data, _data.craftingMaterials));
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
