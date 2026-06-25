using UnityEngine;

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

    public void PlayVeryEasyMode()
    {
        SceneTransition.Instance.LoadScene("Level_CucDe");
    }

    public void PlayEasyMode()
    {
        SceneTransition.Instance.LoadScene("Level_De");
    }

    public void PlayNormalMode()
    {
        SceneTransition.Instance.LoadScene("Level_BinhThuong");
    }

    public void PlayHardMode()
    {
        SceneTransition.Instance.LoadScene("Level_Kho");
    }

    public void PlayVeryHardMode()
    {
        SceneTransition.Instance.LoadScene("Level_CucKho");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }
}