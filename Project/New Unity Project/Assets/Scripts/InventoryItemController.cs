<<<<<<< HEAD
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItemController : MonoBehaviour
{
    ItemData item;
    public Button RemoveButton;
    TMP_InputField inputField;

    void Start()
    {                                                                                                                                                                                                                                                                                                                                                                                                                              
        // Find the TMP Input Field component
        inputField = FindObjectOfType<TMP_InputField>();

        if (inputField == null)
        {
            Debug.LogError("No TextMeshPro Input Field found!");
        }
    }

    public void RemoveItem()
    {
        if (item != null)
        {
            // Retrieve the text entered by the user
            string text = inputField.text;

            // Parse the text to an integer
            int quantityToRemove;
            if (!int.TryParse(text, out quantityToRemove))
            {
                Debug.LogError("Invalid input for quantity.");
                return;
            }

            if (item.quantity > 1)
            {
                // If quantity is greater than 1, decrease the quantity
                item.quantity -= quantityToRemove;
                if (item.quantity <= 0)
                {
                    item.quantity = 1;
                }
            }
            else
            {
                // If quantity is 1, remove the item from the inventory
                InventoryManager.Instance.Remove(item);
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
}
=======
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemController : MonoBehaviour
{
    ItemData item;//item
    
    public Button RemoveButton;

    public void RemoveItem(){
        InventoryManager.Instance.Remove(item);
        
        Destroy(gameObject);
    }

    public void AddItem(ItemData newItem){
        item = newItem;
    }
}
>>>>>>> 10362fada20fb5a04301176b02b25bf49290ebc3
