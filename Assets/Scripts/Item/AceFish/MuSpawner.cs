using UnityEngine;
using System.Collections;

public class MuSpawner : MonoBehaviour
{
    public GameObject muPrefab;
    public float spawnInterval = 20f;
    public float distanceFromCamera = 8f;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnMu();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnMu()
    {
        float camBottom =
            cam.transform.position.y - cam.orthographicSize;

        Vector3 spawnPos = new Vector3(
            Random.Range(-cam.orthographicSize * cam.aspect,
                         cam.orthographicSize * cam.aspect),
            camBottom - distanceFromCamera,
            0
        );

        Instantiate(muPrefab, spawnPos, Quaternion.identity);
    }
}