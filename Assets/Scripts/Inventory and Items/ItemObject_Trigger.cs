using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class for items to collect in game scene
public class ItemObject_Trigger : MonoBehaviour
{
    private ItemObject myItemObject => GetComponentInParent<ItemObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            myItemObject.PickupItem();
        }
    }
}
