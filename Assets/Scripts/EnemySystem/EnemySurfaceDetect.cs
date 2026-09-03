using UnityEngine;

public class EnemySurfaceDetect : MonoBehaviour
{
    public Rigidbody2D rb;
    private bool _isOnStairs = false;
    private bool _isGrounded = false;

    public bool IsGrounded { get { return _isGrounded; } }
    public bool IsStairs {  get { return _isOnStairs; } }

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Stairs"))
        {
            rb.gravityScale = 0f;
            _isOnStairs = true;
        }
            
        if (collision.CompareTag("Ground"))
            _isGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Stairs"))
        {
            rb.gravityScale = 10f;
            _isOnStairs = false;
        }
        if (collision.CompareTag("Ground"))
            _isGrounded = false;

    }
}
