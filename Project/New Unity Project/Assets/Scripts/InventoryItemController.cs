using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItemController : MonoBehaviour
{
       ItemData item;
    public Button RemoveButton;

     public void RemoveItem()
    {
        if (item != null)
        {

            if (item.quantity > 1)
            {
                // If quantity is greater than 1, decrease the quantity
                item.quantity--;
                if (item.quantity <= 0)
                {
                    item.quantity = 1;
                }
            }
            else
            {
                // If quantity is 1, remove the item from the inventory
                InventoryManager.Instance.Remove(this.item);
                Destroy(gameObject);
            }
        }
        else
        {
            Debug.LogWarning("Item is null in RemoveItem() method.");
        }
    }

    public void AddItem(ItemData newItem)
    {
        item = newItem;
    }

    public void ItemUsage()
    {
        switch (item.itemType)
        {
            case ItemData.ItemType.KitMedico:
                player.Instance.IncreaseHP(item.Value);
                item.quantity--;
                break;
        }
        if (item.quantity <= 0)
        {
            InventoryManager.Instance.Remove(item);
            Destroy(gameObject);
            item.quantity = 1;
        }
    }
}
