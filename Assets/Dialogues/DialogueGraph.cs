using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Dialogue Graph")]
public class DialogueGraph : ScriptableObject
{
    public string dialogueName;
    public string startNodeId;
    public List<DialogueNode> nodes;
    public QuestBase quest;
    public string GoodEnding;
    public string BadEnding;

    private Dictionary<string, DialogueNode> nodeDictionary;
    public DialogueNode FindNodeByID(string targetID)
    {
        if (nodeDictionary == null)
        {
            nodeDictionary = new Dictionary<string, DialogueNode>();
            foreach (var node in nodes)
            {
                nodeDictionary[node.NodeID] = node;
            }
        }
        if (nodeDictionary.TryGetValue(targetID, out DialogueNode result))
            return result;
        else
            return null;
    }

    public DialogueNode GetFirstNode()
    { 
        return FindNodeByID(startNodeId); 
    }
}
