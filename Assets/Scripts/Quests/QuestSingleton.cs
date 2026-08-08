using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class QuestSingleton : MonoBehaviour
{
    [SerializeField] public bool questComplete;
    [SerializeField] public string questName;
    public Dictionary<string, bool> IsQuestComplete;

    public static QuestSingleton Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void MakeAMarkToAQuest(QuestBase localQuest)
    {
        questName = localQuest.QuestName;
        IsQuestComplete[questName] = true;
    }

    public bool CheckTheMarkOfAQuest()
    {
        return IsQuestComplete[questName];
    }
}
