using UnityEngine;

public class GridVisualizer : MonoBehaviour
{
    public GridManager gridManager;

    private void OnDrawGizmos()
    {
        if (gridManager == null || gridManager.grid == null)
            return;

        Grid grid = gridManager.grid;

        for (int x = 0; x <= gridManager.width; x++)
        {
            Vector3 start = grid.CellToWorld(new Vector3Int(x, 0, 0));
            Vector3 end = grid.CellToWorld(
                new Vector3Int(x, gridManager.height, 0)
            );

            Gizmos.DrawLine(start, end);
        }

        for (int y = 0; y <= gridManager.height; y++)
        {
            Vector3 start = grid.CellToWorld(new Vector3Int(0, y, 0));
            Vector3 end = grid.CellToWorld(
                new Vector3Int(gridManager.width, y, 0)
            );

            Gizmos.DrawLine(start, end);
        }
    }
}
