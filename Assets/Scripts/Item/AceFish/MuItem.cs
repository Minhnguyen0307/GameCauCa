using UnityEngine;
using UnityEngine.InputSystem;

public class MuItem : MonoBehaviour
{
    public float moveSpeed = 2f;
    public GameObject muEffectPrefab;

    void Update()
    {
        // Item bay lên (không bị ảnh hưởng pause)
        transform.position += Vector3.up * moveSpeed * Time.unscaledDeltaTime;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos =
                Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                OnClick(mousePos);
            }
        }
    }

    void OnClick(Vector2 clickPos)
    {
        // Hiệu ứng
        Instantiate(muEffectPrefab, clickPos, Quaternion.identity);

        // Quét cá
        SweepFish(clickPos);

        Destroy(gameObject);
    }

    void SweepFish(Vector2 center)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(center, 10f);

        foreach (var hit in hits)
        {
            FishCatchable fish = hit.GetComponent<FishCatchable>();
            if (fish != null)
            {
                fish.ForceCatch(); // bắt 100%
            }
        }
    }
}