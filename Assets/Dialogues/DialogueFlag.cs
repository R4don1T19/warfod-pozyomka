using UnityEngine;
using System.Collections.Generic;
using System;
public class DialogueFlag : MonoBehaviour
{
    public static DialogueFlag Instance {  get; private set; }
    private HashSet<string> _flags = new HashSet<string>();
    [SerializeField] private DialogueGraph _graph;
    public event Action<QuestBase> OnGainQuest;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetFlag(string flag)
    {
        if (string.IsNullOrEmpty(flag)) return;
        _flags.Add(flag);
        Debug.Log($"flag added: {flag}");

        DialogueFlag.Instance.OnGainQuest?.Invoke(_graph.quest);
    }

    public bool HasFlag(string flag)
    {
        return _flags.Contains(flag);
    }
    public void RemoveFlag(string flag)
    {
        _flags.Remove(flag);
    }
}
