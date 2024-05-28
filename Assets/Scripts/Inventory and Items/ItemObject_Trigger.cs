using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class for items to collect in game scene
public class ItemObject_Trigger : MonoBehaviour
{
    private ItemObject myItemObject => GetComponentInParent<ItemObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null && player.stats.IsAlive())
        {
            myItemObject.PickupItem();
        }
    }
}
