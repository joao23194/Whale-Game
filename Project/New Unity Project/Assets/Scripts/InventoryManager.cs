using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<ItemData> Items = new List<ItemData>();
    public Transform ItemContent;
    public GameObject InventoryItem;
    public Toggle EnableRemove;
    public InventoryItemController[] InventoryItems;

    private int auxQTD = 0;
    private string auxName;

    private void Awake()
    {
        Instance = this;

        // Reset all item quantities to their default values
        ResetAllItemQuantities();
    }

    private void ResetAllItemQuantities()
    {
        foreach (var item in Resources.FindObjectsOfTypeAll<ItemData>()) //encontra todos os objetos do tipo ItemData para depois fazer reset ao valor
        {
            item.ResetQuantity();
        }
    }
    
    public void Add(ItemData item)
    {
        // Boolean to check if the item was added
        bool itemAdded = false;

        foreach (var dupe in Items)
        {
            // Check if the item has the same name
            if (dupe.itemName == item.itemName)
            {
                // Increase the quantity and show that there's more than one
                dupe.quantity += item.defaultQtd;
                itemAdded = true;
                break;
            }
        }

        // If the item is still unique (no duplicates), add a new one
        if (!itemAdded)
        {
            Items.Add(item);
        }

        // Refresh the UI to show the updated inventory
        ListItems();
    }

    public void Remove(ItemData item)
    {
        Items.Remove(item);
        // Refresh the UI to show the updated inventory
        ListItems();
    }

    public void ListItems()
    {
        // Clear the objects at the start of the inventory
        foreach (Transform item in ItemContent)
        {
             item.gameObject.SetActive(false); //exisita um erro onde apaga a instância e assim era reconhecido como nulo ou não existente.
             //Portanto, em vez de Destroy(), fiz SetActive(false)
        }

        foreach (var item in Items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<TMP_Text>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            var itemQtd = obj.transform.Find("ItemQuantity").GetComponent<TMP_Text>();
            var RemoveButton = obj.transform.Find("RemoveButton").GetComponent<Button>();

            auxQTD = item.quantity;
            auxName = item.itemName;

            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
            itemQtd.text = auxQTD.ToString();
            if (EnableRemove.isOn)
                RemoveButton.gameObject.SetActive(true);
            SetInventoryItems();
        }
    }

    public void EnableItemsRemove()
    {
        if (EnableRemove.isOn)
        {
            foreach (Transform item in ItemContent)
            {
                item.Find("RemoveButton").gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (Transform item in ItemContent)
            {
                item.Find("RemoveButton").gameObject.SetActive(false);
            }
        }
    }

    public void SetInventoryItems()
    {
        InventoryItems = ItemContent.GetComponentsInChildren<InventoryItemController>();

        for (int i = 0; i < Items.Count; i++)
        {
            InventoryItems[i].AddItem(Items[i]);
        }
    }
}
