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
        Camera cam = Camera.main;

        float cameraRight =
            cam.transform.position.x + cam.orthographicSize * cam.aspect;

        Vector3 spawnPos = new Vector3(
            cameraRight + spawnDistance,
            Random.Range(-2f, 2f),
            0f
        );

        Instantiate(specialFishPrefab, spawnPos, Quaternion.identity);
    }
}