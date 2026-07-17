using UnityEngine;

public class MainMenuController : MonoBehaviour
{
#if UNITY_EDITOR
    private void Awake()
    {
        AddSceneToBuildSettings("Assets/Scenes/Minigame.unity");
    }

    private void OnValidate()
    {
        AddSceneToBuildSettings("Assets/Scenes/Minigame.unity");
    }

    private static void AddSceneToBuildSettings(string scenePath)
    {
        var scenes = UnityEditor.EditorBuildSettings.scenes;
        bool alreadyAdded = false;
        foreach (var scene in scenes)
        {
            if (scene.path == scenePath)
            {
                alreadyAdded = true;
                break;
            }
        }

        if (!alreadyAdded)
        {
            var newScenes = new UnityEditor.EditorBuildSettingsScene[scenes.Length + 1];
            System.Array.Copy(scenes, newScenes, scenes.Length);
            newScenes[scenes.Length] = new UnityEditor.EditorBuildSettingsScene(scenePath, true);
            UnityEditor.EditorBuildSettings.scenes = newScenes;
            Debug.Log($"[AutoBuildSettings] Automatically added scene '{scenePath}' to Build Settings!");
        }
    }
#endif

    private void Start()
    {
        Time.timeScale = 1f;

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.score = 0;
        }

        if (AudioController.Instance != null)
        {
            AudioController.Instance.PlayMusic(SoundType.MainMenu);
        }

        ConfigureExistingMinigameButtons();

        // Dynamically attach SettingsController to setup the settings UI (gear button and panel)
        gameObject.AddComponent<SettingsController>();
    }

    private void ConfigureExistingMinigameButtons()
    {
        UnityEngine.UI.Button[] buttons = FindObjectsOfType<UnityEngine.UI.Button>(true);
        foreach (var btn in buttons)
        {
            TMPro.TMP_Text tmpText = btn.GetComponentInChildren<TMPro.TMP_Text>();
            UnityEngine.UI.Text uiText = btn.GetComponentInChildren<UnityEngine.UI.Text>();
            string btnText = "";
            if (tmpText != null) btnText = tmpText.text;
            else if (uiText != null) btnText = uiText.text;

            if (btnText.ToLower().Contains("minigame") || btn.name.ToLower().Contains("minigame"))
            {
                btn.onClick = new UnityEngine.UI.Button.ButtonClickedEvent();
                btn.onClick.AddListener(PlayMinigameMode);
                Debug.Log($"[MainMenuController] Configured click listener for existing Minigame button: {btn.name}");
            }
        }
    }

    public void PlayMinigameMode()
    {
        SceneTransition.Instance.LoadScene("Minigame");
    }

    public void PlayVeryEasyMode()
    {
        SceneTransition.Instance.LoadScene("Level_CucDe");
    }

    public void PlayEasyMode()
    {
        SceneTransition.Instance.LoadScene("Level_De");
    }

    public void PlayNormalMode()
    {
        SceneTransition.Instance.LoadScene("Level_BinhThuong");
    }

    public void PlayHardMode()
    {
        SceneTransition.Instance.LoadScene("Level_Kho");
    }

    public void PlayVeryHardMode()
    {
        SceneTransition.Instance.LoadScene("Level_CucKho");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }
}