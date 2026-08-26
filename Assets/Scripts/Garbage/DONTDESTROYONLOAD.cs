using UnityEngine;
using System.Collections.Generic;

public class DONTDESTROYONLOAD : MonoBehaviour
{
    public static DONTDESTROYONLOAD Instance { get; set; }
    public Dictionary<string, Vector2>TransitDataPlayer = new Dictionary<string, Vector2>();
    public Dictionary<string, Vector2>TransitDataCamera = new Dictionary<string, Vector2>();
    [SerializeField] internal bool transition = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
}
