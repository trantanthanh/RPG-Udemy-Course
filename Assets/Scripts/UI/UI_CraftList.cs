using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//SetupCraftList - Weapon
public class UI_CraftList : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Transform craftSlotParent;
    [SerializeField] private GameObject craftSlotPrefab;

    [SerializeField] private List<ItemData_Equipment_SO> craftEquipment;
    private List<UI_CraftSlot> craftSlots = new List<UI_CraftSlot>();

    // Start is called before the first frame update
    void Start()
    {
        AssignCraftSlots();
    }

    private void AssignCraftSlots()
    {
        for (int i = 0; i < craftSlotParent.childCount; i++)
        {
            craftSlots.Add(craftSlotParent.GetChild(i).GetComponent<UI_CraftSlot>());
        }
    }

    public void SetupCraftList()
    {
        //clear all exist
        for (int i = 0; i < craftSlots.Count; i++)
        {
            Destroy(craftSlots[i].gameObject);
        }

        //creat new list
        craftSlots = new List<UI_CraftSlot>();
        for (int i = 0; i < craftEquipment.Count; i++)
        {
            GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotParent);
            newSlot.GetComponent<UI_CraftSlot>().SetupCraftSlot(craftEquipment[i]);
            craftSlots.Add(newSlot.GetComponent<UI_CraftSlot>());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetupCraftList();
    }
}
