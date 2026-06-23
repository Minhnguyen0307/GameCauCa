using UnityEngine;
using UnityEngine.InputSystem;

public class HieuUngDenItem : MonoBehaviour
{
    public float speed = 2f;
    public float penaltyDuration = 5f;
    public float destroyOffset = 2f;

    private bool used = false;
    private Collider2D col;
    private Camera cam;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        // Move item upwards
        transform.position += Vector3.up * speed * Time.deltaTime;

        // Auto destroy when moving off screen top
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

        // Detect click
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (cam == null) return;

            Vector2 mousePos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            if (col == Physics2D.OverlapPoint(mousePos))
            {
                used = true;

                // Trigger catch disabled state (5 seconds)
                if (HieuUngDenManager.Instance != null)
                {
                    HieuUngDenManager.Instance.TriggerDisableCatch(penaltyDuration);
                }

                // Destroy the item
                Destroy(gameObject);
            }
        }
    }
}
