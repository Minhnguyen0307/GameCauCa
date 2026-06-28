using UnityEngine;
using System.Collections;

public class ClockSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject clockPrefab;          // Prefab clock
    public float spawnInterval = 30f;       // Thời gian giữa mỗi lần spawn
    public float spawnDistance = 2f;        // Khoảng cách bên dưới camera để bắt đầu bay lên

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

        if (clockPrefab != null)
        {
            GameObject clockObj = Instantiate(clockPrefab, spawnPos, Quaternion.identity);
            
            // Đảm bảo có BoxCollider2D trigger
            BoxCollider2D col = clockObj.GetComponent<BoxCollider2D>();
            if (col == null)
            {
                col = clockObj.AddComponent<BoxCollider2D>();
            }
            col.isTrigger = true;

            // Đảm bảo có script ClockItem
            ClockItem itemScript = clockObj.GetComponent<ClockItem>();
            if (itemScript == null)
            {
                itemScript = clockObj.AddComponent<ClockItem>();
            }

            Debug.Log("[ClockSpawner] Clock item spawned successfully.");
        }
        else
        {
            Debug.LogWarning("[ClockSpawner] Prefab is not assigned in the Inspector!");
        }
    }
}
