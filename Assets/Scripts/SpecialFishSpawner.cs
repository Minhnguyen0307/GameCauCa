using UnityEngine;
using System.Collections;

public class SpecialFishSpawner : MonoBehaviour
{
    public GameObject specialFishPrefab;
    public float spawnDistance = 25f;
    public float spawnInterval = 50f;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnSpecialFish();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnSpecialFish()
    {
        if (FishFreezeManager.Instance != null && FishFreezeManager.Instance.IsFrozen)
            return;

        Camera cam = Camera.main;

        float cameraRight =
            cam.transform.position.x + cam.orthographicSize * cam.aspect;

        Vector3 spawnPos = new Vector3(
            cameraRight + spawnDistance,
            Random.Range(-2f, 2f),
            0f
        );

        Instantiate(specialFishPrefab, spawnPos, Quaternion.identity);
        if (FishDoubleManager.Instance != null && FishDoubleManager.Instance.IsDoubleActive)
        {
            Vector3 extraPos = spawnPos + new Vector3(0f, Random.Range(-0.3f, 0.3f), 0f);
            Instantiate(specialFishPrefab, extraPos, Quaternion.identity);
        }
    }
}