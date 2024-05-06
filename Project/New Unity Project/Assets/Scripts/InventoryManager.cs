using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<ItemData> Items = new List<ItemData>();

    public Transform ItemContent;
    public GameObject InventoryItem;

    private void Awake()
    {
        Instance = this;
    }
    public void Add(ItemData item){
        Items.Add(item);
    }
    public void Remove(ItemData item){
        Items.Remove(item);
    }

    public void ListItems(){
        foreach (var item in Items){
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemName = obj.transform.Find("Item/ItemName").GetComponent<Text>();
            var itemIcon = obj.transform.Find("Item/ItemIcon").GetComponent<Image>();

            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
        }
    }
}
