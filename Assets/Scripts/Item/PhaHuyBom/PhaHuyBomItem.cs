using UnityEngine;
using UnityEngine.InputSystem;

public class PhaHuyBomItem : MonoBehaviour
{
    public float speed = 2f;
    public float destroyOffset = 2f;

    private bool used = false;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        // Di chuyển item đi lên (sử dụng unscaled time để không bị ảnh hưởng bởi đóng băng/tạm dừng)
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

        // Phát hiện click chuột bằng Input System mới (tương tự MuItem.cs)
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (cam == null) cam = Camera.main;
            if (cam == null) return;

            Vector2 mousePos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                TriggerDefusal();
            }
        }
    }

    private void TriggerDefusal()
    {
        used = true;

        Debug.Log("[PhaHuyBomItem] Activated! Defusing all bombs on screen.");

        // Tìm và kích nổ tất cả bom thường một cách an toàn
        Bomb[] normalBombs = FindObjectsByType<Bomb>(FindObjectsSortMode.None);
        foreach (Bomb bomb in normalBombs)
        {
            if (bomb != null)
            {
                bomb.Explode(noDamage: true);
            }
        }

        // Tìm và kích nổ tất cả bom của Loch Ness Monster một cách an toàn
        LochNessBomb[] lochNessBombs = FindObjectsByType<LochNessBomb>(FindObjectsSortMode.None);
        foreach (LochNessBomb bomb in lochNessBombs)
        {
            if (bomb != null)
            {
                bomb.Explode(noDamage: true);
            }
        }

        // Tiêu diệt chính item này
        Destroy(gameObject);
    }
}
