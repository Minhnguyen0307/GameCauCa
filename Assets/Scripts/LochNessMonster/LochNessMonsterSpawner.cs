using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Quản lý việc spawn Loch Ness Monster.
/// Chỉ hoạt động trong scene "Level_CucKho".
/// Xuất hiện mỗi 20s một lần từ bên trái màn hình.
/// </summary>
public class LochNessMonsterSpawner : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject lochNessPrefab;        // Gán prefab lochnessmonster_0 vào đây

    [Header("Spawn Settings")]
    [Tooltip("Khoảng cách spawn tính từ mép trái màn hình (đơn vị world)")]
    public float spawnOffset = 5f;

    [Tooltip("Thời gian giữa mỗi lần xuất hiện (giây)")]
    public float spawnInterval = 20f;

    [Tooltip("Spawn ngay khi game bắt đầu không?")]
    public bool spawnOnStart = false;

    [Header("Spawn Y Range")]
    [Tooltip("Giới hạn Y tối thiểu để spawn")]
    public float minY = -3f;
    [Tooltip("Giới hạn Y tối đa để spawn")]
    public float maxY = 3f;

    void Start()
    {
        // Chỉ chạy trong scene Level_CucKho
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName != "Level_CucKho")
        {
            Debug.Log($"[LochNessMonsterSpawner] Scene hiện tại không phải \"Level_CucKho\". Loch Ness Monster sẽ không xuất hiện.");
            enabled = false;
            return;
        }

        if (lochNessPrefab == null)
        {
            Debug.LogWarning("[LochNessMonsterSpawner] Chưa gán lochNessPrefab!");
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
            SpawnMonster();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnMonster()
    {
        // Không spawn khi game bị đóng băng bởi Stop Fish Item
        if (FishFreezeManager.Instance != null && FishFreezeManager.Instance.IsFrozen)
            return;

        Camera cam = Camera.main;
        if (cam == null) return;

        // Tính mép trái màn hình trong world space
        float cameraLeft = cam.transform.position.x - cam.orthographicSize * cam.aspect;

        // Spawn ngoài rìa trái màn hình
        Vector3 spawnPos = new Vector3(
            cameraLeft - spawnOffset,
            Random.Range(minY, maxY),
            0f
        );

        Instantiate(lochNessPrefab, spawnPos, Quaternion.identity);

        Debug.Log($"[LochNessMonsterSpawner] Loch Ness Monster xuất hiện tại {spawnPos}");
    }
}
