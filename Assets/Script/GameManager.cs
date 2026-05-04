using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI References")]
    public TextMeshProUGUI scoreText;      
    public GameObject gameOverPanel;       
    public TextMeshProUGUI finalScoreText;

    [Header("Credit UI")]
    public GameObject creditPanel;       
    public TextMeshProUGUI finalScoreCreditText;

    private int currentScore = 0;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null) scoreText.text = currentScore.ToString();
    }

    public void ShowGameOver()
    {
        Time.timeScale = 0f;

        gameOverPanel.SetActive(true);
        if (finalScoreText != null)
        {
            finalScoreText.text = "Your Score: " + currentScore.ToString();
        }
    }

    public void ShowCredits()
    {
        Time.timeScale = 0f; 
        creditPanel.SetActive(true);
        if (finalScoreCreditText != null)
        {
            finalScoreCreditText.text = "Total Score: " + currentScore.ToString();
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit(); 
    }
}
