using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
//for the define item object
public enum ItemType
{
    Material,
    Equipment
}

[CreateAssetMenu(fileName = "New Data Item", menuName = "Data/Item")]
public class ItemData_SO : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;
    public string itemId;

    [Range(0, 100)]
    public int dropChance;

    protected StringBuilder stringBuilder = new StringBuilder();

    public virtual string GetDescription()
    {
        return "";
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        string path = AssetDatabase.GetAssetPath(this);
        itemId = AssetDatabase.AssetPathToGUID(path);
    }
#endif
}
