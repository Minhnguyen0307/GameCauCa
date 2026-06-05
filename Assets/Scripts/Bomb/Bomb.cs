using UnityEngine;

public class Bomb : MonoBehaviour
{
    public int damage = 1;
    public float explosionRadius = 3f;
    public GameObject explosionEffectPrefab;

    private bool exploded = false;
    private BombFall bombFall;

    void Start()
    {
        bombFall = GetComponent<BombFall>();
    }

    void Update()
    {
        // Nổ khi bom chạm đáy (BombFall đã dừng)
        if (!exploded && bombFall != null && bombFall.IsStopped)
        {
            Explode();
        }
    }

    // Vẫn giữ OnCollisionEnter2D phòng trường hợp có collider vật lý khác
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (exploded) return;
        Explode();
    }

    // Trigger với cá hoặc các object khác
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (exploded) return;
        Explode();
    }

    void Explode()
    {
        exploded = true;

        Collider2D[] hits =
            Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        // Dùng HashSet để tránh damage cùng một object nhiều lần
        // (trường hợp object có nhiều collider)
        var damagedObjects = new System.Collections.Generic.HashSet<GameObject>();

        foreach (Collider2D hit in hits)
        {
            GameObject root = hit.attachedRigidbody != null
                ? hit.attachedRigidbody.gameObject
                : hit.gameObject;

            if (damagedObjects.Contains(root)) continue;
            damagedObjects.Add(root);

            // Chỉ damage qua Health — tôn trọng HP đã cài đặt
            Health hp = root.GetComponentInChildren<Health>();
            if (hp != null)
            {
                hp.TakeDamage(damage);
                continue; // object có Health thì dùng Health xử lý, không Destroy thẳng
            }

            // Object không có Health (cá bình thường) thì Destroy luôn
            FishCatchable fish = root.GetComponent<FishCatchable>();
            if (fish != null)
            {
                Destroy(root);
            }
        }

        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        if (AudioController.Instance != null)
            AudioController.Instance.PlaySFX(SoundType.bomb);

        Destroy(gameObject);
    }
}