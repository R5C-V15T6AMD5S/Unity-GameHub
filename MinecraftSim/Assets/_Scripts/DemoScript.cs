using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScript : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item[] itemsToPickup;

    public void PickupItem(int id)
    {
       bool result = inventoryManager.AddItem(itemsToPickup[id]);
        if(result)
        {
            Debug.Log("Item added");
        } else
        {
            Debug.Log("Item not added");
        }
    }

    public void GetSelectedItem()
    {
        Item receivedItem = inventoryManager.GetSelectedItem(false);
        if(receivedItem != null)
        {
            Debug.Log("Received" + receivedItem);
        } else
        {
            Debug.Log("Not Received");
        }
    }

    public void UseSelectedItem()
    {
        Item receivedItem = inventoryManager.GetSelectedItem(true);
        if (receivedItem != null)
        {
            Debug.Log("Used" + receivedItem);
        }
        else
        {
            Debug.Log("Not used Received");
        }
    }
}
