using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Gắn vào GameObject Kraken (cùng với Collider2D).
/// Khi người chơi click vào Kraken → kích hoạt Game Over ngay lập tức.
/// Dùng New Input System (khớp với ClickCatch.cs).
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class KrakenClickHandler : MonoBehaviour
{
    void Start()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Minigame")
        {
            enabled = false;
        }
    }

    void Update()
    {
        // Chỉ xử lý khi game chưa over và có click trái
        if (GameOverManager.Instance != null && GameOverManager.Instance.IsGameOver)
            return;

        if (!Mouse.current.leftButton.wasPressedThisFrame)
            return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider == null) return;

        // Kiểm tra click đúng vào Kraken này
        if (hit.collider.gameObject == gameObject || hit.collider.transform.IsChildOf(transform))
        {
            if (GameOverManager.Instance != null)
                GameOverManager.Instance.TriggerGameOver();
            else
                Debug.LogWarning("[KrakenClickHandler] Không tìm thấy GameOverManager trong Scene!");
        }
    }
}
