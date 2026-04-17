using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState { Menu, Playing, GameOver }
    public static GameState currentState;

    // --- Difficulty Parameters ---
    [Header("Difficulty Settings")]
    public float initialSpeed = 2.5f;
    public float targetSpeed = 5.0f; // The speed that marks the end of the initial ramp-up
    public float maxSpeed = 6.0f;    // The near-impossible speed
    public int scoreToReachTargetSpeed = 15; // Hits required to reach targetSpeed
    [Tooltip("Controls how slowly speed increases after reaching the target. Smaller is slower.")]
    public float stage2RampUpFactor = 0.05f;

    [Header("Miss-Line Settings")]
    [Range(0f, 1f)]
    public float stage1MissLineChance = 0.1f; // 10% chance in stage 1
    [Range(0f, 1f)]
    public float stage2MissLineChance = 0.25f; // 25% chance in stage 2

    public float initialSpawnDelay = 1.5f;
    public float minSpawnDelay = 0.8f;
    [Tooltip("Adds randomness to spawn delay. 0.1 means +/- 0.1 seconds.")]
    public float spawnDelayVariance = 0.1f;

    // Static variables for global access
    public static float CurrentSpeed { get; private set; }
    public static float CurrentSpawnDelay { get; private set; }
    public static int Score = 0;
    public static bool isLineActive = false; // Tracks if a line is currently on-screen

    public static GameManager instance; // Singleton instance
    
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

    void Update()
    {
        // This no longer needs to handle difficulty updates
    }

    void Start()
    {
        // The game starts in the menu
        currentState = GameState.Menu;
    }

    public void StartGame()
    {
        // Reset variables and start the game
        currentState = GameState.Playing;
        isLineActive = false;
        Score = 0;
        UpdateDifficulty(); // Set initial speed and spawn delay
    }

    // Method to handle game over
    public void GameOver()
    {
        currentState = GameState.GameOver;
        ScoreManager.instance.SaveHighScore(Score);
    }

    public void RestartGame()
    {
        // When restarting, we need to clean up the persistent singletons
        // so they can be re-initialized in the new scene.
        if (UIManager.instance != null)
        {
            Destroy(UIManager.instance.gameObject);
            UIManager.instance = null;
        }
        if (GameManager.instance != null)
        {
            Destroy(GameManager.instance.gameObject);
            GameManager.instance = null;
        }

        // Reload the scene to go back to the menu
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void IncrementScore()
    {
        Score++;
        UpdateDifficulty();

        // Trigger the UI animation
        if (UIManager.instance != null)
        {
            UIManager.instance.PulseScore();
        }
    }

    private void UpdateDifficulty()
    {
        if (Score < scoreToReachTargetSpeed)
        {
            // --- Stage 1: Linearly ramp up to target speed ---
            // This provides a steady, predictable increase for the first 15 hits.
            float progress = (float)Score / scoreToReachTargetSpeed;
            CurrentSpeed = Mathf.Lerp(initialSpeed, targetSpeed, progress);
            CurrentSpawnDelay = Mathf.Lerp(initialSpawnDelay, minSpawnDelay, progress); // Spawn delay also scales
        }
        else
        {
            // --- Stage 2: Slowly crawl from target speed to max speed ---
            // After 15 hits, the curve becomes exponential and much slower.
            int scorePastTarget = Score - scoreToReachTargetSpeed;
            float progress = 1 - Mathf.Exp(-scorePastTarget * stage2RampUpFactor);
            CurrentSpeed = Mathf.Lerp(targetSpeed, maxSpeed, progress);
            // Spawn delay continues to decrease, but much more slowly.
            float targetSpawnDelay = Mathf.Lerp(initialSpawnDelay, minSpawnDelay, 1f); // This is minSpawnDelay
            CurrentSpawnDelay = Mathf.Lerp(targetSpawnDelay, minSpawnDelay, progress);
        }

        Debug.Log($"Score: {Score}, Speed: {CurrentSpeed:F2}, Spawn Delay: {CurrentSpawnDelay:F2}");
    }
}