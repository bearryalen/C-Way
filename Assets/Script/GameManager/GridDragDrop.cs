using UnityEngine;

public class GridDragDrop : MonoBehaviour
{
    private Grid targetGrid;
    private Vector3 offset;
    private Vector3 originalPosition;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        // Find the Grid component in the scene automatically
        targetGrid = Object.FindFirstObjectByType<Grid>();

        if (targetGrid == null)
        {
            Debug.LogError("No Grid component found in the scene!");
        }
    }

    void OnMouseDown()
    {
        // Record starting position in case placement fails
        originalPosition = transform.position;

        // Calculate offset to prevent the object center from snapping to the cursor tip instantly
        Vector3 mouseWorldPos = GetMouseWorldPosition();
        offset = transform.position - mouseWorldPos;
    }

    void OnMouseDrag()
    {
        // Move the object smoothly with the cursor
        transform.position = GetMouseWorldPosition() + offset;
    }

    void OnMouseUp()
    {
        if (targetGrid != null)
        {
            // Convert current world position to the nearest integer grid cell coordinate
            Vector3Int cellPosition = targetGrid.WorldToCell(transform.position);

            // Center the item precisely in the middle of that grid cell
            Vector3 snappedPosition = targetGrid.GetCellCenterWorld(cellPosition);

            // Lock Z axis back to 0 for 2D spacing
            snappedPosition.z = 0;

            // Update item position to the cell center
            transform.position = snappedPosition;
        }
        else
        {
            // Fallback if grid vanishes
            transform.position = originalPosition;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        // Convert mouse pixel coordinates into the game world coordinates
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(mainCamera.transform.position.z);
        return mainCamera.ScreenToWorldPoint(mouseScreenPos);
    }
}
