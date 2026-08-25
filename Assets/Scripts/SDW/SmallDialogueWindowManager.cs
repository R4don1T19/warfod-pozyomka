using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
public class SmallDialogueWindowManager : MonoBehaviour
{
    [SerializeField] private DialogueUI dUI;
    [SerializeField] private SmallDialogueWindowPlayerTriggerZone pTZ;
    [SerializeField] private PlayerMovement PM;
    [SerializeField] private GameObject DIalogueBox;
    [SerializeField] private TMP_Text TextLine;
    [SerializeField] private TMP_Text SpeakerName;
    [SerializeField] private Image SpeakerSprite;
    private SmallDialogueWindowLine currentLine;
    private bool DialogueIsActive = false;
    private int LineCount = 0;
    private int CurrentNode = 0;
    private void Update()
    {
        if (pTZ.StartInspect)
        {
            if (Input.GetKeyDown(KeyCode.E) && !DialogueIsActive)
                StartInspect(pTZ.Graph);
            else if (Input.GetKeyDown(KeyCode.E) && DialogueIsActive)
                DialogueEnd();
        }
    }

    private void StartInspect(SmallDialogueWindowGraph graph)
    {
        PM.enabled = false;
        // Все нужны для UI-элементов значения берутся напрямую именно с самого последнего списка(реплика, спрайт и имя говорящего).
        currentLine = graph.NodesList[CurrentNode].lines[LineCount];

        DIalogueBox.SetActive(true);
        SpeakerName.text = currentLine.name;
        SpeakerSprite.sprite = currentLine.icon;
        TextLine.text = currentLine.line;
        LineCount++;

        if (LineCount >= graph.NodesList[CurrentNode].lines.Count)
        {
            LineCount = 0;
            CurrentNode++;
            if (CurrentNode < graph.NodesList.Count)
                return;
            else
            {
                CurrentNode = 0;
                LineCount = 0;
                DialogueIsActive = true;
                return;
            }
        }
    }
    private void DialogueEnd()
    {
        SpeakerName.text = null;
        SpeakerSprite.sprite = null;
        TextLine.text = null;
        DIalogueBox.SetActive(false);

        DialogueIsActive = false;
        PM.enabled = true;
    }
}
