using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//for manager collect item and remove item in out inventory
public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public List<InventoryItem> inventoryItems = new List<InventoryItem>();//list of all equiptment items
    public Dictionary<ItemData, InventoryItem> inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

    public List<InventoryItem> stashItems = new List<InventoryItem>();//list of all materials items
    public Dictionary<ItemData, InventoryItem> stashDictionary = new Dictionary<ItemData, InventoryItem>();

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;//this parent (like container) hold all of equipment item slots
    [SerializeField] private Transform stashSlotParent;//this parent (like container) hold all of material item slots

    private UI_ItemSlot[] inventoryItemSlot;//for the equipment items
    private UI_ItemSlot[] stashItemSlot;//for the material items
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }
        Destroy(gameObject);
    }

    void Start()
    {
        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
    }

    private void UpdateSlotUI()
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventoryItems[i]);
        }

        for (int i = 0; i < stashItems.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stashItems[i]);
        }
    }

    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment)
        {
            SeparateItem(_item, inventoryItems, inventoryDictionary);
        }
        else if (_item.itemType == ItemType.Material)
        {
            SeparateItem(_item, stashItems, stashDictionary);
        }
        UpdateSlotUI();
    }

    private void SeparateItem(ItemData _item, List<InventoryItem> _kindListItems, Dictionary<ItemData, InventoryItem> _kindDictionary)
    {
        if (_kindDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            _kindListItems.Add(newItem);
            _kindDictionary.Add(_item, newItem);
        }
    }
    public void RemoveItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment)
        {
            RemoveSeparateItem(_item, inventoryItems, inventoryDictionary);
        }
        else if (_item.itemType == ItemType.Material)
        {
            RemoveSeparateItem(_item, stashItems, stashDictionary);
        }

        UpdateSlotUI();
    }

    private void RemoveSeparateItem(ItemData _item, List<InventoryItem> _kindListItems, Dictionary<ItemData, InventoryItem> _kindDictionary)
    {
        if (_kindDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                _kindListItems.Remove(value);
                _kindDictionary.Remove(_item);
            }
            else
            {
                value.RemoveStack();
            }
        }
    }
}
