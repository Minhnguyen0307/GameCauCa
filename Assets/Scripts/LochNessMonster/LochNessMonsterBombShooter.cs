using UnityEngine;

/// <summary>
/// Gắn vào prefab lochnessmonster_0.
/// Tự động bắn bom từ vị trí miệng (Mouth) theo khoảng thời gian định sẵn.
/// </summary>
public class LochNessMonsterBombShooter : MonoBehaviour
{
    [Header("Bomb Settings")]
    [Tooltip("Prefab quả bom của Loch Ness (lochnessbomb)")]
    public GameObject bombPrefab;

    [Tooltip("Vị trí miệng của quái vật (Nếu trống, sẽ tự tìm con tên 'Mouth' hoặc dùng offset mặc định)")]
    public Transform mouthTransform;

    [Tooltip("Offset vị trí miệng mặc định nếu không gán mouthTransform (tính từ tâm quái vật)")]
    public Vector3 localMouthOffset = new Vector3(2.0f, 1.2f, 0f);

    [Header("Shooting Settings")]
    [Tooltip("Thời gian chờ trước phát bắn đầu tiên (giây)")]
    public float initialDelay = 1.5f;

    [Tooltip("Khoảng thời gian giữa các lần bắn bom (giây)")]
    public float shootInterval = 3f;

    [Tooltip("Lực phóng bom ban đầu (X: bắn về phía trước, Y: bắn lên/xuống)")]
    public Vector2 launchSpeed = new Vector2(3f, 4f);

    private float shootTimer;
    private LochNessMonsterMove monsterMove;

    void Start()
    {
        shootTimer = initialDelay;
        monsterMove = GetComponent<LochNessMonsterMove>();

        // Nếu chưa gán mouthTransform, thử tìm GameObject con có tên "Mouth" hoặc "FirePoint"
        if (mouthTransform == null)
        {
            Transform foundMouth = transform.Find("Mouth");
            if (foundMouth == null)
            {
                foundMouth = transform.Find("FirePoint");
            }
            mouthTransform = foundMouth;
        }
    }

    void Update()
    {
        // Không bắn bom khi game bị đóng băng bởi Stop Fish Item
        if (FishFreezeManager.Instance != null && FishFreezeManager.Instance.IsFrozen)
            return;

        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            ShootBomb();
            shootTimer = shootInterval;
        }
    }

    void ShootBomb()
    {
        if (bombPrefab == null)
        {
            Debug.LogWarning("[LochNessMonsterBombShooter] Chưa gán bombPrefab cho quái vật!");
            return;
        }

        // Xác định vị trí miệng để bắn bom (chuyển đổi localMouthOffset sang thế giới nếu dùng offset)
        Vector3 spawnPos = mouthTransform != null 
            ? mouthTransform.position 
            : transform.TransformPoint(localMouthOffset);

        // Tạo quả bom mới
        GameObject bombObj = Instantiate(bombPrefab, spawnPos, Quaternion.identity);
        LochNessBomb lochNessBomb = bombObj.GetComponent<LochNessBomb>();

        if (lochNessBomb != null)
        {
            // Cộng tốc độ di chuyển hiện tại của Loch Ness Monster để quả bom bay tự nhiên hơn
            float parentSpeedX = monsterMove != null ? monsterMove.speed : 0f;
            Vector2 initialVelocity = new Vector2(parentSpeedX + launchSpeed.x, launchSpeed.y);

            lochNessBomb.Launch(initialVelocity);
        }
        else
        {
            Debug.LogWarning("[LochNessMonsterBombShooter] Prefab bom không chứa component LochNessBomb!");
        }
    }

    // Vẽ vị trí bắn bom trong Editor để tiện căn chỉnh
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 mouthPos = mouthTransform != null 
            ? mouthTransform.position 
            : transform.TransformPoint(localMouthOffset);
            
        Gizmos.DrawWireSphere(mouthPos, 0.2f);
    }
}
