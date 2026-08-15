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
        DDOL = GameObject.Find("DDOL").GetComponent<DONTDESTROYONLOAD>();
        DontDestroyOnLoad(DDOL);
        //if (DDOL.transition == true && SceneName == "Kontassalama")
        //{
        //    GameObject.Find("MainCamera").transform.position = DDOL.CameraPositionBeforeTransitionK;
        //    GameObject.Find("MainPlayer").transform.position = DDOL.TransitionToOutsideK;
        //}
        if (DDOL.transition)
        {
            switch (SceneName)
            {
                case "Kontassalama":
                    GameObject.Find("MainCamera").transform.position = DDOL.CameraPositionK;
                    GameObject.Find("MainPlayer").transform.position = DDOL.PlayerPositionK;
                    break;
                case "Kontassalama1":
                    GameObject.Find("MainCamera").transform.position = DDOL.CameraPositionK1;
                    GameObject.Find("MainPlayer").transform.position = DDOL.PlayerPositionK1;
                    break;
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
            switch (SceneName)
            {
                case "house1":
                    LL.outside();
                    break;
                case "house2":
                    LL.outside();
                    break;
                case "Kontassalama1":
                    if (DoorID == "Kontassalama2")
                        LL.outside();
                    if (DoorID == "HouseFirst" || DoorID == "HouseSecond") 
                        LL.woodenHouse();
                    break;
                case "Kontassalama2":
                    LL.outside();
                    break;
            }
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
