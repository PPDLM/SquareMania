using UnityEngine;

public class CubeController : MonoBehaviour
{
    public float rotationSpeed = 10f;
    [Range(0.1f, 2f)] public float cubeScale = 1.0f; // Controla la escala desde el Inspector
    public Transform[] sides; // Asigna los 4 lados en el Inspector
    public Vector2 baseHitboxSize = new Vector2(3.0f, 3.0f); // El tamaño base de la hitbox

    // La hitbox ahora se calcula dinámicamente
    private Vector2 CurrentHitboxSize => baseHitboxSize * cubeScale;

    private static Quaternion targetRotation = Quaternion.identity;

    void OnValidate()
    {
        // This function is called in the editor when a script is loaded or a value is changed in the Inspector.
        // It allows us to see the size change without entering Play mode.
        transform.localScale = new Vector3(cubeScale, cubeScale, 1f);
    }

    void Start()
    {
        transform.localScale = new Vector3(cubeScale, cubeScale, 1f); // Also set scale on game start
        transform.rotation = targetRotation; // Instantly set rotation on scene load

        // Add and configure a Rigidbody2D for reliable physics queries
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

        // Programmatically set side colors from the single source of truth in Spawner
        // This guarantees the colors will match for hit detection.
        for (int i = 0; i < sides.Length; i++)
        {
            if (sides[i] != null && Spawner.instance.lineColors.Length > i)
            {
                sides[i].GetComponent<SpriteRenderer>().color = Spawner.instance.lineColors[i];
            }
        }
    }

    void Update()
    {
        if (GameManager.currentState != GameManager.GameState.Playing) return;

        HandleRotationInput();
        SmoothlyRotate();
        HandleHitInput();
    }

    private void HandleRotationInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            targetRotation *= Quaternion.Euler(0, 0, 90);
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            targetRotation *= Quaternion.Euler(0, 0, -90);
        }
    }

    private void SmoothlyRotate()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void HandleHitInput()
    {
        // 1. Check if the space key is pressed at all.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 2. Log the game state at the exact moment of the press.
            Debug.Log("Spacebar pressed. Current Game State: " + GameManager.currentState);

            // 3. If we are not in the 'Playing' state, do nothing. This is a safety check.
            if (GameManager.currentState != GameManager.GameState.Playing)
            {
                Debug.Log("...but game is not in 'Playing' state. Ignoring hit.");
                return;
            }

            // 4. Perform the physics check.
            Vector2 checkPosition = transform.position;
            Collider2D lineCollider = Physics2D.OverlapBox(checkPosition, CurrentHitboxSize, 0f);

            if (lineCollider != null && lineCollider.CompareTag("Line"))
            {
                // 5. We found a line!
                Debug.Log("SUCCESS: OverlapBox found a Line. Checking color match.");
                CheckHit(lineCollider.GetComponent<Line>());
            }
            else
            {
                // 6. We either found nothing, or it wasn't a line.
                Debug.Log("FAILURE: OverlapBox did not find a Line. Calling GameOver.");
                FindFirstObjectByType<GameManager>().GameOver();
            }
        }
    }

    private void CheckHit(Line currentLine)
    {
        if (currentLine == null) return;

        float minAngle = float.MaxValue;
        Transform facingSide = null;

        // Determine which side is "facing" the line
        Vector3 dirToLine = (currentLine.transform.position - transform.position).normalized;

        foreach (Transform side in sides)
        {
            // The original 'side.up' was unreliable.
            // This new logic calculates the true direction from the pivot's center to the side's center.
            Vector3 dirToSide = (side.position - transform.position).normalized;
            float angle = Vector3.Angle(dirToSide, dirToLine);

            if (angle < minAngle)
            {
                minAngle = angle;
                facingSide = side;
            }
        }

        if (facingSide != null)
        {
            // Add a debug log to be 100% certain which side is being chosen.
            Debug.Log($"Facing side chosen: {facingSide.name} with angle {minAngle}");
            Color sideColor = facingSide.GetComponent<SpriteRenderer>().color;
            currentLine.CheckHit(sideColor);
        }
    }

    public Color GetFacingSideColor(Vector3 linePosition)
    {
        float minAngle = float.MaxValue;
        Transform facingSide = null;
        Vector3 dirToLine = (linePosition - transform.position).normalized;

        foreach (Transform side in sides)
        {
            Vector3 dirToSide = (side.position - transform.position).normalized;
            float angle = Vector3.Angle(dirToSide, dirToLine);

            if (angle < minAngle)
            {
                minAngle = angle;
                facingSide = side;
            }
        }

        if (facingSide != null)
        {
            return facingSide.GetComponent<SpriteRenderer>().color;
        }

        return Color.clear; // Return a transparent color if no side is found
    }

    // Draw a visual representation of the hitbox in the Scene view for easy debugging
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f); // Verde, semi-transparente
        Gizmos.DrawCube(transform.position, CurrentHitboxSize);
    }
}