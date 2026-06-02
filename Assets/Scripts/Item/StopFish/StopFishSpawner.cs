using UnityEngine;
using System.Collections;

public class StopFishSpawner : MonoBehaviour
{
    public GameObject stopFishPrefab;
    public float spawnInterval = 40f;
    public float spawnDistance = 20f;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnStopFish();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnStopFish()
    {
        Camera cam = Camera.main;

        float cameraBottom =
            cam.transform.position.y - cam.orthographicSize;

        Vector3 spawnPos = new Vector3(
            Random.Range(-6f, 6f),
            cameraBottom - spawnDistance,
            0f
        );

        Instantiate(stopFishPrefab, spawnPos, Quaternion.identity);
    }
}