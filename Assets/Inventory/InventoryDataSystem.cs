using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;

[Serializable]
public class InventoryDataSystem : MonoBehaviour
{
    public bool ItemNearby = false;
    public ItemsBases ItemInRange;
    public ItemDataInObject ItemToDestroy;
    public List<InventorySlot> Inventory => InventorySingleton.Instance.SingletonInventory;
    public SpriteRenderer ItemIcon;
    private void Start()
    {
        ItemIcon = GetComponentInChildren<SpriteRenderer>();
        ItemIcon.color = new Color(1, 1, 1, 0f);
    }
    private void Update()
    {
        if (ItemNearby && Input.GetKeyUp(KeyCode.E))
            AddItemToInventory(ItemInRange, 1);
    }

    private int FindClearSlotBeforeItemAddition(ItemsBases _item)
    {
        int count = 0;
        foreach (var slot in Inventory)
        {
            if (slot.itemData == _item && slot.amountInSlot < _item.maxAmount)
            {
                return count;
            }
            else
            {
                count++;
                continue;
            }
        }
        Debug.Log("No iteration.");
        return -1;
    }
    public void AddItemToInventory(ItemsBases _item, int amount)
    {
        int count = FindClearSlotBeforeItemAddition(_item);
        if (count == -1)
        {
            foreach (var slot in Inventory)
            {
                if (slot.itemData == null)
                {
                    slot.amountInSlot = 1;
                    slot.itemData = ItemInRange;
                    InventorySingleton.Instance.subscribeEventForInventoryUI(_item, amount);
                    InventorySingleton.Instance.subscribeEventForQuestUI(slot.amountInSlot, _item);
                    Destroy(ItemToDestroy.gameObject);
                    return;
                }
                Debug.Log("No new item");
            }
        }
        else
        {
            var slot = Inventory[count];            
            slot.amountInSlot += amount;
            InventorySingleton.Instance.subscribeEventForInventoryUI(_item, slot.amountInSlot);
            InventorySingleton.Instance.subscribeEventForQuestUI(slot.amountInSlot, _item);
            Destroy(ItemToDestroy.gameObject);
        }
    }

    public void RemoveItemInInventory(ItemsBases item, int amount)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Item"))
        {
            ItemNearby = true;
            ItemIcon.color = new Color(1, 1, 1, 1f);
            if (other.TryGetComponent(out ItemDataInObject One))
            {
                ItemInRange = One.data;
                ItemToDestroy = One;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            ItemNearby = false;
            ItemIcon.color = new Color(1, 1, 1, 0f);
            ItemInRange = null;
            ItemToDestroy = null;
        }
    }
}

