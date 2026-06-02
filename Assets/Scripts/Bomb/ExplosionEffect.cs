using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    public float growSpeed = 6f;
    public float lifeTime = 0.4f;

    private float timer;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        transform.localScale = Vector3.zero;
    }

    void Update()
    {
        transform.localScale += Vector3.one * growSpeed * Time.deltaTime;

        timer += Time.deltaTime;
        float alpha = Mathf.Lerp(1f, 0f, timer / lifeTime);

        sr.color = new Color(1f, 1f, 1f, alpha);

        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}