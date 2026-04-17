using UnityEngine;

[RequireComponent(typeof(Camera))]
public class AspectRatioController : MonoBehaviour
{
    private Camera cam;
    private float lastWidth = 0f;
    private float lastHeight = 0f;

    void Start()
    {
        cam = GetComponent<Camera>();
        UpdateAspectRatio();
    }

    void Update()
    {
        // Only update if the screen dimensions have changed
        if (Screen.width != lastWidth || Screen.height != lastHeight)
        {
            UpdateAspectRatio();
        }
    }

    void UpdateAspectRatio()
    {
        lastWidth = Screen.width;
        lastHeight = Screen.height;

        float targetAspect = 1.0f; // We want a square aspect ratio
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;
        Rect rect = new Rect(0, 0, 1, 1);

        if (scaleHeight < 1.0f) // Window is taller than it is wide (portrait)
        {
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
        }
        else // Window is wider than it is tall (landscape)
        {
            float scaleWidth = 1.0f / scaleHeight;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
        }

        cam.rect = rect;
    }
}