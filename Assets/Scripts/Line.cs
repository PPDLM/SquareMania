using UnityEngine;

public class Line : MonoBehaviour
{
    public Color lineColor;
    public bool isMissLine = false; // This will be set by the Spawner

    void Start()
    {
        // Get the color from the SpriteRenderer
        lineColor = GetComponent<SpriteRenderer>().color;
    }

    void Update()
    {
        if (GameManager.currentState != GameManager.GameState.Playing) return;

        // Move towards the center
        transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, GameManager.CurrentSpeed * Time.deltaTime);

        // Check for pass-through condition
        if (Vector3.Distance(transform.position, Vector3.zero) < 0.1f)
        {
            if (isMissLine)
            {
                // A Miss-Line reached the center. Now check if the cube color was correct.
                CheckMissLineColor();
            }
            else
            {
                // A normal line passed through the cube, which is a failure.
                GameManager.isLineActive = false;
                GameManager.instance.GameOver();
            }
            Destroy(gameObject);
        }
    }

    public void CheckHit(Color facingSideColor)
    {
        if (isMissLine)
        {
            // FAILURE: Player hit space on a Miss-Line.
            Debug.LogError("HIT FAILED. Player pressed space on a Miss-Line.");
            GameManager.isLineActive = false;
            GameManager.instance.GameOver();
            Destroy(gameObject);
            return;
        }

        // --- Existing logic for normal lines ---
        Color myColor = GetComponent<SpriteRenderer>().color;
        Vector3 lineRgb = new Vector3(myColor.r, myColor.g, myColor.b);
        Vector3 sideRgb = new Vector3(facingSideColor.r, facingSideColor.g, facingSideColor.b);

        if (Vector3.Distance(lineRgb, sideRgb) < 0.1f)
        {
            // Success
            GameManager.instance.IncrementScore();
            GameManager.isLineActive = false;
            Destroy(gameObject);
        }
        else
        {
            // Failure - color mismatch
            Debug.LogError($"HIT FAILED. Line Color: {myColor} | Side Color: {facingSideColor}");
            GameManager.isLineActive = false;
            GameManager.instance.GameOver();
            Destroy(gameObject);
        }
    }

    private void CheckMissLineColor()
    {
        // This is called only when a Miss-Line reaches the center.
        CubeController cube = FindFirstObjectByType<CubeController>();
        if (cube == null)
        {
            GameManager.instance.GameOver();
            return;
        }

        Color facingSideColor = cube.GetFacingSideColor(transform.position);
        Color myColor = GetComponent<SpriteRenderer>().color;

        Vector3 lineRgb = new Vector3(myColor.r, myColor.g, myColor.b);
        Vector3 sideRgb = new Vector3(facingSideColor.r, facingSideColor.g, facingSideColor.b);

        if (Vector3.Distance(lineRgb, sideRgb) < 0.1f)
        {
            // Correctly let the Miss-Line pass. No score, but no game over.
            Debug.Log("Miss-Line cleared successfully!");
            GameManager.instance.IncrementScore();
            GameManager.isLineActive = false;
        }
        else
        {
            // FAILURE: Cube was not rotated to the correct color for the Miss-Line.
            Debug.LogError($"MISS-LINE FAILED. Line Color: {myColor} | Side Color: {facingSideColor}");
            GameManager.isLineActive = false;
            GameManager.instance.GameOver();
        }
    }
}