using UnityEngine.UI;
using UnityEngine;
using TMPro;
public class SmallDialogueWindowManager : MonoBehaviour 
{
    [SerializeField] private DialogueUI dUI;
    [SerializeField] private SmallDialogueWindowPlayerTriggerZone pTZ;
    [SerializeField] private GameObject DIalogueBox;
    [SerializeField] private TMP_Text TextLine;
    [SerializeField] private TMP_Text SpeakerName;
    [SerializeField] private Image SpeakerSprite;
    private void Update()
    {
        if (pTZ.StartInspect)
        {
            if (Input.GetKeyDown(KeyCode.E))
                StartInspect(pTZ.Graph);
        }
    }

    private void StartInspect(SmallDialogueWindowGraph graph)
    {
        Debug.Log(graph.NodesList[0].lines[0].line);
    }
}
