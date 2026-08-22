using UnityEngine;
using System.Collections.Generic;
using System;
[CreateAssetMenu(fileName = "SmallDialogueWindow", menuName = "Create new SmallDialogueWindow")]
[System.Serializable]public class SmallDialogueWindowGraph : ScriptableObject
{
    public List<SmallDialogueWindowNode> NodesList;
}
