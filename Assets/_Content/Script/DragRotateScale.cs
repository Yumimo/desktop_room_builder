using Unity.AI.Navigation;
using UnityEngine;

public class DragOrScaleFromBounds : MonoBehaviour
{
    public NavMeshSurface navMeshSurface;
    private Vector3 screenPoint;
    private Vector3 offset;

    private bool isDragging = false;
    private bool isScaling = false;

    private float initialMouseDistance;
    private Vector3 initialScale;

    [Header("Settings")]
    public float edgeThreshold = 0.2f;     // % of bounds from edge considered 'scaling zone'
    public float minScale = 0.1f;

    private Camera cam;
    private Bounds bounds;

    void Start()
    {
        cam = Camera.main;
    }

    void OnMouseDown()
    {
        if (cam == null) return;

        screenPoint = cam.WorldToScreenPoint(transform.position);
        Vector3 mouseWorld = cam.ScreenToWorldPoint(GetMouseWorldPoint(screenPoint.z));
        offset = transform.position - mouseWorld;

        bounds = CalculateBoundsFromBoxCollider();

        if (IsNearEdge(mouseWorld)) // ➤ Edge = SCALE
        {
            isScaling = true;
            initialMouseDistance = Vector3.Distance(mouseWorld, transform.position);
            initialScale = transform.localScale;
        }
        else // ➤ Center = MOVE
        {
            isDragging = true;
        }
    }

    void OnMouseDrag()
    {
        if (cam == null) return;

        Vector3 mouseWorld = cam.ScreenToWorldPoint(GetMouseWorldPoint(screenPoint.z));

        if (isDragging)
        {
            transform.position = mouseWorld + offset;
        }
        else if (isScaling)
        {
            float currentDistance = Vector3.Distance(mouseWorld, transform.position);
            float scaleRatio = currentDistance / initialMouseDistance;

            Vector3 newScale = initialScale * scaleRatio;
            newScale = Vector3.Max(newScale, Vector3.one * minScale);

            transform.localScale = newScale;
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
        isScaling = false;
        navMeshSurface.BuildNavMesh();
    }

    private Vector3 GetMouseWorldPoint(float zDepth)
    {
        return new Vector3(Input.mousePosition.x, Input.mousePosition.y, zDepth);
    }

    private Bounds CalculateBoundsFromBoxCollider()
    {
        BoxCollider col = GetComponent<BoxCollider>();
        if (col == null)
        {
            Debug.LogWarning("BoxCollider not found on object: " + gameObject.name);
            return new Bounds(transform.position, Vector3.one);
        }
        return col.bounds;
    }

    /// <summary>
    /// Determines if the mouse is near the edge of the object bounds.
    /// </summary>
    private bool IsNearEdge(Vector3 pointWorld)
    {
        Vector3 localPoint = transform.InverseTransformPoint(pointWorld);
        Vector3 extents = transform.InverseTransformVector(bounds.extents);

        float thresholdX = Mathf.Abs(extents.x) * (1f - edgeThreshold);
        float thresholdY = Mathf.Abs(extents.y) * (1f - edgeThreshold);

        bool nearXEdge = Mathf.Abs(localPoint.x) > thresholdX;
        bool nearYEdge = Mathf.Abs(localPoint.y) > thresholdY;

        return nearXEdge || nearYEdge;
    }
}
