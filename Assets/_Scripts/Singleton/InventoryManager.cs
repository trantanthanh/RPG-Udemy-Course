using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using static UnityEditor.Progress;

//for manager collect item and remove item in out inventory
public class InventoryManager : MonoBehaviour, ISaveManager
{
    public static InventoryManager Instance;

    [SerializeField] List<ItemData_SO> startingItems = new List<ItemData_SO>();

    public List<InventoryItem> equipment = new List<InventoryItem>();//list items are equipped
    public Dictionary<ItemData_Equipment_SO, InventoryItem> equipmentDictionary = new Dictionary<ItemData_Equipment_SO, InventoryItem>();

    public List<InventoryItem> inventory = new List<InventoryItem>();//list of all equiptment items
    public Dictionary<ItemData_SO, InventoryItem> inventoryDictionary = new Dictionary<ItemData_SO, InventoryItem>();

    public List<InventoryItem> stash = new List<InventoryItem>();//list of all materials items
    public Dictionary<ItemData_SO, InventoryItem> stashDictionary = new Dictionary<ItemData_SO, InventoryItem>();

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;//this parent (like container) hold all of equipment item slots
    [SerializeField] private Transform stashSlotParent;//this parent (like container) hold all of material item slots
    [SerializeField] private Transform equipmentSlotParent;//this parent (like container) hold all of equipped items

    [SerializeField] private Transform statSlotParent;//UI Stats menu

    private UI_ItemSlot[] inventorySlots;//for the equipment items
    private UI_ItemSlot[] stashSlots;//for the material items
    private UI_EquipmentSlot[] equipmentSlots;//for items equipped 

    private UI_StatSlot[] statSlots;

    float flaskCooldown;
    float lastTimeUsedFlask = 0f;
    public float FlaskCooldown { get => flaskCooldown; }

    float armorCooldown;
    float lastTimeUsedArmorEffect = 0f;


    [Header("Data base")]
    public List<InventoryItem> loadedItems;

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

