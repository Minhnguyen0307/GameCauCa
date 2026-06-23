using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class HieuUngDenManager : MonoBehaviour
{
    public static HieuUngDenManager Instance { get; private set; }

    private bool isCatchDisabled = false;
    public bool IsCatchDisabled => isCatchDisabled;

    private GameObject warningTextObject;
    private Coroutine activeCoroutine;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        // Automatically initialize on start, making it 100% plug-and-play
        GameObject go = new GameObject("HieuUngDenManager");
        go.AddComponent<HieuUngDenManager>();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset state on scene change
        isCatchDisabled = false;
        if (warningTextObject != null)
        {
            Destroy(warningTextObject);
        }
        activeCoroutine = null;

        // Automatically spawn the HieuUngDenSpawner in Level_Kho only if not already present in the scene
        if (scene.name == "Level_Kho")
        {
            if (FindAnyObjectByType<HieuUngDenSpawner>() == null)
            {
                GameObject spawner = new GameObject("HieuUngDenSpawner");
                spawner.AddComponent<HieuUngDenSpawner>();
                Debug.Log("[HieuUngDenManager] Level_Kho loaded, spawned HieuUngDenSpawner automatically.");
            }
            else
            {
                Debug.Log("[HieuUngDenManager] HieuUngDenSpawner already exists in the scene hierarchy. Skipping auto-spawn.");
            }
        }
    }

    public void TriggerDisableCatch(float duration)
    {
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
        }
        activeCoroutine = StartCoroutine(DisableCatchRoutine(duration));
    }

    private IEnumerator DisableCatchRoutine(float duration)
    {
        isCatchDisabled = true;
        ShowWarningText();

        // Use WaitForSecondsRealtime to ensure it runs even if game is paused/manipulated by timescale
        yield return new WaitForSecondsRealtime(duration);

        isCatchDisabled = false;
        HideWarningText();
        activeCoroutine = null;
    }

    private void ShowWarningText()
    {
        if (warningTextObject != null)
        {
            warningTextObject.SetActive(true);
            return;
        }

        Canvas canvas = FindAnyObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogWarning("[HieuUngDenManager] No Canvas found in the scene to display the warning text!");
            return;
        }

        warningTextObject = new GameObject("HieuUngDenWarningText");
        warningTextObject.transform.SetParent(canvas.transform, false);

        TextMeshProUGUI textMesh = warningTextObject.AddComponent<TextMeshProUGUI>();
        textMesh.text = "Không thể bắt cá";
        textMesh.fontSize = 52;
        textMesh.color = Color.red;
        textMesh.alignment = TextAlignmentOptions.Center;

        // Try to load TMPro's default SDF font asset if possible, or search for one in the scene to match styling
        var existingText = FindAnyObjectByType<TextMeshProUGUI>();
        if (existingText != null && existingText.gameObject != warningTextObject)
        {
            textMesh.font = existingText.font;
        }

        RectTransform rect = warningTextObject.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = new Vector2(0f, 120f); // Slightly above center
        rect.sizeDelta = new Vector2(600f, 120f);
    }

    private void HideWarningText()
    {
        if (warningTextObject != null)
        {
            warningTextObject.SetActive(false);
        }
    }
}
