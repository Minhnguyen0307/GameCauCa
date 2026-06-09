using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public float timeRemaining = 120f;
    public bool timerIsRunning = false;

    [Header("UI Reference")]
    public TMP_Text timerText;
    public GameObject timeUpPanel;
    public TMP_Text finalScoreText;

    private void Start()
    {
        timerIsRunning = true;
        
        if (timeUpPanel != null)
        {
            timeUpPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (timerIsRunning && Time.timeScale > 0f)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.unscaledDeltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
                OnTimeOut();
            }
        }
    }

    private void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        if (timerText != null)
        {
            timerText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
        }
    }

    private void OnTimeOut()
    {
        Time.timeScale = 0f;

        if (timeUpPanel != null)
        {
            timeUpPanel.SetActive(true);
        }

        if (finalScoreText != null && ScoreManager.Instance != null)
        {
            finalScoreText.text = "Final Score: " + ScoreManager.Instance.score;
        }
        else if (finalScoreText != null)
        {
            finalScoreText.text = "Time's Up!";
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.score = 0;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); 
    }
}