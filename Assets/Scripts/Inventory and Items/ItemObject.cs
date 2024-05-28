using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This class for items drop in game
public class ItemObject : MonoBehaviour
{
    [SerializeField] ItemData itemData;
    [SerializeField] Vector2 velocity;
    Rigidbody2D rigidbody => GetComponent<Rigidbody2D>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            rigidbody.velocity = velocity;
        }
    }

    private void OnValidate()
    {
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item object - " + itemData.itemName;
    }

    public void PickupItem()
    {
        InventoryManager.Instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
