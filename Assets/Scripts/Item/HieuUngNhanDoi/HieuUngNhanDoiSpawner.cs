using UnityEngine;
using System.Collections;

public class HieuUngNhanDoiSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject nhanDoiPrefab;      // Prefab hieuungnhandoi 1
    public float spawnInterval = 30f;       // Thời gian giữa mỗi lần spawn
    public float spawnDistance = 2f;       // Khoảng cách bên dưới camera để bắt đầu bay lên

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        // Chờ khoảng thời gian đầu trước khi spawn
        yield return new WaitForSeconds(spawnInterval);

        while (true)
        {
            SpawnItem();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnItem()
    {
        // Bỏ qua nếu game đang bị đóng băng bởi StopFishItem
        if (FishFreezeManager.Instance != null && FishFreezeManager.Instance.IsFrozen)
            return;

        if (cam == null) cam = Camera.main;
        if (cam == null) return;

        float cameraBottom = cam.transform.position.y - cam.orthographicSize;
        float halfWidth = cam.orthographicSize * cam.aspect;

        // Vị trí X ngẫu nhiên trong màn hình (tránh sát mép camera quá)
        float spawnX = Random.Range(-halfWidth + 1f, halfWidth - 1f);
        Vector3 spawnPos = new Vector3(
            spawnX,
            cameraBottom - spawnDistance,
            0f
        );

        if (nhanDoiPrefab != null)
        {
            Instantiate(nhanDoiPrefab, spawnPos, Quaternion.identity);
            Debug.Log("[HieuUngNhanDoiSpawner] Item spawned successfully.");
        }
        else
        {
            Debug.LogWarning("[HieuUngNhanDoiSpawner] Prefab is not assigned in the Inspector!");
        }
    }
}
