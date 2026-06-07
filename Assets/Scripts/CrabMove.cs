using UnityEngine;

public class CrabMove : MonoBehaviour
{
    public float speed = 2f;
    public float groundY = -4.5f;
    public float fallSpeed = 8f;   // tốc độ rơi xuống đáy sau khi hết xoáy

    private int direction = 1;
    private Camera cam;
    private bool isBeingSucked = false;

    void Start()
    {
        cam = Camera.main;

        transform.position = new Vector3(
            transform.position.x,
            groundY,
            transform.position.z
        );
    }

    void Update()
    {
        // Kiểm tra xem cua có đang bị xoáy nước hút không
        // (khi bị hút, Y sẽ khác groundY đáng kể)
        bool currentlySucked = Mathf.Abs(transform.position.y - groundY) > 0.1f
                               && IsNearActiveVortex();

        if (currentlySucked)
        {
            // Đang bị hút → không can thiệp, để XoayNuocEffect kéo position
            isBeingSucked = true;
            return;
        }

        if (isBeingSucked)
        {
            // Vừa thoát khỏi xoáy → rơi xuống đáy
            float newY = Mathf.MoveTowards(transform.position.y, groundY, fallSpeed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            if (Mathf.Abs(transform.position.y - groundY) < 0.05f)
            {
                // Đã chạm đáy → về trạng thái bình thường
                transform.position = new Vector3(transform.position.x, groundY, transform.position.z);
                isBeingSucked = false;
            }
            return;
        }

        // Trạng thái bình thường: bò ngang ở đáy
        if (FishFreezeManager.Instance != null && FishFreezeManager.Instance.IsFrozen)
            return;

        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        transform.position = new Vector3(
            transform.position.x,
            groundY,
            transform.position.z
        );

        float leftEdge  = cam.transform.position.x - cam.orthographicSize * cam.aspect;
        float rightEdge = cam.transform.position.x + cam.orthographicSize * cam.aspect;

        if (transform.position.x < leftEdge || transform.position.x > rightEdge)
        {
            direction *= -1;
            Flip();
        }
    }

    // Kiểm tra có xoáy nước đang hoạt động gần cua không
    bool IsNearActiveVortex()
    {
        XoayNuocEffect vortex = FindObjectOfType<XoayNuocEffect>();
        if (vortex == null || !vortex.gameObject.activeInHierarchy) return false;

        float dist = Vector2.Distance(transform.position, vortex.transform.position);
        return dist <= vortex.suctionRadius;
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}