using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    public GameObject bombPrefab;
    public float spawnInterval = 2f;
    public int maxBombs = 3;

    void Start()
    {
        InvokeRepeating(nameof(SpawnBomb), 1f, spawnInterval);
    }

    void SpawnBomb()
    {
        if (GameObject.FindGameObjectsWithTag("Bomb").Length >= maxBombs)
            return;

        float x = Random.Range(
            Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x,
            Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x
        );

        float y = Camera.main.ViewportToWorldPoint(
            new Vector3(0, 1, 0)
        ).y + 1f;

        Instantiate(bombPrefab, new Vector2(x, y), Quaternion.identity);
    }
}