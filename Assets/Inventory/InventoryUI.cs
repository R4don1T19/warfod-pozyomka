using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting;
using System.Runtime.InteropServices.WindowsRuntime;

public class InventoryUI : MonoBehaviour
{
    public bool inventoryOpened = false;
    [SerializeField] private GameObject InventoryChildren;
    [SerializeField] private GameObject InventoryBox;
    [SerializeField] private InventoryDataSystem IDS;
    [SerializeField] private Image imageIcon;
    [SerializeField] private TMP_Text amountText;
    [SerializeField] private Sprite blank;
    [SerializeField] private Image InventoryIcon;

    private void Start()
    {
        InventorySingleton.Instance.OnInventoryChanged += ChangeInventoryUI;
        InventorySingleton.Instance.OnInventoryChangedByQuest += ChangeInventoryUI;
        InventoryChildren.SetActive(false);
    }
    private void Update()
    {
        if (DialogueManager.Instance._isDialogueActive == true)
            return;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!inventoryOpened)
            {
                InventoryIcon.gameObject.SetActive(false);
                InventoryChildren.SetActive(true);
                inventoryOpened = !inventoryOpened;
            }
            else
            {
                InventoryIcon.gameObject.SetActive(true);
                InventoryChildren.SetActive(false);
                inventoryOpened = !inventoryOpened;
            }
        }
    }
    private void OnDisable()
    {
        InventorySingleton.Instance.OnInventoryChanged -= ChangeInventoryUI;
        InventorySingleton.Instance.OnInventoryChangedByQuest -= ChangeInventoryUI;
    }

    private void ChangeInventoryUI(ItemsBases item, int amount)
    {
        foreach(Transform slot in InventoryBox.transform)
        {
            amountText = slot.GetComponentInChildren<TMP_Text>();
            imageIcon = slot.GetComponent<Image>();

            if (imageIcon.sprite == item.image)
            {
                if (amount <= 0)
                {
                    amountText.text = "0";
                    imageIcon.sprite = blank;
                    Debug.Log("Clear item slot");
                }
                else
                {
                    amountText.text = Convert.ToString(amount);
                    Debug.Log("Added some items");
                }
                return;
            }
        }

        foreach(Transform slot in InventoryBox.transform)
        {
            amountText = slot.GetComponentInChildren<TMP_Text>();
            imageIcon = slot.GetComponent<Image>();

            if (imageIcon.sprite == blank || imageIcon.sprite == null)
            {
                amountText.text = amount.ToString();
                imageIcon.sprite = item.image;
                Debug.Log("Null to item");
                return;
            }
        }
        Debug.Log("Nothing happend");
    }

    /* Есть предмет 1. У него есть 1 изображение и 1 текст.
        Есть UI-предмет, который пустой. 
          Если UI-предмет не имеет изображения и у него 0 текст => он пустой, значит сюда можно поместить предмет.
        Если UI-предмет имеет какое-то изображение, значит и текст у него не пустой, но можно сложить => +текст
        Если UI-предмет */
}
