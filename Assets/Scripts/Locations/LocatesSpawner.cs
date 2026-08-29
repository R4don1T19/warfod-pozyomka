using UnityEngine;
public class LocatesSpawner : MonoBehaviour
{
    // Здесь должно происходить 
    [SerializeField] private bool PlayerIsNearby = false;
    private LocatesPlayerTransit LPT;
    private DoorID DoorID;
    private SpriteRenderer sprite; 

    private void Start()
    {
        DoorID = GetComponentInChildren<DoorID>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        sprite.color = new Color(1, 1, 1, 0f);
    }
    private void Update()
    {
        if (PlayerIsNearby)
        {
            if (Input.GetKeyDown(KeyCode.E))
                LPT.Transit(DoorID.SceneName, DoorID.DoorIDTo);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DoorIdentificator"))
        {
            PlayerIsNearby = true;
            LPT = collision.GetComponentInChildren<LocatesPlayerTransit>();
            sprite.color = new Color(1, 1, 1, 1f);
        }
    }
    private void OnTriggerExit2D(Collider2D Collision)
    {
        if (Collision.CompareTag("DoorIdentificator"))
        {
            PlayerIsNearby = false;
            LPT = null;
            sprite.color = new Color(1, 1, 1, 0f);
        }
    }
}
