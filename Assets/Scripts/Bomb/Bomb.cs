using UnityEngine;

public class Bomb : MonoBehaviour
{
    public int damage = 1;
    public float explosionRadius = 3f;
    public GameObject explosionEffectPrefab; // prefab ExplosionEffect

    private bool exploded = false;

    void Explode()
    {
        if (exploded) return;
        exploded = true;

        // 💥 DAMAGE
        Collider2D[] hits =
            Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D hit in hits)
        {
            Health hp = hit.GetComponent<Health>();
            if (hp != null)
            {
                hp.TakeDamage(damage);
            }
        }

        // 🎆 SPAWN HIỆU ỨNG NỔ (ĐỘC LẬP)
        if (explosionEffectPrefab != null)
        {
            Instantiate(
                explosionEffectPrefab,
                transform.position,
                Quaternion.identity
            );
        }

        // ❌ HỦY BOM
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Health>() != null)
        {
            Explode();
        }
    }
}