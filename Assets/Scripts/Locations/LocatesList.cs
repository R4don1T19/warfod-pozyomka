using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LocatesList : MonoBehaviour
{
    [SerializeField] private bool playerisnear = false;
    private SpriteRenderer Interact;
    [SerializeField] private LocatesEnter LE;
    private PlayerMovement PM;
    private DONTDESTROYONLOAD DDOL;
    public bool PlayerIsNear { get { return playerisnear; } }
    private void Awake()
    {
        DDOL = GameObject.Find("DDOL").GetComponent<DONTDESTROYONLOAD>();
        DontDestroyOnLoad(DDOL);
    }
    private void Start()
    {
        PM = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        if (Interact != null)
        {
            if (playerisnear)
                Interact.color = new Color(1, 1, 1, 1f);
            else if (!playerisnear)
                Interact.color = new Color(1, 1, 1, 0f);
        }
    }
    internal void woodenHouse()
    {
        DDOL.PlayerPositionK = PM.transform.position;
        DDOL.CameraPositionK = GameObject.Find("MainCamera").transform.position;
        if (LE.DoorID == "HouseFirst")
            SceneManager.LoadScene("house1");
        else if (LE.DoorID == "HouseSecond")
            SceneManager.LoadScene("house2");
    }
    internal void outside()
    {
        if (LE.DoorID == "Kontassalama2")
        {
            if (LE.SceneName == "Kontassalama1")
            {
                DDOL.CameraPositionK1 = GameObject.Find("MainCamera").transform.position;
                DDOL.PlayerPositionK1 = PM.transform.position;
            }
            SceneManager.LoadScene("Kontassalama2");
        }
        else if (LE.DoorID == "Kontassalama1")
        {
            DDOL.PlayerPositionK = PM.transform.position;
            DDOL.CameraPositionK = GameObject.Find("MainCamera").transform.position;
            SceneManager.LoadScene("Kontassalama1");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DoorIdentificator"))
        {
            Interact = GetComponentInChildren<SpriteRenderer>();
            playerisnear = true;
            LE = collision.GetComponent<LocatesEnter>();
            PM = collision.GetComponentInParent<PlayerMovement>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("DoorIdentificator"))
            playerisnear = false;
    }
}
