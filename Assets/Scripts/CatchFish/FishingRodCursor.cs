using UnityEngine;

public class FishingRodCursor : MonoBehaviour
{
    public Texture2D fishingRodCursor;

    [Header("Software Cursor Settings")]
    public bool useSoftwareCursor = true;
    public float cursorWidth = 80f; // Tăng kích thước chiều rộng (ví dụ 80 pixel)
    public float cursorHeight = 100f; // Tăng kích thước chiều cao (ví dụ 100 pixel)
    public float offsetX = 0f;
    public float offsetY = 40f; // Dịch cần câu xuống dưới 40 pixel để khớp với vị trí con chuột

    void OnEnable()
    {
        if (!useSoftwareCursor && fishingRodCursor != null)
        {
            // Điểm click nằm ở đầu móc câu cho phần cứng
            Vector2 hotspot = new Vector2(
                fishingRodCursor.width / 2f,
                fishingRodCursor.height
            );
            Cursor.SetCursor(fishingRodCursor, hotspot, CursorMode.Auto);
            Cursor.visible = true;
        }
        else if (useSoftwareCursor)
        {
            Cursor.visible = false;
        }
    }

    void Update()
    {
        if (useSoftwareCursor)
        {
            // Luôn đảm bảo ẩn con trỏ chuột hệ thống trong khi chơi
            if (Cursor.visible)
            {
                Cursor.visible = false;
            }
        }
    }

    void OnGUI()
    {
        if (useSoftwareCursor && fishingRodCursor != null)
        {
            // Lấy vị trí chuột trong không gian GUI
            Vector2 mousePos = Event.current.mousePosition;

            // Tính toán Rect vẽ cần câu sao cho điểm hotspot nằm ở đầu móc câu (dưới cùng, chính giữa + offset)
            Rect rect = new Rect(
                mousePos.x - cursorWidth / 2f + offsetX,
                mousePos.y - cursorHeight + offsetY,
                cursorWidth,
                cursorHeight
            );

            GUI.DrawTexture(rect, fishingRodCursor);
        }
    }

    void OnDisable()
    {
        // Trả cursor về mặc định khi tắt script hoặc chuyển scene
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Cursor.visible = true;
    }

    void OnDestroy()
    {
        Cursor.visible = true;
    }
}