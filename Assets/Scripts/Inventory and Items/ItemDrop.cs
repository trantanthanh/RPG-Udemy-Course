using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Attach to enemy and setup item to drop
public class ItemDrop : MonoBehaviour
{
    [SerializeField] int possibleItemsDrop;
    [SerializeField] ItemData[] possibleDrop;
    private List<ItemData> dropList = new List<ItemData>();

    [SerializeField] private GameObject dropPrefab;

    public virtual void GenerateDrop()
    {
        //Generate drop list
        for (int i = 0; i < possibleDrop.Length; i++)
        {
            if (Random.Range(0, 100) <= possibleDrop[i].dropChance)
            {
                dropList.Add(possibleDrop[i]);
            }
        }

        //Drop items
        for (int i = 0; i < possibleItemsDrop; i++)
        {
            ItemData itemDrop = dropList[Random.Range(0, dropList.Count)];
            dropList.Remove(itemDrop);
            DropItem(itemDrop);
            if (dropList.Count <= 0)
            {
                break;
            }
        }
    }

    protected void DropItem(ItemData _itemData)
    {
        if (_itemData != null)
        {
            GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);
            Vector2 randomVecolity = new Vector2(Random.Range(-5, 5), Random.Range(12, 15));

            newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVecolity);
        }
    }
}
