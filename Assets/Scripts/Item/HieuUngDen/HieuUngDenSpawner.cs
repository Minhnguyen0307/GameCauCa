using UnityEngine;
using System.Collections;

public class HieuUngDenSpawner : MonoBehaviour
{
    [Header("Prefab Setup (Optional - will auto-generate if null)")]
    public GameObject hieuUngDenPrefab;

    public float spawnInterval = 15f;
    public float spawnDistance = 20f;
    public float itemSpeed = 2f;

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        // Wait first interval before first spawn
        yield return new WaitForSeconds(spawnInterval);

        while (true)
        {
            SpawnItem();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnItem()
    {
        if (cam == null) cam = Camera.main;
        if (cam == null) return;

        float camBottom = cam.transform.position.y - cam.orthographicSize;
        float halfWidth = cam.orthographicSize * cam.aspect;

        // Random X position within the viewport width
        float x = Random.Range(-halfWidth + 1f, halfWidth - 1f);
        Vector3 spawnPos = new Vector3(x, camBottom - spawnDistance, 0f);

        GameObject item;

        if (hieuUngDenPrefab != null)
        {
            item = Instantiate(hieuUngDenPrefab, spawnPos, Quaternion.identity);
            
            // Sync speed from spawner settings
            HieuUngDenItem itemScript = item.GetComponent<HieuUngDenItem>();
            if (itemScript == null)
            {
                itemScript = item.AddComponent<HieuUngDenItem>();
            }
            itemScript.speed = itemSpeed;
        }
        else
        {
            // Dynamically build the item GameObject (fallback)
            item = new GameObject("HieuUngDen_Item");
            item.transform.position = spawnPos;
            item.transform.localScale = new Vector3(0.25f, 0.25f, 1f);

            // Add SpriteRenderer and load the sprite
            SpriteRenderer sr = item.AddComponent<SpriteRenderer>();
            Sprite[] sprites = Resources.LoadAll<Sprite>("hieuungden");
            if (sprites != null && sprites.Length > 0)
            {
                sr.sprite = sprites[0];
            }
            else
            {
                Debug.LogWarning("[HieuUngDenSpawner] Failed to load 'hieuungden' sprite from Resources!");
            }
            sr.sortingOrder = 5; // Render on top of backgrounds/fish

            // Add BoxCollider2D (as trigger)
            BoxCollider2D col = item.AddComponent<BoxCollider2D>();
            col.isTrigger = true;

            // Add movement/interaction logic
            HieuUngDenItem itemScript = item.AddComponent<HieuUngDenItem>();
            itemScript.speed = itemSpeed;
        }

        Debug.Log($"[HieuUngDenSpawner] Spawned Black Effect Item at {spawnPos}");
    }
}
