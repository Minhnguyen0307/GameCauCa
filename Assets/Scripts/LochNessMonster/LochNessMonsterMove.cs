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

    [Header("Minigame Health Settings")]
    public int maxHP = 175;
    private int currentHP
    {
        get
        {
            if (minigamePersistedHP == null)
            {
                minigamePersistedHP = maxHP;
            }
            return minigamePersistedHP.Value;
        }
        set
        {
            minigamePersistedHP = value;
        }
    }
    private bool isMinigame = false;
    public float hpTextOffsetY = 3f;
    private TextMesh hpTextMesh;

    private static int? minigamePersistedHP = null;

    private float startX;

    public static void ResetMinigameHP()
    {
        minigamePersistedHP = null;
    }

    public static bool IsDead => minigamePersistedHP != null && minigamePersistedHP.Value <= 0;

    void Start()
    {
        // Ghi lại vị trí X ban đầu để tính quãng đường đi được
        startX = transform.position.x;

        // Sprite mặc định đã hướng sang Phải, đảm bảo scale.x là số dương
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        transform.localScale = scale;

        isMinigame = FindObjectOfType<MinigameClickSpawner>() != null;
        if (isMinigame)
        {
            Debug.Log($"[LochNessMonsterMove] Spawned in Minigame. Starting HP: {currentHP}/{maxHP}");

            if (currentHP <= 0)
            {
                Debug.Log("[LochNessMonsterMove] Starting HP is 0 or less. Destroying immediately.");
                Destroy(gameObject);
                return;
            }

            CreateHPText();
        }
    }

    private void CreateHPText()
    {
        GameObject textObj = new GameObject("MinigameHPText");
        textObj.transform.SetParent(transform);
        textObj.transform.localPosition = new Vector3(0, hpTextOffsetY, 0);

        Vector3 parentScale = transform.localScale;
        float sx = parentScale.x != 0 ? 1f / Mathf.Abs(parentScale.x) : 1f;
        float sy = parentScale.y != 0 ? 1f / Mathf.Abs(parentScale.y) : 1f;
        textObj.transform.localScale = new Vector3(sx, sy, 1f);

        hpTextMesh = textObj.AddComponent<TextMesh>();
        hpTextMesh.text = currentHP.ToString();
        hpTextMesh.fontSize = 50;
        hpTextMesh.characterSize = 0.08f;
        hpTextMesh.anchor = TextAnchor.MiddleCenter;
        hpTextMesh.alignment = TextAlignment.Center;
        hpTextMesh.color = Color.yellow;
        hpTextMesh.fontStyle = FontStyle.Bold;

        MeshRenderer meshRenderer = textObj.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            SpriteRenderer parentRenderer = GetComponent<SpriteRenderer>();
            if (parentRenderer == null) parentRenderer = GetComponentInChildren<SpriteRenderer>();

            if (parentRenderer != null)
            {
                meshRenderer.sortingLayerID = parentRenderer.sortingLayerID;
                meshRenderer.sortingOrder = parentRenderer.sortingOrder + 10;
            }
            else
            {
                meshRenderer.sortingOrder = 100;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isMinigame) return;

        currentHP -= damage;
        Debug.Log($"[LochNessMonsterMove] Took {damage} damage. Current HP: {currentHP}/{maxHP}");

        if (currentHP <= 0)
        {
            Debug.Log("[LochNessMonsterMove] Loch Ness Monster defeated!");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (isMinigame)
        {
            if (currentHP <= 0)
            {
                Destroy(gameObject);
                return;
            }

            if (hpTextMesh != null)
            {
                string hpStr = currentHP.ToString();
                if (hpTextMesh.text != hpStr)
                {
                    hpTextMesh.text = hpStr;
                }
            }
        }

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
