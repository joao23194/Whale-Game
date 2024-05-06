using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<ItemData> Items = new List<ItemData>();

    public Transform ItemContent;
    public GameObject InventoryItem;
    public Toggle EnableRemove;

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
            EnableRemove.isOn = false;
        }

        foreach (var item in Items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<Text>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();

            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
        }
    }

    public void EnableItemsRemove(){
        if(EnableRemove.isOn){
            foreach(Transform item in ItemContent){
                item.Find("RemoveButton").gameObject.SetActive(true);
            }
        }else{
            foreach(Transform item in ItemContent){
                item.Find("RemoveButton").gameObject.SetActive(false);
            }
        }
    }
}
