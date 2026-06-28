using System.Collections;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public GameObject fishPrefab;   // prefab cá
    public int fishCount = 10;       // số lượng cá
    public float spawnDelay = 10f;   // thời gian giữa mỗi con

    public float minY = -3f;         // giới hạn vị trí Y
    public float maxY = 3f;

    void Start()
    {
        StartCoroutine(SpawnFish());
    }

    IEnumerator SpawnFish()
    {
        for (int i = 0; i < fishCount; i++)
        {
            if (FishFreezeManager.Instance == null || !FishFreezeManager.Instance.IsFrozen)
            {
                Vector3 spawnPos = new Vector3(
                    -10f,
                    Random.Range(minY, maxY),
                    0f
                );

                Instantiate(fishPrefab, spawnPos, Quaternion.identity);
                if (FishDoubleManager.Instance != null && FishDoubleManager.Instance.IsDoubleActive)
                {
                    Vector3 extraPos = spawnPos + new Vector3(0f, Random.Range(-0.3f, 0.3f), 0f);
                    Instantiate(fishPrefab, extraPos, Quaternion.identity);
                }
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }
}