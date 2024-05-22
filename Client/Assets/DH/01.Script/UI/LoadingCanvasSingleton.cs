using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingCanvasSingleton : MonoBehaviour
{
    public static LoadingCanvasSingleton Singleton { get; private set; }
    public Loading Loading { get; private set; }
    [field: SerializeField] public bool IsDisableOnLoad { get; private set; } = true;

    private void Awake()
    {
        if (Singleton != null) Destroy(gameObject);
        Singleton = this;

        DontDestroyOnLoad(gameObject);

        Loading = FindObjectOfType<Loading>(true);

        SceneManager.sceneUnloaded += HandleSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneUnloaded -= HandleSceneLoaded;
    }

    public void SetStateSceneLoader(bool state)
    {
        Loading.gameObject.SetActive(state);
    }

    private void HandleSceneLoaded(Scene before)
    {
        if (IsDisableOnLoad)
            Loading.gameObject.SetActive(false);
    }
}
