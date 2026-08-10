using UnityEngine;

public class ParallaxPlusEffect : MonoBehaviour
{
    [SerializeField] private float Parallax = 0.95f;
    [SerializeField] private GameObject CameraObject;

    private Vector2 StartPositionCamera;
    private Vector2 StartPositionObject;
    private bool isInitialized = false;

    // Вместо LateUpdate делаем подписку
    private void OnEnable()
    {
        Application.onBeforeRender += UpdateParallax;
        // Application.OnBeforeRender Вызывается тогда, когда нужно сделать что-то непосредственно перед рендером сцены(следующего кадра).
        // В моем случае, он помог мне нубрать проблему с несоответствием вычислениями LateUpdate и данными, которые не успели обновиться.
    }
    private void OnDisable()
    {
        Application.onBeforeRender -= UpdateParallax;
    }
    private void UpdateParallax()
    {
        if (!isInitialized)
        {
            StartPositionCamera = CameraObject.transform.position;
            StartPositionObject = transform.position;
            isInitialized = true;
            return;
        }

        float CameraDistX = CameraObject.transform.position.x - StartPositionCamera.x;
        float CameraDistY = CameraObject.transform.position.y - StartPositionCamera.y;

        float ObjectDistX = StartPositionObject.x + (StartPositionObject.x + CameraDistX) / Parallax - (StartPositionObject.x / Parallax);
        float ObjectDistY = StartPositionObject.y + (StartPositionObject.y + CameraDistY) / Parallax - (StartPositionObject.y / Parallax);

        transform.position = new Vector3(ObjectDistX, ObjectDistY, transform.position.z);
    }
}
