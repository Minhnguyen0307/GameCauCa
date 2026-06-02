using UnityEngine;

public class FishingRodCursor : MonoBehaviour
{
    public Texture2D fishingRodCursor;

    void Start()
    {
        // Điểm click nằm ở đầu móc câu
        Vector2 hotspot = new Vector2(
            fishingRodCursor.width / 2,
            fishingRodCursor.height
        );

        Cursor.SetCursor(fishingRodCursor, hotspot, CursorMode.Auto);
    }

    void OnDisable()
    {
        // Trả cursor về mặc định khi tắt
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}