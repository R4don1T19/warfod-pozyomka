using Unity.VisualScripting;
using UnityEngine;

public class DialogueNPC : MonoBehaviour
{
    [SerializeField] private DialogueGraph DialogueBeforeQuest;
    [SerializeField] private DialogueGraph IDLE;
    [SerializeField] private DialogueGraph DialogueAfterQuest;
    [SerializeField] private QuestUI qUI;
    [SerializeField] private bool PlayerIsNear = false;
    [SerializeField] public bool FinalTalk => DialogueFlag.Instance.HasFlag(DialogueAfterQuest.GoodEnding);

    private SpriteRenderer interactWindow;

    private void Start()
    {
        qUI = FindFirstObjectByType<QuestUI>();
        interactWindow = GetComponentInChildren<SpriteRenderer>();
        interactWindow.color = new Color(1, 1, 1, 0f);
    }
    private void Update()
    {
        if(PlayerIsNear && Input.GetKeyUp(KeyCode.E))
        {
            if (DialogueManager.Instance._isDialogueActive == true)
            {
                DialogueManager.Instance.Advance();
                return;
            }

            if (qUI.questComplete == true)
                DialogueManager.Instance.StartDialogue(DialogueAfterQuest);
            else if (FinalTalk == true)
                DialogueManager.Instance.StartDialogue(IDLE);
            else if (qUI.CurrentQuest == null)
                DialogueManager.Instance.StartDialogue(DialogueBeforeQuest);
            else
                DialogueManager.Instance.StartDialogue(IDLE);
        }    
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerIsNear = true;
            interactWindow.color = new Color(1, 1, 1, 1f);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerIsNear = false;
            interactWindow.color = new Color(1, 1, 1, 0f);
        }
    }
}
