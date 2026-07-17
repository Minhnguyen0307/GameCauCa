using UnityEngine;

public class BomMinigame : MonoBehaviour
{
    public float fallSpeed = 6f;
    public GameObject explosionEffectPrefab;

    private float bottomY;
    private bool exploded = false;

    void Start()
    {
        // Tính mép dưới màn hình trong world space
        if (Camera.main != null)
        {
            bottomY = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + 0.5f;
        }
        else
        {
            bottomY = -5f; // Dự phòng
        }
    }

    void Update()
    {
        if (exploded) return;

        // Di chuyển bom rơi xuống dưới
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);

        // Nổ nếu chạm đáy màn hình
        if (transform.position.y <= bottomY)
        {
            transform.position = new Vector2(transform.position.x, bottomY);
            Explode();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (exploded) return;
        HandleCollision(other.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (exploded) return;
        HandleCollision(collision.gameObject);
    }

    private void HandleCollision(GameObject otherObj)
    {
        if (otherObj == null) return;

        // Kiểm tra xem có va chạm với cá không
        FishCatchable fishCatch = otherObj.GetComponentInParent<FishCatchable>();
        if (fishCatch == null) fishCatch = otherObj.GetComponentInChildren<FishCatchable>();

        FishEatable fishEat = otherObj.GetComponentInParent<FishEatable>();
        if (fishEat == null) fishEat = otherObj.GetComponentInChildren<FishEatable>();

        bool isFish = (fishCatch != null || fishEat != null || otherObj.CompareTag("Fish") || otherObj.name.ToLower().Contains("fish"));

        // Kiểm tra xem có va chạm với quái vật không
        KrakenMove kraken = otherObj.GetComponentInParent<KrakenMove>();
        if (kraken == null) kraken = otherObj.GetComponentInChildren<KrakenMove>();

        LochNessMonsterMove lochNess = otherObj.GetComponentInParent<LochNessMonsterMove>();
        if (lochNess == null) lochNess = otherObj.GetComponentInChildren<LochNessMonsterMove>();

        bool isMonster = (kraken != null || lochNess != null);

        if (isFish)
        {
            Debug.Log($"[BomMinigame] Va chạm với cá: {otherObj.name}. Tiêu diệt cá.");
            
            // Tiêu diệt GameObject của cá (root hoặc chính nó)
            GameObject targetToDestroy = otherObj;
            if (fishCatch != null) targetToDestroy = fishCatch.gameObject;
            else if (fishEat != null) targetToDestroy = fishEat.gameObject;

            Destroy(targetToDestroy);
            Explode();
        }
        else if (isMonster)
        {
            string monsterName = kraken != null ? "Kraken" : "Loch Ness";
            Debug.Log($"[BomMinigame] Va chạm với quái vật: {monsterName} ({otherObj.name}). Gây 1 sát thương.");

            if (kraken != null)
            {
                kraken.TakeDamage(1);
            }
            else if (lochNess != null)
            {
                lochNess.TakeDamage(1);
            }
            
            // Nổ ngay khi chạm quái vật
            Explode();
        }
    }

    private void Explode()
    {
        exploded = true;

        // Tạo hiệu ứng nổ
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // Phát âm thanh nổ
        if (AudioController.Instance != null)
        {
            AudioController.Instance.PlaySFX(SoundType.bomb, 0.75f);
        }

        Destroy(gameObject);
    }
}
