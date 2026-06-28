using UnityEngine;
using UnityEngine.InputSystem;

public class HieuUngNhanDoiItem : MonoBehaviour
{
    public float speed = 2f;
    public float destroyOffset = 2f;

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
        // Di chuyển item đi lên (sử dụng unscaled time để không bị ảnh hưởng bởi pause)
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

        // Phát hiện click chuột bằng Input System mới (tương tự MuItem.cs và PhaHuyBomItem.cs)
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

        // 1. Kích hoạt double spawn trong 10s và slow down trong 7s trên FishDoubleManager
        if (FishDoubleManager.Instance != null)
        {
            FishDoubleManager.Instance.ActivateDoubleSpawn(10f);
            FishDoubleManager.Instance.ActivateSlowDown(7f);
        }

        // 2. Nhân đôi những con cá hiện đang trong camera
        DoubleVisibleFish();

        // 3. Tiêu diệt chính item này (không có chữ hiển thị như yêu cầu)
        Destroy(gameObject);
    }

    private void DoubleVisibleFish()
    {
        if (cam == null) cam = Camera.main;
        if (cam == null) return;

        // Tìm tất cả cá di chuyển trên màn hình
        FishMove[] normalFish = FindObjectsByType<FishMove>(FindObjectsSortMode.None);
        SpecialFishMove[] specialFish = FindObjectsByType<SpecialFishMove>(FindObjectsSortMode.None);
        SharkMove[] sharks = FindObjectsByType<SharkMove>(FindObjectsSortMode.None);
        OrcaMove[] orcas = FindObjectsByType<OrcaMove>(FindObjectsSortMode.None);

        foreach (var fish in normalFish)
        {
            if (fish != null && IsInCameraView(fish.transform.position))
            {
                Vector3 spawnPos = fish.transform.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0f);
                Instantiate(fish.gameObject, spawnPos, fish.transform.rotation);
            }
        }
        foreach (var fish in specialFish)
        {
            if (fish != null && IsInCameraView(fish.transform.position))
            {
                Vector3 spawnPos = fish.transform.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0f);
                Instantiate(fish.gameObject, spawnPos, fish.transform.rotation);
            }
        }
        foreach (var fish in sharks)
        {
            if (fish != null && IsInCameraView(fish.transform.position))
            {
                Vector3 spawnPos = fish.transform.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0f);
                Instantiate(fish.gameObject, spawnPos, fish.transform.rotation);
            }
        }
        foreach (var fish in orcas)
        {
            if (fish != null && IsInCameraView(fish.transform.position))
            {
                Vector3 spawnPos = fish.transform.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0f);
                Instantiate(fish.gameObject, spawnPos, fish.transform.rotation);
            }
        }
    }

    private bool IsInCameraView(Vector3 position)
    {
        Vector3 viewPos = cam.WorldToViewportPoint(position);
        return viewPos.x >= 0f && viewPos.x <= 1f && viewPos.y >= 0f && viewPos.y <= 1f && viewPos.z > 0f;
    }
}
