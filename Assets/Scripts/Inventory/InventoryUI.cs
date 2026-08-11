using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting;
using System.Runtime.InteropServices.WindowsRuntime;

public class InventoryUI : MonoBehaviour
{
    private bool _PanelUI = false;
    private bool _IconUI = true;
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
            ShowIconOrPanelUI();
        }
    }
    private void ShowIconOrPanelUI()
    {
        if (_IconUI)
        {
            InventoryIcon.gameObject.SetActive(false);
            _IconUI = false;
            InventoryChildren.SetActive(true);
            _PanelUI = true;
        }
        else
        {
            InventoryIcon.gameObject.SetActive(true);
            _IconUI = true;
            InventoryChildren.SetActive(false);
            _PanelUI = false;
        }
    }
    public void CloseAllUIElements()
    {
        if (_IconUI)
        {
            _IconUI = false;
            InventoryIcon.gameObject.SetActive(_IconUI);
        }
        else if (_PanelUI)
        {
            _PanelUI = false;
            InventoryChildren.SetActive(_PanelUI);
        }
    }
    public void ShowAllUIElements()
    {
        _IconUI = true;
        InventoryIcon.gameObject.SetActive(_IconUI);
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
