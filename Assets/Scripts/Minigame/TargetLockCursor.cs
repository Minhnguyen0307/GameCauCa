using UnityEngine;

public class TargetLockCursor : MonoBehaviour
{
    public Texture2D targetLockTexture;

    [Header("Software Cursor Settings")]
    public bool useSoftwareCursor = true;
    public float cursorWidth = 64f;
    public float cursorHeight = 64f;
    public float offsetX = 0f;
    public float offsetY = 0f;

    void OnEnable()
    {
        if (!useSoftwareCursor && targetLockTexture != null)
        {
            // Điểm click nằm ở chính giữa của tâm ngắm (targetlock)
            Vector2 hotspot = new Vector2(
                targetLockTexture.width / 2f,
                targetLockTexture.height / 2f
            );
            Cursor.SetCursor(targetLockTexture, hotspot, CursorMode.Auto);
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
            if (Cursor.visible)
            {
                Cursor.visible = false;
            }
        }
    }

    void OnGUI()
    {
        if (useSoftwareCursor && targetLockTexture != null)
        {
            // Lấy vị trí chuột trong không gian GUI
            Vector2 mousePos = Event.current.mousePosition;

            // Tính toán Rect vẽ tâm ngắm sao cho hotspot nằm chính giữa
            Rect rect = new Rect(
                mousePos.x - cursorWidth / 2f + offsetX,
                mousePos.y - cursorHeight / 2f + offsetY,
                cursorWidth,
                cursorHeight
            );

            GUI.DrawTexture(rect, targetLockTexture);
        }
    }

    void OnDisable()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Cursor.visible = true;
    }

    void OnDestroy()
    {
        Cursor.visible = true;
    }
}
