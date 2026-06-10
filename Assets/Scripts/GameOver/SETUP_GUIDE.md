# Hướng dẫn Setup Game Over UI trong Unity

## Bước 1 – Tạo GameOverPanel UI

Trong Hierarchy của Scene, tạo cấu trúc:

```
Canvas
└── GameOverPanel          (UI Panel, ẩn mặc định)
    ├── Background         (Image - màu tối, alpha ~180)
    ├── TitleText          (TMP_Text) → "GAME OVER"
    ├── ScoreText          (TMP_Text) → "Score: 0"
    ├── RetryButton        (Button + TMP_Text "Retry")
    └── MainMenuButton     (Button + TMP_Text "Main Menu")
```

## Bước 2 – Tạo GameObject GameOverManager

1. Tạo Empty GameObject trong Scene, đặt tên `GameOverManager`
2. Attach script `GameOverManager`
3. Gán vào Inspector:
   - **Game Over Panel** → kéo `GameOverPanel` vào
   - **Final Score Text** → kéo `ScoreText` vào

## Bước 3 – Gán Button Events

- **RetryButton → OnClick()** → kéo `GameOverManager` → chọn `GameOverManager.OnRetryClicked`
- **MainMenuButton → OnClick()** → kéo `GameOverManager` → chọn `GameOverManager.OnMainMenuClicked`

## Bước 4 – Gán KrakenClickHandler vào Prefab Kraken

1. Mở Prefab Kraken
2. Attach script `KrakenClickHandler` vào root GameObject của Kraken
3. Đảm bảo Kraken có **Collider2D** (để `OnMouseDown` hoạt động)

## Lưu ý

- `GameOverPanel` phải **tắt (SetActive false)** khi bắt đầu — script tự xử lý điều này
- Camera cần **Physics Raycaster 2D** để `OnMouseDown` nhận input đúng trên 2D objects
- Nếu dùng New Input System, thay `OnMouseDown` bằng `IPointerClickHandler` (xem thêm nếu cần)
