using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;

public class QuestUI : MonoBehaviour
{
    private bool _UIOpened = false;
    [SerializeField] private GameObject UIQuestWindow;
    [SerializeField] private GameObject QuestSlot;
    [SerializeField] public QuestBase CurrentQuest;
    [SerializeField] public bool questComplete = false;
    [SerializeField] private Image QuestIcon;
    private void Start()
    {
        DialogueFlag.Instance.OnGainQuest += AddQuest;
        InventorySingleton.Instance.OnQuestAmountChanged += ChangeAmountInPanel;
        UIQuestWindow.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.X))
        {
            if (_UIOpened)
            {
                QuestIcon.gameObject.SetActive(true);
                _UIOpened = !_UIOpened;
                UIQuestWindow.SetActive(false);
            }
            else
            {
                QuestIcon.gameObject.SetActive(false);
                _UIOpened = !_UIOpened;
                UIQuestWindow.SetActive(true);
            }
        }
    }

    private void AddQuest(QuestBase quest)
    {
        if (CurrentQuest == quest)
            return;
        GameObject localSlot = Instantiate(QuestSlot, UIQuestWindow.transform);
        localSlot.SetActive(true);
        TMP_Text QuestDescription = localSlot.transform.Find("QuestDescription").GetComponent<TMP_Text>();
        TMP_Text QuestName = localSlot.transform.Find("QuestName").GetComponent<TMP_Text>();
        TMP_Text QuestRequiredItem = localSlot.transform.Find("QuestRequiredItem").GetComponent<TMP_Text>();
        TMP_Text QuestAmount = localSlot.transform.Find("QuestAmount").GetComponent<TMP_Text>();

        CurrentQuest = quest;
        QuestAmount.text = $"Need {CurrentQuest.amount} items.";
        QuestDescription.text = quest.BeforeDescription;
        QuestName.text = quest.QuestName;
        QuestRequiredItem.text = $"Need item: {quest.RequieredItem.name}";
    }

    private void ChangeAmountInPanel(int amountInventory, ItemsBases item)
    {
        if (item.name != CurrentQuest?.RequieredItem.name)
            return;
        else if (item.name == CurrentQuest.RequieredItem.name)
        {
            if (amountInventory >= CurrentQuest.amount)
            {
                questComplete = true;
                foreach (Transform slot in UIQuestWindow.transform)
                {
                    TMP_Text QuestName = slot.transform.Find("QuestName").GetComponent<TMP_Text>();
                    if (CurrentQuest.QuestName != QuestName.text)
                        continue;
                    TMP_Text QuestAmount = slot.transform.Find("QuestAmount").GetComponent<TMP_Text>();
                    TMP_Text QuestDescription = slot.transform.Find("QuestDescription").GetComponent<TMP_Text>();
                    TMP_Text QuestRequiredItem = slot.transform.Find("QuestRequiredItem").GetComponent<TMP_Text>();
                    QuestAmount.text = "All items have been found";
                    QuestDescription.text = CurrentQuest.AfterDescription;
                    QuestRequiredItem.text = "";
                    return;
                }
            }
            foreach (Transform slot in UIQuestWindow.transform)
            {
                TMP_Text QuestAmount = slot.transform.Find("QuestAmount").GetComponent<TMP_Text>();
                QuestAmount.text = $"Obtain {Convert.ToString(amountInventory)}/{CurrentQuest.amount} items";
            }
        }
    }

    public void RemoveItemsAfterQuest()
    {
        foreach (InventorySlot slot in InventorySingleton.Instance.SingletonInventory)
        {
            if (slot.itemData.ItemName == CurrentQuest.RequieredItem.ItemName && slot.amountInSlot >= CurrentQuest.amount)
            {
                ItemsBases localItem = slot.itemData;
                slot.amountInSlot -= CurrentQuest.amount;
                if (slot.amountInSlot < 1)
                    slot.itemData = null;
                InventorySingleton.Instance.subscribeEventForInventoryUIWithQuestUI(localItem, slot.amountInSlot);
                questComplete = false;

                RemoveQuest();
                CurrentQuest = null;
                break;
            }
        }
    }

    public void RemoveQuest()
    {
        foreach(Transform slot in UIQuestWindow.transform)
        {
            TMP_Text Questname = slot.transform.Find("QuestName").GetComponent<TMP_Text>();
            if (Questname.text == CurrentQuest.QuestName)
                Destroy(slot.gameObject);
        }
    }

    private void OnDisable()
    {
        DialogueFlag.Instance.OnGainQuest -= AddQuest;
        InventorySingleton.Instance.OnQuestAmountChanged -= ChangeAmountInPanel;
    }
}
