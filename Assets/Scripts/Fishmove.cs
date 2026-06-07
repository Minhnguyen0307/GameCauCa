using UnityEngine;

public class FishMove : MonoBehaviour
{
    public float speed = 2f;

    // true = bơi sang phải, false = bơi sang trái
    public bool moveRight = false;

    void Start()
    {
        ApplyDirection();
    }

    void Update()
    {
        if (FishFreezeManager.Instance != null && FishFreezeManager.Instance.IsFrozen)
            return;

        float dir = moveRight ? 1f : -1f;
        transform.Translate(Vector2.right * dir * speed * Time.deltaTime);
    }

    void OnBecameInvisible()
    {
        moveRight = !moveRight;
        ApplyDirection();
    }

    void ApplyDirection()
    {
        Vector3 scale = transform.localScale;

        // ❗ sprite gốc quay từ PHẢI → TRÁI
        // nên:
        // - bơi trái  → giữ scale.x > 0
        // - bơi phải → lật scale.x < 0

        scale.x = Mathf.Abs(scale.x);

        if (moveRight)
            scale.x *= -1;

        transform.localScale = scale;
    }
}