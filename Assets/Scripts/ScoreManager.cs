using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    private const string HighScoreKey = "HighScore";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveHighScore(int score)
    {
        int currentHighScore = GetHighScore();
        if (score > currentHighScore)
        {
            PlayerPrefs.SetInt(HighScoreKey, score);
            PlayerPrefs.Save();
        }
    }

    public int GetHighScore()
    {
        return PlayerPrefs.GetInt(HighScoreKey, 0);
    }
}
