using UnityEngine;
using UnityEngine.UI; // Required for legacy UI Text
using System.Collections; // Required for Coroutines

public class UIManager : MonoBehaviour
{
    public static UIManager instance; // Singleton instance

    [Header("UI Panels")]
    public GameObject startMenuPanel;
    public GameObject gameOverPanel;

    [Header("UI Text")]
    public Text scoreText;
    public Text finalScoreText; // Text on the game over screen
    public Text highScoreText; // Text for the high score

    private GameManager gameManager;
    private Coroutine scorePulseCoroutine;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    void Update()
    {
        // Show/hide panels based on game state
        startMenuPanel.SetActive(GameManager.currentState == GameManager.GameState.Menu);
        gameOverPanel.SetActive(GameManager.currentState == GameManager.GameState.GameOver);

        // Only show score text when playing
        scoreText.gameObject.SetActive(GameManager.currentState == GameManager.GameState.Playing);

        if (GameManager.currentState == GameManager.GameState.Playing)
        {
            scoreText.text = "Score: " + GameManager.Score;
        }
        else if (GameManager.currentState == GameManager.GameState.GameOver)
        {
            finalScoreText.text = "Final Score: " + GameManager.Score;
            highScoreText.text = "High Score: " + ScoreManager.instance.GetHighScore();
        }

        // Handle keyboard input for menu and game over screens
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) // Return is the Enter key
        {
            if (GameManager.currentState == GameManager.GameState.Menu)
            {
                OnPlayButton();
            }
            else if (GameManager.currentState == GameManager.GameState.GameOver)
            {
                OnRetryButton();
            }
        }
    }

    public void PulseScore()
    {
        if (scorePulseCoroutine != null)
        {
            StopCoroutine(scorePulseCoroutine);
        }
        scorePulseCoroutine = StartCoroutine(ScorePulseRoutine());
    }

    private IEnumerator ScorePulseRoutine()
    {
        float duration = 0.2f;
        float timer = 0;
        Vector3 originalScale = scoreText.transform.localScale;
        Vector3 targetScale = originalScale * 1.5f;

        // Scale up
        while (timer < duration / 2)
        {
            scoreText.transform.localScale = Vector3.Lerp(originalScale, targetScale, timer / (duration / 2));
            timer += Time.deltaTime;
            yield return null;
        }

        // Scale down
        timer = 0;
        while (timer < duration / 2)
        {
            scoreText.transform.localScale = Vector3.Lerp(targetScale, originalScale, timer / (duration / 2));
            timer += Time.deltaTime;
            yield return null;
        }

        scoreText.transform.localScale = originalScale;
    }

    // --- Button Methods ---
    public void OnPlayButton()
    {
        gameManager.StartGame();
    }

    public void OnRetryButton()
    {
        gameManager.RestartGame();
    }
}
