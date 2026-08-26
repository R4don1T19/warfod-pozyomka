using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LocatesEnter : MonoBehaviour
{ 
    [SerializeField] internal string SceneName;
    [SerializeField] internal string DoorID;
    private LocatesList LL;
    private DONTDESTROYONLOAD DDOL;
    private void Awake()
    {
        SceneName = SceneManager.GetActiveScene().name;
        DDOL = GameObject.FindWithTag("Singleton").GetComponent<DONTDESTROYONLOAD>();
        DontDestroyOnLoad(DDOL);

        if (DDOL.transition)
        {
            if (DONTDESTROYONLOAD.Instance.TransitDataPlayer.TryGetValue(SceneName, out Vector2 CameraValue) && DONTDESTROYONLOAD.Instance.TransitDataCamera.TryGetValue(SceneName, out Vector2 PlayerValue))
            {
                Camera.main.transform.position = CameraValue;
                GameObject.FindWithTag("Player").transform.position = PlayerValue;
            }
        }
    }
    private void Start()
    {
        LL = GetComponent<LocatesList>();
    }
    private void Update()
    {
        if (LL == null)
            return;
        else if (LL.PlayerIsNear && Input.GetKeyUp(KeyCode.E))
        {
            DDOL.transition = true;
            LL.Transit(SceneName, DoorID);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Door"))
        {
            LL = collision.GetComponent<LocatesList>();
            DoorID = collision.GetComponentInChildren<DoorID>().ID;
        }
    }
}
