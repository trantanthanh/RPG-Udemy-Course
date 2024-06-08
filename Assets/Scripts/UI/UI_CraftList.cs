using Menu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//SetupCraftList - Weapon, Armor, Amulet, Flask
public class UI_CraftList : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Transform craftSlotParent;
    [SerializeField] private GameObject craftSlotPrefab;

    [SerializeField] private List<ItemData_Equipment_SO> craftEquipment;
    private static bool isInit = false;
    // Start is called before the first frame update
    void Start()
    {
        if (!isInit)
        {
            isInit = true;
            transform.parent.GetChild(0).gameObject.GetComponent<UI_CraftList>().SetupCraftList();//button weapon craft list
            SetupDefaultCraftWindow();
            Debug.Log("Init craft window");
        }
    }

    public void SetupCraftList()
    {
        //clear all exist
        for (int i = 0; i < craftSlotParent.childCount; i++)
        {
            Destroy(craftSlotParent.GetChild(i).gameObject);
        }

        for (int i = 0; i < craftEquipment.Count; i++)
        {
            GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotParent);
            newSlot.GetComponent<UI_CraftSlot>().SetupCraftSlot(craftEquipment[i]);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetupCraftList();
    }

    public void SetupDefaultCraftWindow()
    {
        if (craftEquipment.Count > 0)
        {
            GetComponentInParent<UI>().craftWindow.SetupCraftWindow(craftEquipment[0]);
        }
    }
}
