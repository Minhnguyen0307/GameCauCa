using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PhaHuyBomSpawner : MonoBehaviour
{
    public float spawnInterval = 20f;
    public float spawnDistance = 8f;
    public float itemSpeed = 2f;

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        // Chờ khoảng thời gian đầu tiên trước khi spawn lần đầu
        yield return new WaitForSeconds(spawnInterval);

        while (true)
        {
            SpawnItem();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnItem()
    {
        // Bỏ qua spawn nếu game đang bị đóng băng (tương tự LochNessMonsterSpawner)
        if (FishFreezeManager.Instance != null && FishFreezeManager.Instance.IsFrozen)
            return;

        if (cam == null) cam = Camera.main;
        if (cam == null) return;

        float camBottom = cam.transform.position.y - cam.orthographicSize;
        float halfWidth = cam.orthographicSize * cam.aspect;

        // Vị trí X ngẫu nhiên trong phạm vi chiều rộng màn hình
        float x = Random.Range(-halfWidth + 1f, halfWidth - 1f);
        Vector3 spawnPos = new Vector3(x, camBottom - spawnDistance, 0f);

        GameObject item = null;

        // Thử tải prefab từ thư mục Resources
        GameObject prefab = Resources.Load<GameObject>("hieuungquetbom_0");
        if (prefab != null)
        {
            item = Instantiate(prefab, spawnPos, Quaternion.identity);
            Debug.Log("[PhaHuyBomSpawner] Spawned item from Resources prefab.");
        }
        else
        {
            // Dự phòng: Tự động dựng GameObject cho item bằng code
            item = new GameObject("PhaHuyBom_Item");
            item.transform.position = spawnPos;
            item.transform.localScale = new Vector3(0.18f, 0.18f, 1f);

            // Thêm SpriteRenderer và tải sprite
            SpriteRenderer sr = item.AddComponent<SpriteRenderer>();
            Sprite[] sprites = Resources.LoadAll<Sprite>("hieuungquetbom");
            if (sprites != null && sprites.Length > 0)
            {
                sr.sprite = sprites[0];
            }
            else
            {
                Debug.LogWarning("[PhaHuyBomSpawner] Failed to load 'hieuungquetbom' sprite from Resources!");
            }
            sr.sortingOrder = 5; // Vẽ đè lên trên background và cá
            Debug.Log("[PhaHuyBomSpawner] Spawned item using dynamic GameObject construction.");
        }

        if (item != null)
        {
            // Đảm bảo BoxCollider2D (dạng trigger) tồn tại
            BoxCollider2D col = item.GetComponent<BoxCollider2D>();
            if (col == null)
            {
                col = item.AddComponent<BoxCollider2D>();
            }
            col.isTrigger = true;

            // Đảm bảo logic PhaHuyBomItem tồn tại và có tốc độ chính xác
            PhaHuyBomItem itemScript = item.GetComponent<PhaHuyBomItem>();
            if (itemScript == null)
            {
                itemScript = item.AddComponent<PhaHuyBomItem>();
            }
            itemScript.speed = itemSpeed;
        }
    }
}
