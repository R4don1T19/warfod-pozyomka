using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
   
    private DialogueGraph _graph;
    private DialogueNode _node;
    private int _currentLineIndex;
    public bool _isDialogueActive = false;

    [SerializeField] private PlayerMovement PM;
    [SerializeField] private DialogueUI dUI;
    [SerializeField] private QuestUI qUI;
    [SerializeField] private InventoryDataSystem iDS;
    [SerializeField] private InventoryUI iUI;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void StartDialogue(DialogueGraph graph)
    {
        if (_isDialogueActive) return;

        dUI = FindAnyObjectByType<DialogueUI>(FindObjectsInactive.Include);
        PM = PlayerMovement.Instance.GetComponent<PlayerMovement>();

        qUI.CloseAllUIElements(); // Закрыть все квестовые UI-элементы, пока идет диалог
        iUI.CloseAllUIElements(); // То же самое, но уже с UI-элементами Инвентаря.

        _graph = graph;
        _isDialogueActive = true;
        PM.enabled = false;       // Игрок двигаться не может пока идет диалог

        ShowNode(_graph.GetFirstNode());
    }

    private void ShowNode(DialogueNode node)
    {
        if (node == null)
        {
            EndDialogue();
            return;
        }
        _node = node;
        _currentLineIndex = 0;
        ShowCurrentLine();
    }

    private void ShowCurrentLine()
    {
        DialogueLine line = _node.lines[_currentLineIndex];
        dUI.ShowLine(line);
    }

    public void Advance()
    {
        if (!_isDialogueActive)
            return;
        _currentLineIndex++;

        if (_currentLineIndex < _node.lines.Count)
        {
            ShowCurrentLine();
            return;
        }

        if (_node.choices.Count > 0)
        {
            dUI.ShowChoices(_node.choices);
            return;
        }

        if (!string.IsNullOrEmpty(_node.nextNodeID))
        {
            ShowNode(_graph.FindNodeByID(_node.nextNodeID));
            return;
        }
        EndDialogue();
    }

    public void SelectChoice(DialogueChoice choice)
    {
        if (!string.IsNullOrEmpty(choice.keyChoice))
        {
            DialogueFlag.Instance.SetFlag(choice.keyChoice);
            if (qUI.questComplete == false)
            {
                ShowNode(_graph.FindNodeByID(choice.nextNodeID));
                return;
            }
            else
                qUI.RemoveItemsAfterQuest();

        }
        ShowNode(_graph.FindNodeByID(choice.nextNodeID));
    }

    public void EndDialogue()
    {
        _isDialogueActive = false;
        _graph = null;
        _node = null;
        dUI.Hide();

        qUI.OpenAllUIElements(); // Можно снова увидеть квест-UI
        iUI.ShowAllUIElements(); // Можно снова увидеть инвентарь-UI
        
        PM.enabled = true; // Теперь игрок может двигаться
    }



}
