using UnityEngine;
using UnityEngine.InputSystem;

public class ClockItem : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 2f;
    public float destroyOffset = 2f;

    [Header("Timer Settings")]
    public float timeToAdd = 15f;

    private bool used = false;
    private Camera cam;
    private Collider2D col;

    private void Start()
    {
        cam = Camera.main;
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        // Di chuyển item đi lên (unscaledDeltaTime để không bị ảnh hưởng bởi pause/stop)
        transform.position += Vector3.up * speed * Time.unscaledDeltaTime;

        // Tự động hủy khi bay vượt quá mép trên màn hình
        if (cam != null)
        {
            float cameraTop = cam.transform.position.y + cam.orthographicSize;
            if (transform.position.y > cameraTop + destroyOffset)
            {
                Destroy(gameObject);
                return;
            }
        }

        if (used) return;

        // Phát hiện click chuột bằng Input System mới (tương tự các item khác)
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (cam == null) cam = Camera.main;
            if (cam == null) return;

            Vector2 mousePos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            bool clickedMe = false;
            if (col != null)
            {
                clickedMe = (col == Physics2D.OverlapPoint(mousePos));
            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
                clickedMe = (hit.collider != null && hit.collider.gameObject == gameObject);
            }

            if (clickedMe)
            {
                ActivateEffect();
            }
        }
    }

    private void ActivateEffect()
    {
        used = true;

        // 1. Cộng 15 giây vào GameTimer
        GameTimer gameTimer = FindAnyObjectByType<GameTimer>();
        if (gameTimer != null)
        {
            gameTimer.timeRemaining += timeToAdd;
            Debug.Log($"[ClockItem] Added {timeToAdd} seconds to GameTimer. New time remaining: {gameTimer.timeRemaining}");
        }
        else
        {
            Debug.LogWarning("[ClockItem] GameTimer not found in the scene!");
        }

        // 2. Phát âm thanh Click
        if (AudioController.Instance != null)
        {
            AudioController.Instance.PlaySFX(SoundType.Click);
        }

        // 3. Tiêu diệt chính item
        Destroy(gameObject);
    }
}
