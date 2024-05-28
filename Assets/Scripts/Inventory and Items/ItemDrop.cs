using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Attach to enemy and setup item to drop
public class ItemDrop : MonoBehaviour
{
    [SerializeField] private GameObject dropPrefab;
    [SerializeField] private ItemData itemData;

    public void DropItem()
    {
        if (itemData != null)
        {
            GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);
            Vector2 randomVecolity = new Vector2(Random.Range(-5, 5), Random.Range(12, 15));

            newDrop.GetComponent<ItemObject>().SetupItem(itemData, randomVecolity);
        }
    }
}
