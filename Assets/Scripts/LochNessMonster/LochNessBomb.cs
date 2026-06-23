using UnityEngine;

/// <summary>
/// Gắn vào prefab lochnessbomb.
/// Bay theo quỹ đạo hình parabol (vận tốc ban đầu + trọng lực), nổ khi chạm đáy màn hình hoặc va chạm vật thể.
/// </summary>
public class LochNessBomb : MonoBehaviour
{
    [Header("Explosion Settings")]
    [Tooltip("Sát thương gây ra khi nổ")]
    public int damage = 1;

    [Tooltip("Bán kính vụ nổ")]
    public float explosionRadius = 3f;

    [Tooltip("Prefab hiệu ứng nổ (VFX)")]
    public GameObject explosionEffectPrefab;

    [Header("Projectile Settings")]
    [Tooltip("Trọng lực tác dụng lên bom (đơn vị/giây^2)")]
    public float gravity = 9.8f;

    private Vector2 velocity;
    private float bottomY;
    private bool exploded = false;

    void Start()
    {
        // Tính mép dưới màn hình trong world space tương tự như BombFall
        if (Camera.main != null)
        {
            bottomY = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + 0.5f;
        }
        else
        {
            bottomY = -5f; // Giá trị dự phòng nếu không tìm thấy Camera.main
        }
    }

    /// <summary>
    /// Kích hoạt phóng bom với vận tốc ban đầu
    /// </summary>
    public void Launch(Vector2 initialVelocity)
    {
        velocity = initialVelocity;
    }

    void Update()
    {
        // Hỗ trợ đóng băng khi sử dụng Stop Fish Item
        if (FishFreezeManager.Instance != null && FishFreezeManager.Instance.IsFrozen)
            return;

        // Cập nhật vận tốc theo trọng lực
        velocity.y -= gravity * Time.deltaTime;

        // Di chuyển bom theo hệ tọa độ thế giới
        transform.Translate(velocity * Time.deltaTime, Space.World);

        // Xoay bom theo hướng bay để tạo hiệu ứng chuyển động chân thực
        if (velocity.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        // Tự nổ khi chạm đáy màn hình
        if (transform.position.y <= bottomY)
        {
            transform.position = new Vector3(transform.position.x, bottomY, transform.position.z);
            Explode();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (exploded) return;

        // Tránh tự kích nổ khi chạm vào chính Loch Ness Monster hoặc các phần của nó
        if (other.CompareTag("LochNess") || other.GetComponentInParent<LochNessMonsterMove>() != null)
            return;

        Explode();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (exploded) return;

        // Tránh tự kích nổ khi va chạm với Loch Ness Monster
        if (collision.gameObject.CompareTag("LochNess") || collision.gameObject.GetComponentInParent<LochNessMonsterMove>() != null)
            return;

        Explode();
    }

    void Explode()
    {
        exploded = true;

        // Tìm tất cả các collider trong bán kính vụ nổ
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        System.Collections.Generic.HashSet<GameObject> damagedObjects = new System.Collections.Generic.HashSet<GameObject>();

        foreach (Collider2D hit in hits)
        {
            GameObject root = hit.attachedRigidbody != null
                ? hit.attachedRigidbody.gameObject
                : hit.gameObject;

            if (damagedObjects.Contains(root)) continue;
            damagedObjects.Add(root);

            // Gây sát thương nếu đối tượng có thành phần Health (như cá lớn)
            Health hp = root.GetComponentInChildren<Health>();
            if (hp != null)
            {
                hp.TakeDamage(damage);
                continue;
            }

            // Nếu là cá thường thì tiêu diệt ngay lập tức
            FishCatchable fish = root.GetComponent<FishCatchable>();
            if (fish != null)
            {
                Destroy(root);
            }
        }

        // Tạo hiệu ứng vụ nổ VFX nếu có
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // Phát âm thanh tiếng nổ bom
        if (AudioController.Instance != null)
        {
            AudioController.Instance.PlaySFX(SoundType.bomb);
        }

        // Hủy quả bom sau khi nổ
        Destroy(gameObject);
    }

    // Vẽ bán kính vụ nổ trong Editor để tiện debug
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
