using UnityEngine;
using UnityEngine.SceneManagement;
public class LocatesList : MonoBehaviour
{
    [SerializeField] private bool playerisnear = false;
    [SerializeField] private LocatesEnter LE;
    [SerializeField] private PlayerMovement PM;
    [SerializeField] private DONTDESTROYONLOAD DDOL;
    private Camera camera;
    private SpriteRenderer Interact;
    public bool PlayerIsNear { get { return playerisnear; } }
    private void Awake()
    {
        DDOL = DONTDESTROYONLOAD.Instance.GetComponent<DONTDESTROYONLOAD>();
        DontDestroyOnLoad(DDOL);
    }
    private void Start()
    {
        camera = Camera.main;
    }
    private void Update()
    {
        ShowIcon();
    }
    internal void Transit(string FromLocation, string ToLocation)
    {
        if (DONTDESTROYONLOAD.Instance.TransitDataCamera.ContainsKey(FromLocation) && DONTDESTROYONLOAD.Instance.TransitDataPlayer.ContainsKey(FromLocation))
        {
            DONTDESTROYONLOAD.Instance.TransitDataCamera[FromLocation] = camera.transform.position;
            DONTDESTROYONLOAD.Instance.TransitDataPlayer[FromLocation] = PM.transform.position;
        }
        else
        {
            DONTDESTROYONLOAD.Instance.TransitDataPlayer.Add(FromLocation, PM.transform.position);
            DONTDESTROYONLOAD.Instance.TransitDataCamera.Add(FromLocation, camera.transform.position);
        }
        SceneManager.LoadScene(ToLocation);
    }
    private void ShowIcon()
    {
        if (Interact != null)
        {
            if (playerisnear)
                Interact.color = new Color(1, 1, 1, 1f);
            else if (!playerisnear)
                Interact.color = new Color(1, 1, 1, 0f);
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