        statSlots = statSlotParent.GetComponentsInChildren<UI_StatSlot>();
        AddStartingItems();
    }

    private void AddStartingItems()
    {
        if (loadedItems.Count > 0)
        {
            foreach (InventoryItem item in loadedItems)
            {
                for (int i = 0; i < item.stackSize; i++)
                {
                    AddItem(item.data);
                }
            }

            return;
        }


        for (int i = 0; i < startingItems.Count; i++)
        {
            if (CanAddItem(startingItems[i]))
            {
                AddItem(startingItems[i]);
            }
        }
    }

    private void UpdateSlotUI()
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].CleanupSlot();

            foreach (KeyValuePair<ItemData_Equipment_SO, InventoryItem> item in equipmentDictionary)
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

        for (int i = 0; i < statSlots.Length; i++)//Update infor of Character stats in UI
        {
            statSlots[i].UpdateStatValueUI();
        }
    }

    public void EquipItem(ItemData_SO _item)
    {
        ItemData_Equipment_SO newItemEquipment = _item as ItemData_Equipment_SO;
        InventoryItem newItem = new InventoryItem(_item);

        UnEquipItem(newItemEquipment);

        equipment.Add(newItem);
        equipmentDictionary.Add(newItemEquipment, newItem);

        newItemEquipment.AddModifiers();//Apply modifier to player

        RemoveItem(_item);//Remove out inventory
    }

    public void UnEquipItem(ItemData_Equipment_SO itemToUnequip, bool needReturnToInventory = true)
    {
        ItemData_Equipment_SO oldEquipment = null;

        foreach (KeyValuePair<ItemData_Equipment_SO, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == itemToUnequip.equipmentType)
            {
                oldEquipment = item.Key;
                break;
            }
        }

        if (oldEquipment != null && CanAddItem(oldEquipment))
        {
            oldEquipment.RemoveModifiers();//Remove modifiers are applied
            //Remove item already have for replace new (ex: change new weapon)
            if (equipmentDictionary.TryGetValue(oldEquipment, out InventoryItem value))
            {
                equipment.Remove(value);
                equipmentDictionary.Remove(oldEquipment);
            }
            if (needReturnToInventory)
            {
                AddItem(oldEquipment);//Add to inventory
            }
            else
            {
                UpdateSlotUI();
            }
        }
    }

    public bool CanAddItem(ItemData_SO _item)
    {
        if (_item.itemType == ItemType.Equipment)
        {
            if (inventory.Count >= inventorySlots.Length)
            {
                if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
                {
                    return true;
                }
                else
                {
                    Debug.Log("No more inventory slot to add");
                    return false;
                }
            }

        }
        else if (_item.itemType == ItemType.Material)
        {
            if (stash.Count >= stashSlots.Length)
            {
                if (stashDictionary.TryGetValue(_item, out InventoryItem value))
                {
                    return true;
                }
                else
                {
                    Debug.Log("No more stash slot to add");
                    return false;
                }
            }
        }
        return true;
    }

    public void AddItem(ItemData_SO _item)
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

    public void RemoveItem(ItemData_SO _item)
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

    public void UseFlask()
    {
        ItemData_Equipment_SO currentFlask = GetEquipment(EquipmentType.Flask);
        if (currentFlask == null) return;

        if (Time.time >= flaskCooldown + lastTimeUsedFlask)
        {
            PlayerManager.Instance.uiIngame.SetFlaskCooldown();
            flaskCooldown = currentFlask.cooldown;
            lastTimeUsedFlask = Time.time;
            currentFlask.Effect(null);
            //Debug.Log("Use flask");
        }
        else
        {
            //Debug.Log("Use flask is cooldown");
        }
    }

    public bool CanUseArmorEffect()
    {
        ItemData_Equipment_SO currentArmor = GetEquipment(EquipmentType.Armor);
        if (currentArmor == null) return false;
        PlayerStats playerStat = PlayerManager.Instance.player.GetComponent<PlayerStats>();
        if (playerStat.CurrentHealth > (playerStat.GetMaxHealth() / 10)) return false;//Only use armor effect when HP below 10%
        if (Time.time >= armorCooldown + lastTimeUsedArmorEffect)
        {
            armorCooldown = currentArmor.cooldown;
            lastTimeUsedArmorEffect = Time.time;
            return true;
        }
        return false;
    }

    #region Stash
    private void AddToStash(ItemData_SO _item)
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
    private void RemoveItemFromStash(ItemData_SO _item)
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
    private void AddToInventory(ItemData_SO _item)
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

    private void RemoveItemFromInventory(ItemData_SO _item)
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
    public bool CanCraft(ItemData_Equipment_SO _itemToCraft, List<InventoryItem> _requiredMaterials)
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
                for (int j = 0; j < _requiredMaterials[i].stackSize; j++)
                {
                    materialsToRemove.Add(stashValue);
                }
            }
            else
            {
                Debug.Log("Not enough materials");
                return false;
            }
        }

        if (CanAddItem(_itemToCraft))
        {
            for (int i = 0; i < materialsToRemove.Count; i++)
            {
                RemoveItem(materialsToRemove[i].data);
            }

            AddItem(_itemToCraft);
            return true;
        }
        return false;
    }

    public List<InventoryItem> GetEquipmentList() => equipment;
    public List<InventoryItem> GetStashList() => stash;

    public ItemData_Equipment_SO GetEquipment(EquipmentType _type)
    {
        ItemData_Equipment_SO equippedItem = null;
        foreach (KeyValuePair<ItemData_Equipment_SO, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == _type)
            {
                equippedItem = item.Key;
                break;
            }
        }
        return equippedItem;
    }

    public void LoadData(GameData _data)
    {
        Debug.Log("Item loaded");
        foreach (KeyValuePair<string, int> pair in _data.inventory)
        {
            foreach (ItemData_SO item in GetItemDataBase())
            {
                if (item != null && item.itemId == pair.Key)
                {
                    InventoryItem itemToLoad = new InventoryItem(item);
                    itemToLoad.stackSize = pair.Value;

                    loadedItems.Add(itemToLoad);
                }
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.inventory.Clear();

        foreach (KeyValuePair<ItemData_SO, InventoryItem> item in inventoryDictionary)
        {
            _data.inventory.Add(item.Key.itemId, item.Value.stackSize);
        }
    }

    private List<ItemData_SO> GetItemDataBase()
    {
        List<ItemData_SO> itemDataBase = new List<ItemData_SO>();
#if UNITY_EDITOR
        string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/_Data" });
#endif

        foreach (string SOName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData_SO>(SOpath);
            itemDataBase.Add(itemData);
        }

        return itemDataBase;
    }
}
