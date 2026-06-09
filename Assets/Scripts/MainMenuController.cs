using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1f;

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.score = 0;
        }

        if (AudioController.Instance != null)
        {
            AudioController.Instance.PlayMusic(SoundType.MainMenu);
        }
    }

    public void PlayEasyMode()
    {
        SceneManager.LoadScene("Level_De");
    }

    public void PlayNormalMode()
    {
        SceneManager.LoadScene("Level_BinhThuong");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }
}