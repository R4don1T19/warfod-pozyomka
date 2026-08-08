using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class DialogueNode 
{
    public string NodeID;
    public List<DialogueLine> lines;
    public string nextNodeID;
    public List<DialogueChoice> choices;
}
