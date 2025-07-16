using UnityEngine;

public class ObjectInFrontOfCamera : MonoBehaviour
{
    public GameObject objectTarget;

    [Tooltip("Which screen corner or edge to place the object on")]
    public ScreenAnchor anchor = ScreenAnchor.TopRight;

    [Tooltip("Distance from camera to place the object in world units")]
    public float distanceFromCamera = 10f;

    [Tooltip("Screen-space offset in pixels (X, Y)")]
    public Vector2 screenOffset = Vector2.zero;

    [Tooltip("World-space offset after placement (optional)")]
    public Vector3 worldOffset = Vector3.zero;

    public enum ScreenAnchor
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        Left,
        Right,
        Top,
        Bottom,
        Center
    }

    void Start()
    {
        if (objectTarget != null)
        {
            Camera cam = GetComponent<Camera>();
            if (cam == null) return;

            // 1. Get screen position from edge + offset
            Vector3 screenPos = GetScreenEdgePosition(anchor);
            screenPos.x += screenOffset.x;
            screenPos.y += screenOffset.y;
            screenPos.z = distanceFromCamera;

            // 2. Convert to world and apply world offset
            Vector3 worldPos = cam.ScreenToWorldPoint(screenPos);
            objectTarget.transform.position = worldPos + worldOffset;
        }
    }

    Vector3 GetScreenEdgePosition(ScreenAnchor anchor)
    {
        float x = 0;
        float y = 0;

        switch (anchor)
        {
            case ScreenAnchor.TopLeft:
                x = 0;
                y = Screen.height;
                break;
            case ScreenAnchor.TopRight:
                x = Screen.width;
                y = Screen.height;
                break;
            case ScreenAnchor.BottomLeft:
                x = 0;
                y = 0;
                break;
            case ScreenAnchor.BottomRight:
                x = Screen.width;
                y = 0;
                break;
            case ScreenAnchor.Left:
                x = 0;
                y = Screen.height / 2;
                break;
            case ScreenAnchor.Right:
                x = Screen.width;
                y = Screen.height / 2;
                break;
            case ScreenAnchor.Top:
                x = Screen.width / 2;
                y = Screen.height;
                break;
            case ScreenAnchor.Bottom:
                x = Screen.width / 2;
                y = 0;
                break;
            case ScreenAnchor.Center:
                x = Screen.width / 2;
                y = Screen.height / 2;
                break;
        }

        return new Vector3(x, y, 0);
    }
}
