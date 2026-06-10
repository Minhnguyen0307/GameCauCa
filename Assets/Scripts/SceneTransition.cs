using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton xử lý hiệu ứng fade khi chuyển scene.
/// Tự tạo Canvas và Image overlay, không phụ thuộc vào scene.
/// </summary>
public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance;

    [Header("Fade Settings")]
    public float fadeDuration = 1f;

    private Image fadeImage;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CreateFadeOverlay();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeIn());
    }

    // ─────────────────────────────────────────────
    // Setup
    // ─────────────────────────────────────────────

    private void CreateFadeOverlay()
    {
        GameObject canvasGO = new GameObject("FadeCanvas");
        DontDestroyOnLoad(canvasGO);

        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        GameObject imageGO = new GameObject("FadeImage");
        imageGO.transform.SetParent(canvasGO.transform, false);

        fadeImage = imageGO.AddComponent<Image>();
        fadeImage.color = new Color(0f, 0f, 0f, 1f);
        fadeImage.raycastTarget = false;

        RectTransform rect = imageGO.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    // ─────────────────────────────────────────────
    // Public API
    // ─────────────────────────────────────────────

    public void LoadScene(string sceneName)
    {
        StartCoroutine(TransitionRoutine(sceneName));
    }

    // ─────────────────────────────────────────────
    // Coroutines
    // ─────────────────────────────────────────────

    private IEnumerator TransitionRoutine(string sceneName)
    {
        yield return StartCoroutine(FadeOut());
        SceneManager.LoadScene(sceneName);
        // FadeIn tự động qua OnSceneLoaded
    }

    private IEnumerator FadeOut()
    {
        yield return StartCoroutine(FadeToAlpha(1f, fadeDuration));
    }

    private IEnumerator FadeIn()
    {
        yield return StartCoroutine(FadeToAlpha(0f, fadeDuration));
    }

    private IEnumerator FadeToAlpha(float targetAlpha, float duration)
    {
        float elapsed = 0f;
        float startAlpha = fadeImage.color.a;
        Color color = fadeImage.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        fadeImage.color = color;
    }
}
