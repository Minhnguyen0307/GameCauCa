using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Gắn vào GameObject Loch Ness Monster (lochnessmonster_0) (cùng với Collider2D).
/// Khi người chơi click vào Loch Ness Monster → kích hoạt Game Over ngay lập tức.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class LochNessMonsterClickHandler : MonoBehaviour
{
    void Update()
    {
        // Chỉ xử lý khi game chưa over và có click trái
        if (GameOverManager.Instance != null && GameOverManager.Instance.IsGameOver)
            return;

        if (Mouse.current == null) return;
        if (!Mouse.current.leftButton.wasPressedThisFrame)
            return;

        Camera cam = Camera.main;
        if (cam == null) return;

        Vector2 mousePos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider == null) return;

        // Kiểm tra click đúng vào Loch Ness Monster này
        if (hit.collider.gameObject == gameObject || hit.collider.transform.IsChildOf(transform))
        {
            if (GameOverManager.Instance != null)
            {
                Debug.Log("[LochNessMonsterClickHandler] Người chơi đã click nhầm vào Loch Ness Monster! Triggering Game Over.");
                GameOverManager.Instance.TriggerGameOver();
            }
            else
            {
                Debug.LogWarning("[LochNessMonsterClickHandler] Không tìm thấy GameOverManager trong Scene!");
            }
        }
    }
}
