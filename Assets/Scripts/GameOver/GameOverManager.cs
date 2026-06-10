using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Quản lý trạng thái Game Over.
/// Gắn vào một GameObject trong Scene cùng với GameOverPanel UI.
/// </summary>
public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set; }

    [Header("UI")]
    [Tooltip("Panel Game Over (ẩn khi bắt đầu, hiện khi game over)")]
    public GameObject gameOverPanel;

    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    /// <summary>
    /// Gọi khi người chơi click vào Kraken → game over ngay lập tức.
    /// </summary>
    public void TriggerGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        // Dừng thời gian
        Time.timeScale = 0f;

        // Hiển thị panel
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Debug.Log("[GameOverManager] GAME OVER - Người chơi click Kraken!");
    }

    // ── Nút Retry ──────────────────────────────────────────────────────────────
    public void OnRetryClicked()
    {
        Time.timeScale = 1f;
        isGameOver = false;

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.score = 0;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // ── Nút Main Menu (sẽ implement sau) ──────────────────────────────────────
    public void OnMainMenuClicked()
    {
        Time.timeScale = 1f;
        isGameOver = false;
        SceneManager.LoadScene("MainMenu");
    }

    public bool IsGameOver => isGameOver;
}
