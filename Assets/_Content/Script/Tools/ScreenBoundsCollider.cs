using UnityEngine;

public class ScreenBoundsCollider : MonoBehaviour
{
    [Header("Wall Settings")]
    public float wallThickness = 1f;
    public float wallHeight = 3f;
    public float extraPadding = 2f; // additional size for freedom to move

    [Header("Visual Debug (Optional)")]
    public Material debugMaterial; // Optional: assign a material to see the walls

    private void Start()
    {
        CreateBoundaries();
    }

    private void CreateBoundaries()
    {
        Camera cam = Camera.main;

        // We'll assume your cat is at world Z = 0
        float zDepth = 0f;

        // Get screen bounds projected to z = 0 world space
        float distanceFromCamera = Mathf.Abs(cam.transform.position.z - zDepth);

        Vector3 bottomLeft = cam.ScreenToWorldPoint(new Vector3(0, 0, distanceFromCamera));
        Vector3 topRight = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, distanceFromCamera));

        float width = (topRight.x - bottomLeft.x) + extraPadding;
        float height = (topRight.y - bottomLeft.y) + extraPadding;

        Vector3 center = new Vector3((bottomLeft.x + topRight.x) / 2f, (bottomLeft.y + topRight.y) / 2f, zDepth);

        // FLOOR
        CreateWall(
            position: new Vector3(center.x, bottomLeft.y - wallThickness / 2f - extraPadding / 2f, zDepth),
            scale: new Vector3(width + wallThickness * 2, wallThickness, wallThickness)
        );

        // CEILING
        CreateWall(
            position: new Vector3(center.x, topRight.y + wallThickness / 2f + extraPadding / 2f, zDepth),
            scale: new Vector3(width + wallThickness * 2, wallThickness, wallThickness)
        );

        // LEFT WALL
        CreateWall(
            position: new Vector3(bottomLeft.x - wallThickness / 2f - extraPadding / 2f, center.y, zDepth),
            scale: new Vector3(wallThickness, height + wallThickness * 2, wallThickness)
        );

        // RIGHT WALL
        CreateWall(
            position: new Vector3(topRight.x + wallThickness / 2f + extraPadding / 2f, center.y, zDepth),
            scale: new Vector3(wallThickness, height + wallThickness * 2, wallThickness)
        );
    }

    private void CreateWall(Vector3 position, Vector3 scale)
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.transform.position = position;
        wall.transform.localScale = scale;
        wall.name = "Screen Boundary Wall";

        // Optional: Assign debug material or hide mesh
        if (debugMaterial != null)
        {
            wall.GetComponent<Renderer>().material = debugMaterial;
        }
        else
        {
            wall.GetComponent<Renderer>().enabled = false;
        }

        // Ensure collidable static wall
        Collider collider = wall.GetComponent<Collider>();
        if (collider != null)
            collider.isTrigger = false;

        // Remove Rigidbody if created by default
        Destroy(wall.GetComponent<Rigidbody>());

        // Make it static
        wall.isStatic = true;
    }
}
