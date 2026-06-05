using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Hiệu ứng xoáy nước của Kraken.
/// - Quay sprite xoáy nước với tốc độ 1 radian/giây (ngược chiều kim đồng hồ).
/// - Hút tất cả cá trong bán kính 7 đơn vị về phía tâm xoáy.
/// - Khi cá vào đủ gần (sát tâm), Kraken ăn thịt cá đó.
/// Gắn script này vào GameObject "xoaynuoc" con của Kraken.
/// </summary>
public class XoayNuocEffect : MonoBehaviour
{
    [Header("Vortex Settings")]
    [Tooltip("Bán kính hút cá (đơn vị world)")]
    public float suctionRadius = 7f;

    [Tooltip("Lực hút cá về tâm xoáy (đơn vị/giây)")]
    public float suctionForce = 4f;

    [Tooltip("Khoảng cách tới tâm để coi là đã bị ăn")]
    public float eatDistance = 0.5f;

    [Tooltip("Tốc độ quay sprite (radian/giây) — 1 rad/s theo yêu cầu")]
    public float rotationSpeed = 1f;   // radian/giây → đổi sang độ khi dùng

    // Tham chiếu tới KrakenMove để gọi EatFish
    private KrakenMove krakenMove;

    // Danh sách cá đang bị hút (tránh GetComponent mỗi frame)
    private readonly List<Transform> suckedFish = new List<Transform>();

    void Awake()
    {
        // Tìm KrakenMove trên GameObject cha
        krakenMove = GetComponentInParent<KrakenMove>();
        if (krakenMove == null)
            Debug.LogWarning("[XoayNuocEffect] Không tìm thấy KrakenMove trên parent!");

        // Tắt hiệu ứng khi mới tạo, chờ KrakenMove kích hoạt
        gameObject.SetActive(false);
    }

    void Update()
    {
        // ── Quay sprite xoáy nước ──────────────────────────────────────────
        // Chuyển radian/giây sang độ/giây: degrees = radians * Mathf.Rad2Deg
        float degreesPerSecond = rotationSpeed * Mathf.Rad2Deg;
        transform.Rotate(0f, 0f, degreesPerSecond * Time.deltaTime);

        // ── Quét cá trong bán kính ─────────────────────────────────────────
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, suctionRadius);

        suckedFish.Clear();

        foreach (Collider2D hit in hits)
        {
            if (!IsFish(hit.gameObject)) continue;
            suckedFish.Add(hit.transform);
        }

        // ── Hút cá về tâm ─────────────────────────────────────────────────
        for (int i = suckedFish.Count - 1; i >= 0; i--)
        {
            Transform fish = suckedFish[i];

            // Cá có thể đã bị Destroy ở frame trước
            if (fish == null) continue;

            Vector2 toCenter = (Vector2)(transform.position - fish.position);
            float dist = toCenter.magnitude;

            if (dist <= eatDistance)
            {
                // Cá đã vào tâm → ăn thịt
                krakenMove?.EatFish(fish.gameObject);
            }
            else
            {
                // Hút dần về tâm
                fish.position = Vector2.MoveTowards(
                    fish.position,
                    transform.position,
                    suctionForce * Time.deltaTime
                );
            }
        }
    }

    // ── Kiểm tra đối tượng có phải cá không ──────────────────────────────────
    bool IsFish(GameObject obj)
    {
        if (obj == krakenMove?.gameObject) return false;  // không ăn chính mình

        return obj.CompareTag("Fish")
            || obj.CompareTag("Crab")
            || obj.CompareTag("Octopus")
            || obj.CompareTag("SpecialFish")
            || obj.CompareTag("AceFish")
            || obj.GetComponent<FishCatchable>() != null
            || obj.GetComponent<FishEatable>() != null;
    }

    // ── Gizmos debug ─────────────────────────────────────────────────────────
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 0.5f, 1f, 0.3f);
        Gizmos.DrawSphere(transform.position, suctionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, eatDistance);
    }
}
