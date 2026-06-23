using UnityEngine;

/// <summary>
/// Gắn vào prefab lochnessmonster_0.
/// Di chuyển từ trái sang phải, tự hủy sau khi đi được quãng đường travelDistance (30 ô).
/// </summary>
public class LochNessMonsterMove : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Tốc độ di chuyển của Loch Ness Monster")]
    public float speed = 2f;

    [Tooltip("Quãng đường tối đa để tự hủy (ô/đơn vị)")]
    public float travelDistance = 30f;

    private float startX;

    void Start()
    {
        // Ghi lại vị trí X ban đầu để tính quãng đường đi được
        startX = transform.position.x;

        // Sprite mặc định đã hướng sang Phải, đảm bảo scale.x là số dương
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    void Update()
    {
        // Hỗ trợ đóng băng cá qua FishFreezeManager
        if (FishFreezeManager.Instance != null && FishFreezeManager.Instance.IsFrozen)
            return;

        // Di chuyển sang phải theo tọa độ thế giới (Space.World)
        transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);

        // Kiểm tra nếu đã đi đủ khoảng cách yêu cầu (30 ô) thì tự hủy
        if (transform.position.x - startX >= travelDistance)
        {
            Destroy(gameObject);
        }
    }
}
