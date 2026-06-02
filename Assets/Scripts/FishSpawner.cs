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
            Vector3 spawnPos = new Vector3(
                -10f, // xuất hiện từ bên trái (bạn đổi nếu cần)
                Random.Range(minY, maxY),
                0f
            );

            Instantiate(fishPrefab, spawnPos, Quaternion.identity);

            yield return new WaitForSeconds(spawnDelay);
        }
    }
}