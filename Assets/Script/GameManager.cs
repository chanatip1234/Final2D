using UnityEngine;
using TMPro; 

public class GameManager : MonoBehaviour
{
    public static GameManager instance; 

    public TextMeshProUGUI scoreText; 
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
        scoreText.text = "Score: " + currentScore.ToString();
    }
}
