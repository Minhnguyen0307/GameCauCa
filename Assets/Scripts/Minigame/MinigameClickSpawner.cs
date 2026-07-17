using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MinigameClickSpawner : MonoBehaviour
{
    [Tooltip("Kéo thả prefab bomb_0 vào đây")]
    public GameObject bombPrefab;

    [Tooltip("Khoảng thời gian tối thiểu giữa các lần thả bom (giây)")]
    public float spawnCooldown = 0.75f;

    private Camera cam;
    private float lastSpawnTime = -999f;

    void Start()
    {
        cam = Camera.main;
        LochNessMonsterMove.ResetMinigameHP();
        KrakenMove.ResetMinigameHP();
        HideScoreDisplay();
    }

    private void HideScoreDisplay()
    {
        if (ScoreManager.Instance != null && ScoreManager.Instance.scoreText != null)
        {
            ScoreManager.Instance.scoreText.gameObject.SetActive(false);
        }

        TMPro.TMP_Text[] tmpTexts = FindObjectsOfType<TMPro.TMP_Text>(true);
        foreach (var txt in tmpTexts)
        {
            if (txt.name.ToLower().Contains("score") && !txt.transform.IsChildOf(transform))
            {
                Transform parent = txt.transform.parent;
                if (parent != null && parent.name.ToLower().Contains("score"))
                {
                    parent.gameObject.SetActive(false);
                }
                else
                {
                    txt.gameObject.SetActive(false);
                }
            }
        }

        UnityEngine.UI.Text[] standardTexts = FindObjectsOfType<UnityEngine.UI.Text>(true);
        foreach (var txt in standardTexts)
        {
            if (txt.name.ToLower().Contains("score") && !txt.transform.IsChildOf(transform))
            {
                Transform parent = txt.transform.parent;
                if (parent != null && parent.name.ToLower().Contains("score"))
                {
                    parent.gameObject.SetActive(false);
                }
                else
                {
                    txt.gameObject.SetActive(false);
                }
            }
        }
    }

    private bool hasWon = false;

    void Update()
    {
        if (hasWon)
            return;

        if (LochNessMonsterMove.IsDead && KrakenMove.IsDead)
        {
            hasWon = true;
            TriggerWin();
            return;
        }

        // Kiểm tra xem game có đang bị dừng hoặc chuột click vào UI không
        if (Time.timeScale == 0f)
            return;

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (Mouse.current == null) return;

        // Phát hiện click chuột trái và kiểm tra thời gian hồi chiêu
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (Time.time - lastSpawnTime >= spawnCooldown)
            {
                SpawnBombAtMouseX();
                lastSpawnTime = Time.time;
            }
        }
    }

    private void TriggerWin()
    {
        Debug.Log("[MinigameClickSpawner] Both monsters defeated! YOU WIN!");
        
        // Stop timer
        GameTimer timer = FindObjectOfType<GameTimer>();
        if (timer != null)
        {
            timer.timerIsRunning = false;
        }

        GameOverManager gameOverManager = FindObjectOfType<GameOverManager>();
        
        Time.timeScale = 0f;

        GameObject originalPanel = null;
        if (timer != null && timer.timeUpPanel != null)
        {
            originalPanel = timer.timeUpPanel;
        }
        else if (gameOverManager != null && gameOverManager.gameOverPanel != null)
        {
            originalPanel = gameOverManager.gameOverPanel;
        }

        if (originalPanel != null)
        {
            GameObject winPanel = Instantiate(originalPanel, originalPanel.transform.parent);
            winPanel.SetActive(true);

            // Find all TextMeshPro and standard Text elements inside winPanel and modify them
            TMPro.TMP_Text[] texts = winPanel.GetComponentsInChildren<TMPro.TMP_Text>(true);
            foreach (var txt in texts)
            {
                // If it is the score display or contains score, hide it
                if (txt.name.ToLower().Contains("score") || (timer != null && txt == timer.finalScoreText))
                {
                    txt.gameObject.SetActive(false);
                }
                else if (txt.text.Contains("Over") || txt.text.Contains("Up") || txt.text.Contains("Time") || txt.text.Contains("Hết giờ"))
                {
                    txt.text = "YOU WIN!";
                }
            }

            UnityEngine.UI.Text[] standardTexts = winPanel.GetComponentsInChildren<UnityEngine.UI.Text>(true);
            foreach (var txt in standardTexts)
            {
                if (txt.name.ToLower().Contains("score"))
                {
                    txt.gameObject.SetActive(false);
                }
                else if (txt.text.Contains("Over") || txt.text.Contains("Up") || txt.text.Contains("Time") || txt.text.Contains("Hết giờ"))
                {
                    txt.text = "YOU WIN!";
                }
            }
        }
        else
        {
            Debug.LogError("[MinigameClickSpawner] Could not find any GameOver or TimeUp panel in the scene to display YOU WIN!");
        }
    }

    void SpawnBombAtMouseX()
    {
        if (cam == null) cam = Camera.main;
        if (cam == null || bombPrefab == null) return;

        // Chuyển đổi vị trí chuột sang tọa độ world space
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        float spawnX = mouseWorldPos.x;

        // Tính vị trí mép trên của camera
        float cameraTopY = cam.transform.position.y + cam.orthographicSize;

        Vector3 spawnPos = new Vector3(spawnX, cameraTopY, 0f);

        // Tạo bản sao bom
        GameObject bombObj = Instantiate(bombPrefab, spawnPos, Quaternion.identity);

        // Lưu hiệu ứng nổ từ script cũ để gán cho script mới
        GameObject explosionEffect = null;
        Bomb oldBomb = bombObj.GetComponent<Bomb>();
        if (oldBomb != null)
        {
            explosionEffect = oldBomb.explosionEffectPrefab;
            DestroyImmediate(oldBomb);
        }

        // Loại bỏ script di chuyển cũ
        BombFall oldFall = bombObj.GetComponent<BombFall>();
        if (oldFall != null)
        {
            DestroyImmediate(oldFall);
        }

        // Gắn script bom Minigame mới
        BomMinigame newBombObj = bombObj.AddComponent<BomMinigame>();
        newBombObj.explosionEffectPrefab = explosionEffect;

        Debug.Log($"[MinigameClickSpawner] Đã tạo bom Minigame tại tọa độ X: {spawnX}, Y: {cameraTopY}");
    }
}
