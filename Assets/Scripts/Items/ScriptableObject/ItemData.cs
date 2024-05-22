using UnityEngine;

[CreateAssetMenu(fileName = "New Data Item", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
}
