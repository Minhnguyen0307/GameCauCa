using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Spawn The Kraken từ bên phải màn hình mỗi 100 giây.
/// Kraken xuất hiện cách mép màn hình 100 đơn vị về phía phải.
/// Chỉ hoạt động trong Scene "Level_BinhThuong".
/// Gắn script này vào một GameObject rỗng trong Scene (ví dụ: "KrakenSpawner").
/// </summary>
public class KrakenSpawner : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject krakenPrefab;         // Kéo prefab Kraken vào đây

    [Header("Spawn Settings")]
    [Tooltip("Khoảng cách spawn tính từ mép phải màn hình (đơn vị world)")]
    public float spawnDistance = 100f;      // 100 ô cách mép phải

    [Tooltip("Thời gian giữa mỗi lần xuất hiện (giây)")]
    public float spawnInterval = 100f;      // 100 giây / lần

    [Tooltip("Spawn ngay khi game bắt đầu không?")]
    public bool spawnOnStart = false;

    [Header("Spawn Y Range")]
    [Tooltip("Giới hạn Y tối thiểu để spawn Kraken")]
    public float minY = -2f;
    [Tooltip("Giới hạn Y tối đa để spawn Kraken")]
    public float maxY = 2f;

    // Tên scene duy nhất được phép spawn Kraken
    private const string ALLOWED_SCENE = "Level_BinhThuong";

    void Start()
    {
        // Chỉ chạy trong scene Level_BinhThuong
        if (SceneManager.GetActiveScene().name != ALLOWED_SCENE)
        {
            Debug.Log($"[KrakenSpawner] Scene hiện tại không phải \"{ALLOWED_SCENE}\", Kraken sẽ không xuất hiện.");
            enabled = false;
            return;
        }

        if (krakenPrefab == null)
        {
            Debug.LogWarning("[KrakenSpawner] Chưa gán krakenPrefab!");
            return;
        }

        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        // Chờ trước lần spawn đầu (nếu không muốn spawn ngay)
        if (!spawnOnStart)
        {
            yield return new WaitForSeconds(spawnInterval);
        }

        while (true)
        {
            SpawnKraken();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnKraken()
    {
        Camera cam = Camera.main;

        // Tính mép phải màn hình trong world space
        float cameraRight = cam.transform.position.x
                            + cam.orthographicSize * cam.aspect;

        // Spawn cách mép phải 100 đơn vị
        Vector3 spawnPos = new Vector3(
            cameraRight + spawnDistance,
            Random.Range(minY, maxY),
            0f
        );

        GameObject kraken = Instantiate(krakenPrefab, spawnPos, Quaternion.identity);

        Debug.Log($"[KrakenSpawner] The Kraken xuất hiện tại {spawnPos}");
    }
}
