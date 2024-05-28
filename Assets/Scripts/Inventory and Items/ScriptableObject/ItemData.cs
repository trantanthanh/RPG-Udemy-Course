using UnityEngine;


//for the define item object
public enum ItemType
{
    Material,
    Equipment
}

[CreateAssetMenu(fileName = "New Data Item", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;

    [Range(0, 100)]
    public int dropChance;
}
