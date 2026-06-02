using UnityEngine;
using UnityEngine.InputSystem;

public class StopFishMove : MonoBehaviour
{
    public float speed = 1.5f;
    public float freezeTime = 5f;

    [Header("Effect")]
    public GameObject hieuUngPrefab;

    private bool used = false;
    private Collider2D col;

    void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        // Item bay lên
        transform.position += Vector3.up * speed * Time.deltaTime;

        if (used) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos =
                Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            if (col == Physics2D.OverlapPoint(mousePos))
            {
                used = true;

                // ===== 1️⃣ TẠO HIỆU ỨNG Ở GIỮA MÀN HÌNH =====
                if (hieuUngPrefab != null)
                {
                    Vector3 centerPos = Camera.main.transform.position;
                    centerPos.z = 0f; // QUAN TRỌNG cho game 2D

                    Instantiate(
                        hieuUngPrefab,
                        centerPos,
                        Quaternion.identity
                    );
                }

                // ===== 2️⃣ DỪNG CÁ NGAY =====
                FishFreezeManager.Instance.FreezeAllFish(freezeTime);

                // ===== 3️⃣ XOÁ ITEM =====
                Destroy(gameObject);
            }
        }
    }
}