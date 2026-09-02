using UnityEngine;
using UnityEngine.SceneManagement;

public class LocatesPlayerTransit : MonoBehaviour
{
    //Здесь должен быть как минимум скрипт с транзитом
    [SerializeField] private LocatesSpawner Spawner;
    [SerializeField] private ParallaxPlusEffect Parallax;
    private static string ToLocation;
    // Подписка нужна здесб из-за того, что объект, на котором висит этот скрипт(игрок) синглтон, и вместо единоразового
    // вызова я использую подписки, так как Start() или Awake() вызовется только один раз.
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindSpawner();

        if (Spawner != null)
            PlayerMovement.Instance.transform.position = Spawner.transform.position;

        if (Camera.main.name == "MainCamera")
            Camera.main.transform.position = new Vector2(PlayerMovement.Instance.transform.position.x, PlayerMovement.Instance.transform.position.y + 2);
    }
    private void FindSpawner()
    {
        LocatesSpawner[] SpawnersList = FindObjectsByType<LocatesSpawner>();
        foreach (LocatesSpawner spawner in SpawnersList)
        {
            string FromlocationLocale = spawner.GetComponentInChildren<DoorID>().DoorIDFrom;
            Debug.Log(FromlocationLocale);
            if (ToLocation == FromlocationLocale)
            {
                Spawner = spawner;
                break;
            }
        }
    }
    public void Transit(string SceneName, string ToLocationLocal)
    {
        Debug.Log($"{SceneName} + {ToLocationLocal}");
        ToLocation = ToLocationLocal;
        ParallaxCamera();
        SceneManager.LoadScene(SceneName);
    }
    private void ParallaxCamera()
    {
        // сброс стартовой позиции у камеры для лучшего параллакса.
        Parallax = GameObject.FindWithTag("Parallax")?.GetComponentInChildren<ParallaxPlusEffect>();
        if (Parallax != null)
        {
            Parallax.ResetParameters();
            Parallax.StartPositionCamera = Camera.main.transform.position;
        }
    }
}
