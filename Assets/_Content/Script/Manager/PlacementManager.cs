using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public static PlacementManager Instance;
    
    [Header("Furniture")]
    public FurnitureSO currentFurnitureData;

    [Header("Grid")]
    public Grid grid;

    [Header("Layers")]
    public LayerMask groundLayer;
    public LayerMask placementLayer;

    [Header("Materials")]
    public Material ghostValidMaterial;
    public Material ghostInvalidMaterial;

    [Header("Placement Settings")]
    public float yOffset = 0.0f;

    private Camera mainCam;
    private GameObject ghostObject;
    private Vector2Int rotatedSize;
    private Quaternion currentRotation = Quaternion.identity;
    private Vector3Int currentCell;
    private bool isValidPlacement = false;
    private bool isDisable;
    
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        mainCam = Camera.main;
    }

    public void SetupFurniture(FurnitureSO furniture)
    {
        currentFurnitureData = furniture;
        if (ghostObject != null)
        {
            Destroy(ghostObject);
        }
        rotatedSize = furniture.size;
        SpawnGhost();
    }

    public void TogglePlacement(bool _toggle)
    {
        isDisable = _toggle;
        if (isDisable)
        {
            currentFurnitureData = null;
            if (ghostObject != null)
            {
                Destroy(ghostObject);
            }
        }
    }

    void Update()
    {
        if(isDisable) return;
        
        if(currentFurnitureData == null) return;
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
        {
            var room = FindAnyObjectByType<RoomController>();
            grid.cellSize = room.RoomScale;
            if (Input.GetKeyDown(KeyCode.R))
            {
                RotateGhost();
            }

            currentCell = grid.WorldToCell(hit.point);
            Vector3 anchorPosition = grid.GetCellCenterWorld(currentCell) + Vector3.up * yOffset;

            ghostObject.transform.position = anchorPosition;
            ghostObject.transform.rotation = currentRotation;
            ghostObject.transform.localScale = room.RoomScale;
            ghostObject.SetActive(true);

            isValidPlacement = CheckMultiCellOccupied(currentCell, rotatedSize);
            ApplyGhostMaterial(ghostObject, isValidPlacement ? ghostValidMaterial : ghostInvalidMaterial);

            if (Input.GetMouseButtonDown(0) && isValidPlacement)
            {
                GameObject placed = Instantiate(currentFurnitureData.prefab, anchorPosition, currentRotation);

                if (room != null)
                    placed.transform.SetParent(room.transform);

                placed.transform.position = anchorPosition;
                placed.transform.rotation = currentRotation;
                placed.transform.localScale = Vector3.one;
            }
        }
        else
        {
            ghostObject.SetActive(false);
        }
    }

    void RotateGhost()
    {
        currentRotation *= Quaternion.Euler(0f, 90f, 0f);
        rotatedSize = new Vector2Int(rotatedSize.y, rotatedSize.x);
        ghostObject.transform.rotation = currentRotation;
    }

    void SpawnGhost()
    {
        if (ghostObject != null)
            Destroy(ghostObject);

        ghostObject = Instantiate(currentFurnitureData.prefab);
        ghostObject.name = "GhostFurniture";

        foreach (var rb in ghostObject.GetComponentsInChildren<Rigidbody>())
            Destroy(rb);
        foreach (var col in ghostObject.GetComponentsInChildren<Collider>())
            Destroy(col);

        ApplyGhostMaterial(ghostObject, ghostValidMaterial);
    }

    bool CheckMultiCellOccupied(Vector3Int originCell, Vector2Int size)
    {
        Vector3 offsetPos = GetRotatedAnchorPosition(originCell);
        Vector3 right = currentRotation * Vector3.right * grid.cellSize.x;
        Vector3 forward = currentRotation * Vector3.forward * grid.cellSize.z;
        Vector3 halfCell = grid.cellSize / 2f;

        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.y; z++)
            {
                Vector3 worldPos = offsetPos + (x * right) + (z * forward);

                if (!Physics.Raycast(worldPos + Vector3.up * 5f, Vector3.down, 10f, groundLayer))
                    return false;

                Collider[] hits = Physics.OverlapBox(worldPos, halfCell * 0.9f, Quaternion.identity, placementLayer);
                if (hits.Length > 0)
                    return false;
            }
        }

        return true;
    }

    void ApplyGhostMaterial(GameObject obj, Material ghostMat)
    {
        foreach (var renderer in obj.GetComponentsInChildren<MeshRenderer>())
            renderer.material = ghostMat;
    }

    Vector3 GetRotatedAnchorPosition(Vector3Int originCell)
    {
        Vector2Int offset = Vector2Int.zero;

        if (currentRotation == Quaternion.Euler(0f, 90f, 0f))
            offset = new Vector2Int(0, 1 - rotatedSize.y);
        else if (currentRotation == Quaternion.Euler(0f, 180f, 0f))
            offset = new Vector2Int(1 - rotatedSize.x, 1 - rotatedSize.y);
        else if (currentRotation == Quaternion.Euler(0f, 270f, 0f))
            offset = new Vector2Int(1 - rotatedSize.x, 0);

        Vector3Int adjustedCell = originCell + new Vector3Int(offset.x, 0, offset.y);
        return grid.GetCellCenterWorld(adjustedCell) + Vector3.up * yOffset;
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying || grid == null) return;

        for (int x = 0; x < rotatedSize.x; x++)
        {
            for (int z = 0; z < rotatedSize.y; z++)
            {
                Vector3Int cell = new Vector3Int(currentCell.x + x, currentCell.y, currentCell.z + z);
                Vector3 center = grid.GetCellCenterWorld(cell) + Vector3.up * yOffset;
                Gizmos.color = isValidPlacement ? Color.green : Color.red;
                Gizmos.DrawWireCube(center, grid.cellSize * 0.9f);
            }
        }
    }
}
