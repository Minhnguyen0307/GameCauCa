using UnityEngine;
using System.Collections;

public class SharkSpawner : MonoBehaviour
{
    public GameObject sharkPrefab;
    public float spawnDistance = 30f;
    public float spawnInterval = 90f;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnShark();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnShark()
    {
        Camera cam = Camera.main;
        float cameraRight =
            cam.transform.position.x + cam.orthographicSize * cam.aspect;

        Vector3 spawnPos = new Vector3(
            cameraRight + spawnDistance,
            0f,
            0f
        );

        Instantiate(sharkPrefab, spawnPos, Quaternion.identity);
    }
}