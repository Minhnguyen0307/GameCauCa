using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int score = 0;
    public TMP_Text scoreText;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            if (scoreText != null)
            {
                Instance.scoreText = this.scoreText;
                Instance.UpdateUI();
            }
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("AddScore: +" + amount + " | Total = " + score);
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }
}