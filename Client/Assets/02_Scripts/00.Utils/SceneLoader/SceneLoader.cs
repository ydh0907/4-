using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

[SingletonLifeTime(LifeTime.Application)]
public class SceneLoader : MonoSingleton<SceneLoader> {
    [SerializeField] private new Camera camera;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float transitionTime = 0.5f;

    protected override void Awake() {
        base.Awake();

        canvasGroup.alpha = 0;
        camera.gameObject.SetActive(Camera.main == null);
        DontDestroyOnLoad(gameObject);
    }

    private void Load() {
        SetRandomTip();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        if (string.IsNullOrEmpty(_targetScene)) return;
        LoadSceneAsync().Forget();
    }

    private void OnSceneUnloaded(Scene scene) {
        camera.gameObject.SetActive(Camera.main == null);
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void SetRandomTip() {
        // tipText.text = gameTip.GameTips[Random.Range(0, gameTip.GameTips.Count)];
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        camera.gameObject.SetActive(Camera.main == null);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private async UniTask ShowLoadingScreenAsync() {
        await canvasGroup.DOFade(1, transitionTime);
    }

    private async UniTask HideLoadingScreenAsync() {
        await canvasGroup.DOFade(0, transitionTime);
    }

    private async UniTask LoadSceneAsync() {
        await ShowLoadingScreenAsync();

        //await SceneManager.UnloadSceneAsync(_currentScene);

#if !UNITY_EDITOR
        await UniTask.WaitForSeconds(3f, true);
#endif

        if (_targetScene != null) {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(_targetScene);
            asyncOperation.completed += OnSceneLoaded;

            float timer = 0.0f;

            while (!asyncOperation.isDone) {
                await UniTask.Yield();

                timer += Time.deltaTime;

                if (asyncOperation.progress < 0.9f) continue;

                if (timer >= transitionTime)
                    asyncOperation.allowSceneActivation = true;
            }
        }

        await HideLoadingScreenAsync();

        _targetScene = null;
    }

    private void OnSceneLoaded(AsyncOperation operation) {
        IsDone = true;
    }

    #region static

    public bool IsLoading => _targetScene != null;

    public bool IsDone { get; private set; }

    private string _targetScene;

    public void LoadScene(string scene) {
        _targetScene = scene;
        Load();
    }

    #endregion
}
