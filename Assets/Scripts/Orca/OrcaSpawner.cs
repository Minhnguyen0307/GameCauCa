using UnityEngine;
using System.Collections;

public class OrcaSpawner : MonoBehaviour
{
    public GameObject orcaPrefab;
    public float spawnDelay = 60f;
    public float spawnDistance = 50f;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnOrca();
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void SpawnOrca()
    {
        Camera cam = Camera.main;
        float cameraLeft =
            cam.transform.position.x - cam.orthographicSize * cam.aspect;

        Vector3 spawnPos = new Vector3(
            cameraLeft - spawnDistance,
            0f,
            0f
        );

        Instantiate(orcaPrefab, spawnPos, Quaternion.identity);
    }
}