using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;

    [SerializeField] private TMP_Text speakerName;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Image icon;

    [SerializeField] private GameObject choicesPanel;
    [SerializeField] private GameObject choiceButtons;

    private List<GameObject> choicesList = new List<GameObject>();
    private Coroutine _typeCoroutine;

    private void Start()
    {
        dialoguePanel.SetActive(false);   
    }
    public void ShowLine(DialogueLine line)
    {
        choicesPanel.SetActive(false);
        dialoguePanel.SetActive(true);

        speakerName.text = line.name;
        icon.sprite = line.picture;

        if (_typeCoroutine != null)
            StopCoroutine(_typeCoroutine);

        _typeCoroutine = StartCoroutine(TypeText(line.message));
    }
    private IEnumerator TypeText(string text)
    {
        dialogueText.text = "";

        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(0.03f);
        }
    }
    public void ShowChoices(List<DialogueChoice> choices)
    {
        foreach (var buttons in choicesList)
            Destroy(buttons);
        choicesList.Clear();
        choicesPanel.SetActive(true);

        foreach (var choice in choices)
        {
            GameObject buttonObject = Instantiate(choiceButtons, choicesPanel.transform);
            TMP_Text buttonText = buttonObject.GetComponentInChildren<TMP_Text>();
            buttonText.text = choice.choiceText;

            DialogueChoice localChoice = choice;
            Button button = buttonObject.GetComponentInChildren<Button>();
            button.onClick.AddListener(() => DialogueManager.Instance.SelectChoice(localChoice));
            choicesList.Add(buttonObject);
        }
    }

    public void Hide()
    {
        dialoguePanel.SetActive(false);
        choicesPanel.SetActive(false);

        foreach (var button in choicesList)
            Destroy(button);
        choicesList.Clear();
    }
}
