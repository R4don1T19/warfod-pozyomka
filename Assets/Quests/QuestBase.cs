using UnityEngine;
using TMPro;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "NewQuest", menuName = "Quest/New Quest")]
public class QuestBase : ScriptableObject
{
    public string QuestName;
    public string BeforeDescription;
    public string AfterDescription;
    public ItemsBases RequieredItem;
    public int amount;
}
