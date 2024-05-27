using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

//for manager collect item and remove item in out inventory
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<InventoryItem> equipment = new List<InventoryItem>();
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

    public List<InventoryItem> inventory = new List<InventoryItem>();//list of all equiptment items
    public Dictionary<ItemData, InventoryItem> inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

    public List<InventoryItem> stash = new List<InventoryItem>();//list of all materials items
    public Dictionary<ItemData, InventoryItem> stashDictionary = new Dictionary<ItemData, InventoryItem>();

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;//this parent (like container) hold all of equipment item slots
    [SerializeField] private Transform stashSlotParent;//this parent (like container) hold all of material item slots
    [SerializeField] private Transform equipmentSlotParent;//this parent (like container) hold all of equipped items

    private UI_ItemSlot[] inventorySlots;//for the equipment items
    private UI_ItemSlot[] stashSlots;//for the material items
    private UI_EquipmentSlot[] equipmentSlots;//for items equipped 
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        inventorySlots = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashSlots = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlots = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
    }

    private void UpdateSlotUI()
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].CleanupSlot();

            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            {
                if (equipmentSlots[i].slotType == item.Key.equipmentType)
                {
                    equipmentSlots[i].UpdateSlot(item.Value);
                }
            }
        }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].CleanupSlot();
        }
        for (int i = 0; i < inventory.Count; i++)
        {
            inventorySlots[i].UpdateSlot(inventory[i]);
        }

        for (int i = 0; i < stashSlots.Length; i++)
        {
            stashSlots[i].CleanupSlot();
        }
        for (int i = 0; i < stash.Count; i++)
        {
            stashSlots[i].UpdateSlot(stash[i]);
        }
    }

    public void EquipItem(ItemData _item)
    {
        ItemData_Equipment newItemEquipment = _item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(_item);

        UnEquipItem(newItemEquipment);

        equipment.Add(newItem);
        equipmentDictionary.Add(newItemEquipment, newItem);

        newItemEquipment.AddModifiers();//Apply modifier to player

        RemoveItem(_item);//Remove out inventory
    }

    public void UnEquipItem(ItemData_Equipment itemToUnequip)
    {
        ItemData_Equipment oldEquipment = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == itemToUnequip.equipmentType)
            {
                oldEquipment = item.Key;
                break;
            }
        }

        if (oldEquipment != null)
        {
            oldEquipment.RemoveModifiers();//Remove modifiers are applied
            //Remove item already have for replace new (ex: change new weapon)
            if (equipmentDictionary.TryGetValue(oldEquipment, out InventoryItem value))
            {
                equipment.Remove(value);
                equipmentDictionary.Remove(oldEquipment);
            }
            AddItem(oldEquipment);//Add to inventory
        }
    }

    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment)
        {
            AddToInventory(_item);
        }
        else if (_item.itemType == ItemType.Material)
        {
            AddToStash(_item);
        }
        UpdateSlotUI();
    }

    public void RemoveItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment)
        {
            RemoveItemFromInventory(_item);
        }
        else if (_item.itemType == ItemType.Material)
        {
            RemoveItemFromStash(_item);
        }


        UpdateSlotUI();
    }

    #region Stash
    private void AddToStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }
    private void RemoveItemFromStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                stash.Remove(value);
                stashDictionary.Remove(_item);
            }
            else
            {
                value.RemoveStack();
            }
        }
    }
    #endregion

    #region Inventory
    private void AddToInventory(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    private void RemoveItemFromInventory(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
            {
                value.RemoveStack();
            }
        }
    }
    #endregion
    public bool CanCraft(ItemData_Equipment _itemToCraft, List<InventoryItem> _requiredMaterials)
    {
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();
        for (int i = 0; i < _requiredMaterials.Count; i++)
        {
            if (stashDictionary.TryGetValue(_requiredMaterials[i].data, out InventoryItem stashValue))
            {
                if (stashValue.stackSize < _requiredMaterials[i].stackSize)
                {
                    Debug.Log("Not enough material - " + stashValue.data.itemName);
                    return false;
                }
                //add this to used material
                materialsToRemove.Add(stashValue);
            }
            else
            {
                Debug.Log("Not enough materials");
                return false;
            }
        }

        for (int i = 0; i < materialsToRemove.Count; i++)
        {
            RemoveItem(materialsToRemove[i].data);
        }

        AddItem(_itemToCraft);
        return true;
    }
}
