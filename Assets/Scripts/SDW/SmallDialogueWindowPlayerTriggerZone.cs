using UnityEngine;

public class SmallDialogueWindowPlayerTriggerZone : MonoBehaviour
{
    public SmallDialogueWindowGraph Graph;
    public bool StartInspect = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Prop"))
        {
            StartInspect = true;
            if (collision.TryGetComponent<SmallDialogueWindowObjectID>(out SmallDialogueWindowObjectID graph))
                Graph = graph.SDWID;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Prop"))
        {
            Graph = null;
            StartInspect = false;
        }
    }
}
