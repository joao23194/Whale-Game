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
    }
    public void Add(ItemData item)
    {
        Items.Add(item);
    }
    public void Remove(ItemData item)
    {
        Items.Remove(item);
    }

    public void ListItems()
    {
        //Limpa o(s) objeto(s) ao iniciar o inventário
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
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
