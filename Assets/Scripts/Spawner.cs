using UnityEngine;
using System.Collections; // Required for Coroutines

public class Spawner : MonoBehaviour
{
    public GameObject linePrefab;
    // These are no longer used, but can be kept for reference or removed.
    // public float minSpawnDelay = 0.8f;
    // public float maxSpawnDelay = 1.2f;

    public static Spawner instance;
    public Color[] lineColors = new Color[4];

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Define colors precisely using hex codes to avoid any ambiguity
        ColorUtility.TryParseHtmlString("#FF0000FF", out lineColors[0]); // Red
        ColorUtility.TryParseHtmlString("#0000FFFF", out lineColors[1]); // Blue
        ColorUtility.TryParseHtmlString("#00FF00FF", out lineColors[2]); // Green
        ColorUtility.TryParseHtmlString("#FFFF00FF", out lineColors[3]); // Yellow
    }

    void Start()
    {
        // Start the spawning loop as soon as the game begins.
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        // This loop will run indefinitely in the background.
        while (true)
        {
            // 1. Wait until the game is in the 'Playing' state AND there is no active line.
            yield return new WaitUntil(() => GameManager.currentState == GameManager.GameState.Playing && !GameManager.isLineActive);

            // 2. Calculate a random delay for the next spawn.
            float delay = GameManager.CurrentSpawnDelay;
            float variance = GameManager.instance.spawnDelayVariance;
            float timeToWait = Random.Range(delay - variance, delay + variance);

            // 3. Wait for that amount of time before spawning.
            yield return new WaitForSeconds(timeToWait);

            // 4. Spawn the line.
            SpawnLine();
        }
    }

    void SpawnLine()
    {
        // A line is now active
        GameManager.isLineActive = true;

        bool spawnMissLine = false;
        float chance = Random.value; // A random value between 0.0 and 1.0

        if (GameManager.Score < GameManager.instance.scoreToReachTargetSpeed)
        {
            // Stage 1
            if (chance < GameManager.instance.stage1MissLineChance)
            {
                spawnMissLine = true;
            }
        }
        else
        {
            // Stage 2
            if (chance < GameManager.instance.stage2MissLineChance)
            {
                spawnMissLine = true;
            }
        }

        // 1. Randomly pick a spawn point
        int spawnPointIndex = Random.Range(0, 4);
        Vector3 spawnPosition = Vector3.zero;

        switch (spawnPointIndex)
        {
            case 0: // Top
                spawnPosition = new Vector3(0, 6, 0);
                break;
            case 1: // Bottom
                spawnPosition = new Vector3(0, -6, 0);
                break;
            case 2: // Right
                spawnPosition = new Vector3(10, 0, 0);
                break;
            case 3: // Left
                spawnPosition = new Vector3(-10, 0, 0);
                break;
        }

        // 2. Randomly pick a color, independent of the spawn point
        Color spawnColor = lineColors[Random.Range(0, lineColors.Length)];

        GameObject line = Instantiate(linePrefab, spawnPosition, Quaternion.identity);
        Line lineScript = line.GetComponent<Line>();
        SpriteRenderer renderer = line.GetComponent<SpriteRenderer>();
        renderer.color = spawnColor;

        if (spawnMissLine)
        {
            lineScript.isMissLine = true;

            // --- Create the "hollow" effect in code ---
            // 1. Create a new GameObject for the inner part.
            GameObject inner = new GameObject("Inner");
            inner.transform.SetParent(line.transform); // Make it a child of the line.
            inner.transform.localPosition = new Vector3(0, 0, -0.1f); // Place it slightly in front.

            // 2. Add a SpriteRenderer and use the same sprite.
            SpriteRenderer innerRenderer = inner.AddComponent<SpriteRenderer>();
            innerRenderer.sprite = renderer.sprite;
            innerRenderer.color = Color.black; // The inner part is black to "cut out" the center.
            innerRenderer.sortingOrder = renderer.sortingOrder + 1; // Ensure it draws on top.

            // 3. Scale it down to create the border.
            float borderSize = 0.3f; // Adjust this value to change the border thickness.
            inner.transform.localScale = new Vector3(1 - borderSize, 1 - borderSize, 1);
        }

        // Rotate the line if it's a side spawn
        if (spawnPointIndex > 1) // 2=R, 3=L
        {
            line.transform.rotation = Quaternion.Euler(0, 0, 90);
        }

        line.tag = "Line"; // Assign tag for collision detection
    }
}
