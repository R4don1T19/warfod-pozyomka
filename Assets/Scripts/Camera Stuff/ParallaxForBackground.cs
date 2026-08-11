using UnityEngine;

public class ParallaxPlusEffect : MonoBehaviour
{
    [SerializeField] private float ParallaxX;
    [SerializeField] private float ParallaxY;
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
        float ParallaxEffectX = 1 - ParallaxX;
        float ParallaxEffectY = 1 - ParallaxY;
        float CameraDistX = CameraObject.transform.position.x - StartPositionCamera.x;
        float CameraDistY = CameraObject.transform.position.y - StartPositionCamera.y;

        float ObjectDistX = StartPositionObject.x + (CameraDistX * ParallaxEffectX);
        float ObjectDistY = StartPositionObject.y + (CameraDistY * ParallaxEffectY);

        transform.position = new Vector2(ObjectDistX, ObjectDistY);
    }
}
