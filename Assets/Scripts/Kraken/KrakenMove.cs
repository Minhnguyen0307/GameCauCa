using UnityEngine;
using System.Collections;

/// <summary>
/// Luồng hoạt động của Kraken:
/// 1. Di chuyển từ phải → trái
/// 2. Khi vào vùng camera → dừng lại tại vị trí ngẫu nhiên trong camera
/// 3. Bật xoáy nước hút cá trong 5 giây
/// 4. Tắt xoáy nước → tiếp tục di chuyển sang trái cho đến khi ra khỏi màn hình
/// </summary>
public class KrakenMove : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 3f;

    [Header("Vortex")]
    [Tooltip("GameObject con chứa hiệu ứng xoáy nước (xoaynuoc)")]
    public GameObject xoayNuocObject;

    [Tooltip("Thời gian dừng và hút cá (giây)")]
    public float vortexDuration = 5f;

    [Header("Effects")]
    public GameObject eatEffectPrefab;

    // ── Private state ─────────────────────────────────────────────────────────
    private Camera mainCam;
    private float leftBoundary;

    private enum KrakenState { Moving, Vortex, Done }
    private KrakenState state = KrakenState.Moving;

    void Start()
    {
        mainCam = Camera.main;

        leftBoundary = mainCam.transform.position.x
                       - mainCam.orthographicSize * mainCam.aspect
                       - 20f;

        if (xoayNuocObject != null)
            xoayNuocObject.SetActive(false);
        else
            Debug.LogWarning("[KrakenMove] Chưa gán xoayNuocObject!");
    }

    void Update()
    {
        switch (state)
        {
            case KrakenState.Moving:
                MoveLeft();

                // Vào giữa camera (x <= 0.5 viewport) → dừng lại và kích hoạt vortex
                if (IsAtCenterCamera())
                {
                    state = KrakenState.Vortex;
                    StartCoroutine(VortexRoutine());
                }
                break;

            case KrakenState.Vortex:
                // Đứng yên, XoayNuocEffect tự xử lý hút cá
                break;

            case KrakenState.Done:
                MoveLeft();

                // Tự hủy khi ra khỏi màn hình
                if (transform.position.x < leftBoundary)
                    Destroy(gameObject);
                break;
        }
    }

    // ── Di chuyển sang trái ───────────────────────────────────────────────────
    void MoveLeft()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    // ── Kiểm tra Kraken đã đến giữa camera chưa (viewport x <= 0.5) ──────────
    bool IsAtCenterCamera()
    {
        Vector3 vp = mainCam.WorldToViewportPoint(transform.position);
        return vp.x <= 0.5f;
    }

    // ── Coroutine xoáy nước ───────────────────────────────────────────────────
    IEnumerator VortexRoutine()
    {
        // Dừng tại vị trí ngẫu nhiên trong camera (trục Y)
        float camHalfHeight = mainCam.orthographicSize;
        float randomY = Random.Range(
            mainCam.transform.position.y - camHalfHeight * 0.8f,
            mainCam.transform.position.y + camHalfHeight * 0.8f
        );
        transform.position = new Vector3(transform.position.x, randomY, transform.position.z);

        // Bật xoáy nước
        if (xoayNuocObject != null)
            xoayNuocObject.SetActive(true);

        Debug.Log($"[KrakenMove] Xoáy nước bắt đầu tại {transform.position}, kéo dài {vortexDuration}s");

        // Chờ hết thời gian vortex
        yield return new WaitForSeconds(vortexDuration);

        // Tắt xoáy nước
        if (xoayNuocObject != null)
            xoayNuocObject.SetActive(false);

        Debug.Log("[KrakenMove] Xoáy nước kết thúc, Kraken tiếp tục di chuyển.");

        state = KrakenState.Done;
    }

    // ── Va chạm trực tiếp (Trigger) ───────────────────────────────────────────
    void OnTriggerEnter2D(Collider2D other)
    {
        EatFish(other.gameObject);
    }

    // ── Va chạm trực tiếp (Physics) ───────────────────────────────────────────
    void OnCollisionEnter2D(Collision2D collision)
    {
        EatFish(collision.gameObject);
    }

    // ── Public: cho XoayNuocEffect gọi khi cá bị hút vào tâm ────────────────
    public void EatFish(GameObject target)
    {
        if (target == null || target == gameObject) return;

        bool isFish = target.CompareTag("Fish")
                   || target.CompareTag("Crab")
                   || target.CompareTag("Octopus")
                   || target.CompareTag("SpecialFish")
                   || target.CompareTag("AceFish")
                   || target.GetComponent<FishCatchable>() != null
                   || target.GetComponent<FishEatable>() != null;

        if (!isFish) return;

        if (eatEffectPrefab != null)
            Instantiate(eatEffectPrefab, target.transform.position, Quaternion.identity);

        Destroy(target);
    }
}
