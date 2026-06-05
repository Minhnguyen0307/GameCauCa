using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;

    [Header("UI")]
    public GameObject pausePanel;

    private bool isPaused = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // ===================== PAUSE =====================
    public void TogglePause()
    {
        if (isPaused)
            Resume();
        else
            Pause();
    }

    public void Pause()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;

        // SFX (nếu có)
        // AudioController.Instance.PlaySFX(SoundType.Click);
    }

    public void Resume()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;

        // AudioController.Instance.PlaySFX(SoundType.Click);
    }

    // ===================== BUTTON =====================
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}