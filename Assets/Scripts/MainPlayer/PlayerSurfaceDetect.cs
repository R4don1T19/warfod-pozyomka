using Unity.VisualScripting;
using UnityEngine;

public class PlayerSurfaceDetect : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    private Rigidbody2D rb;
    private Transform currentLadder;
    private Transform tpUp;
    private Transform tpDn;
    public Transform TPUP {  get { return tpUp; } set { tpUp = value; } }
    public Transform TPDN { get { return tpDn; } set { tpDn = value; } }
    
    [SerializeField] private bool _isGrounded;
    [SerializeField] private bool _isStairs;
    [SerializeField] private bool _NearLadder;
    [SerializeField] private bool _isOnLadder;

    public bool IsGrounded { get { return _isGrounded; } set { _isGrounded = value; } }
    public bool IsStairs { get  { return _isStairs; } set { _isStairs = value; } }
    public bool NearLadder { get { return _NearLadder; } set { _NearLadder = value; } }
    public bool IsOnLadder { get { return _isOnLadder; } set { _isOnLadder = value; } }
    void Start()
    {
        Player = GameObject.Find("MainPlayer");
        rb = GetComponentInParent<Rigidbody2D>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            _isGrounded = true;
            Player.layer = LayerMask.NameToLayer("Default");
        }

        if (collision.CompareTag("Ladder")) // если мы задели какую-то лестницу, то 
        {
            currentLadder = collision.transform; // обновляем инфу по объектам
            tpDn = currentLadder.Find("DownPosition");
            tpUp = currentLadder.Find("UpPosition");
        }

        if (collision.CompareTag("Stairs"))
        {
            _isStairs = true;
            rb.gravityScale = 0;
        }

        if (collision.CompareTag("LadderZone")) // в зоне лестниц, но не на ней
        {
            _NearLadder = true;
        }

        if (collision.CompareTag("bottomLadder") || collision.CompareTag("topLadder")) // если достигли либо вверха либо низа, то
        {
            _isOnLadder = false; // мы больше не на лестнице
            _NearLadder = true; // но еще в её зоне(важно)
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder")) // покинул лестницу?
        {
            _isOnLadder = false; // значит ты не на лестнице
            _isGrounded = true;
            rb.gravityScale = 10f; // вернем тебе земную гравитацию
            Player.layer = LayerMask.NameToLayer("Default"); // и заберем возможность сквозь стены проходить
        }

        if (collision.CompareTag("Ground"))
        {
            _isGrounded = false;
        }

        if (collision.CompareTag("Stairs"))
        {
            _isStairs = false;
            rb.gravityScale = 10f;
        }

        if (collision.CompareTag("LadderZone")) // ну все, лестниц больше не наблюдаю.
        {
            _NearLadder = false;
        }
    }
}