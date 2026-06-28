using UnityEngine;

public class FishDoubleManager : MonoBehaviour
{
    public static FishDoubleManager Instance { get; private set; }

    private float doubleSpawnTimer = 0f;
    private float slowDownTimer = 0f;

    public bool IsDoubleActive => doubleSpawnTimer > 0f;
    public bool IsSlowDownActive => slowDownTimer > 0f;
    public float SpeedMultiplier => IsSlowDownActive ? 0.5f : 1.0f;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        GameObject go = new GameObject("FishDoubleManager");
        go.AddComponent<FishDoubleManager>();
        DontDestroyOnLoad(go);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (doubleSpawnTimer > 0f)
        {
            doubleSpawnTimer -= Time.deltaTime;
        }

        if (slowDownTimer > 0f)
        {
            slowDownTimer -= Time.deltaTime;
        }
    }

    public void ActivateDoubleSpawn(float duration)
    {
        doubleSpawnTimer = duration;
        Debug.Log($"[FishDoubleManager] Double spawn activated for {duration}s");
    }

    public void ActivateSlowDown(float duration)
    {
        slowDownTimer = duration;
        Debug.Log($"[FishDoubleManager] Slow down activated for {duration}s");
    }
}
