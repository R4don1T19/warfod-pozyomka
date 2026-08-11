using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private GameObject Camera;
    [SerializeField] private float Parallax;
    private Vector2 StartPosition;
    private bool _isActive = false;
    private void Start()
    {
        StartPosition = gameObject.transform.position;
    }
    private void LateUpdate()
    {
        if (!_isActive)
            return;
        else
        {
            Vector2 CameraPosition = Camera.transform.position;
            float ParallaxFactor = 1 - Parallax;

            float PosX = StartPosition.x + (CameraPosition.x - StartPosition.x) * ParallaxFactor;
            float PosY = StartPosition.y + (CameraPosition.y - StartPosition.y) * ParallaxFactor;

            gameObject.transform.position = new Vector2(PosX, PosY);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MainCamera"))
            _isActive = true;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MainCamera"))
            _isActive = false;
    }
}