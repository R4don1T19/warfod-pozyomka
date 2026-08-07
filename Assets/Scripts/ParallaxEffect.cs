using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public GameObject cam;
    public float Parallax;
    private float startPosX;
    void Start()
    {
        cam = GameObject.Find("MainCamera");
    }
    // wow no way
    void Update()
    {
        float distX = (cam.transform.position.x * (1 - Parallax));
        float distY = (cam.transform.position.y * (1 - Parallax));
        transform.position = new Vector3(startPosX + distX, startPosX + distY, transform.position.z);
    }
}
