using UnityEngine;
using UnityEngine.InputSystem;

public class ClickCatch : MonoBehaviour
{
    void Update()
    {
        if (!Mouse.current.leftButton.wasPressedThisFrame)
            return;

        Vector2 mousePos =
            Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider == null) return;

        // Bấm vào Kraken → Game Over
        KrakenMove kraken = hit.collider.GetComponent<KrakenMove>();
        if (kraken != null)
        {
            if (GameOverManager.Instance != null)
                GameOverManager.Instance.TriggerGameOver();
            return;
        }

        // Bấm vào cá → bắt cá
        FishCatchable fish = hit.collider.GetComponent<FishCatchable>();
        if (fish != null)
        {
            fish.TryCatch();
        }
    }
}