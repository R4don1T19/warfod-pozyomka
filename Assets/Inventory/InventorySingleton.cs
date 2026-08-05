using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class InventorySingleton : MonoBehaviour
{
    public static InventorySingleton Instance { get; private set; }
    [SerializeField] public List<InventorySlot> SingletonInventory = new List<InventorySlot>();
    public event Action<ItemsBases, int> OnInventoryChanged;
    public event Action<int, ItemsBases> OnQuestAmountChanged;
    public event Action<ItemsBases, int> OnInventoryChangedByQuest;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void subscribeEventForInventoryUI(ItemsBases item, int amount)
    {
        InventorySingleton.Instance.OnInventoryChanged?.Invoke(item, amount);
    }
    public void subscribeEventForQuestUI(int amount, ItemsBases item)
    {
        InventorySingleton.Instance.OnQuestAmountChanged?.Invoke(amount, item);
    }

    public void subscribeEventForInventoryUIWithQuestUI(ItemsBases item, int amount)
    {
        InventorySingleton.Instance.OnInventoryChangedByQuest?.Invoke(item, amount);
    }
}
