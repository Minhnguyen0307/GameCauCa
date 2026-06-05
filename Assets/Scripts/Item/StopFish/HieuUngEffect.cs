using UnityEngine;

public class HieuUngEffect : MonoBehaviour
{
    public float duration = 0.2f;
    public float maxScale = 1.5f;

    private float timer;
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        // Bắt đầu từ nhỏ và mờ 0.7
        transform.localScale = Vector3.zero;
        sr.color = new Color(1f, 1f, 1f, 0.7f);
    }

    void Update()
    {
        // Dùng unscaledDeltaTime để hiệu ứng chạy đúng kể cả khi timeScale = 0
        timer += Time.unscaledDeltaTime;
        float t = timer / duration;

        // Phóng to
        transform.localScale =
            Vector3.Lerp(Vector3.zero, Vector3.one * maxScale, t);

        // Mờ dần từ 0.7 → 0
        sr.color =
            new Color(1f, 1f, 1f, Mathf.Lerp(0.7f, 0f, t));

        if (timer >= duration)
        {
            Destroy(gameObject);
        }
    }
}