using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This class for items drop in game
public class ItemObject : MonoBehaviour
{
    [SerializeField] ItemData_SO itemData;
    Rigidbody2D myRigidBody => GetComponent<Rigidbody2D>();

    public void SetupItem(ItemData_SO _item, Vector2 _velocity)
    {
        itemData = _item;
        myRigidBody.velocity = _velocity;
        UpdateIconAndName();
    }

    private void OnValidate()
    {
        UpdateIconAndName();
    }

    private void UpdateIconAndName()
    {
        if (itemData == null) return;
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item object - " + itemData.itemName;
    }

    public void PickupItem()
    {
        Debug.Log("Pickup item - " + itemData.itemName);
        if (!InventoryManager.Instance.CanAddItem(itemData))
        {
            myRigidBody.velocity = new Vector2(0, 7);
            return;
        }

        InventoryManager.Instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
