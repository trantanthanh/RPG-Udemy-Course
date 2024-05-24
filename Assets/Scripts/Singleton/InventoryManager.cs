using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//for manager collect item and remove item in out inventory
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<InventoryItem> inventory = new List<InventoryItem>();//list of all equiptment items
    public Dictionary<ItemData, InventoryItem> inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

    public List<InventoryItem> stash = new List<InventoryItem>();//list of all materials items
    public Dictionary<ItemData, InventoryItem> stashDictionary = new Dictionary<ItemData, InventoryItem>();

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;//this parent (like container) hold all of equipment item slots
    [SerializeField] private Transform stashSlotParent;//this parent (like container) hold all of material item slots

    private UI_ItemSlot[] inventorySlots;//for the equipment items
    private UI_ItemSlot[] stashSlots;//for the material items
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
        inventorySlots = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashSlots = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
    }

    private void UpdateSlotUI()
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            inventorySlots[i].UpdateSlot(inventory[i]);
        }

        for (int i = 0; i < stash.Count; i++)
        {
            stashSlots[i].UpdateSlot(stash[i]);
        }
    }

    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment)
        {
            SeparateItem(_item, inventory, inventoryDictionary);
        }
        else if (_item.itemType == ItemType.Material)
        {
            SeparateItem(_item, stash, stashDictionary);
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
            RemoveSeparateItem(_item, inventory, inventoryDictionary);
        }
        else if (_item.itemType == ItemType.Material)
        {
            RemoveSeparateItem(_item, stash, stashDictionary);
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
