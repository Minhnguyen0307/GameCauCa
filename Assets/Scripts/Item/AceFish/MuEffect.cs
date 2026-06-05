using UnityEngine;

public class MuEffect : MonoBehaviour
{
    public float expandTime = 1f;
    public float targetRadius = 10f;

    private float timer = 0f;
    private Vector3 startScale;
    private Vector3 targetScale;
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        // Độ mờ 0.5
        sr.color = new Color(1f, 1f, 1f, 0.5f);

        startScale = Vector3.zero;
        targetScale = Vector3.one * targetRadius;

        transform.localScale = startScale;
    }

    void Update()
    {
        // Dùng unscaledDeltaTime để hiệu ứng chạy đúng kể cả khi timeScale = 0
        timer += Time.unscaledDeltaTime;
        float t = timer / expandTime;

        transform.localScale = Vector3.Lerp(startScale, targetScale, t);

        if (timer >= expandTime)
        {
            Destroy(gameObject);
        }
    }
}