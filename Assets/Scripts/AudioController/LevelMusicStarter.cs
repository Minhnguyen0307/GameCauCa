using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Gắn script này vào một GameObject trong scene level để tự động phát nhạc nền phù hợp.
/// Script tự nhận biết tên scene và phát đúng track nhạc.
/// </summary>
public class LevelMusicStarter : MonoBehaviour
{
    void Start()
    {
        if (AudioController.Instance == null) return;

        string sceneName = SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
            case "Level_BinhThuong":
                AudioController.Instance.PlayMusic(SoundType.LevelNormal);
                break;

            case "Level_CucDe":
            case "Level_De":
                AudioController.Instance.PlayMusic(SoundType.LevelEasy);
                break;

            case "Level_Kho":
                AudioController.Instance.PlayMusic(SoundType.LevelHard);
                break;

            case "Level_CucKho":
                AudioController.Instance.PlayMusic(SoundType.LevelExtreme);
                break;

            case "MainMenu":
                AudioController.Instance.PlayMusic(SoundType.MainMenu);
                break;

            // Thêm các level khác ở đây nếu cần
            default:
                Debug.Log($"[LevelMusicStarter] Không có nhạc được cấu hình cho scene: {sceneName}");
                break;
        }
    }
}
