using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Size")]
    public int width = 10;
    public int height = 10;

    private GameObject[,] occupiedCells;

    [Header("Grid Reference")]
    public Grid grid;

    private void Awake()
    {
        if (grid == null)
            grid = GetComponent<Grid>();

        occupiedCells = new GameObject[width, height];
    }

    public bool IsCellOccupied(Vector3Int cell)
    {
        if (!IsInsideGrid(cell))
            return true;

        return occupiedCells[cell.x, cell.y] != null;
    }

    public bool PlaceObject(Vector3Int cell, GameObject obj)
    {
        if (!IsInsideGrid(cell))
            return false;

        if (occupiedCells[cell.x, cell.y] != null)
            return false;

        occupiedCells[cell.x, cell.y] = obj;

        return true;
    }

    public void RemoveObject(Vector3Int cell)
    {
        if (!IsInsideGrid(cell))
            return;

        occupiedCells[cell.x, cell.y] = null;
    }

    public Vector3Int WorldToCell(Vector3 worldPosition)
    {
        return grid.WorldToCell(worldPosition);
    }

    public Vector3 GetCellCenter(Vector3Int cellPosition)
    {
        return grid.GetCellCenterWorld(cellPosition);
    }

    public bool IsInsideGrid(Vector3Int cell)
    {
        return cell.x >= 0 &&
               cell.x < width &&
               cell.y >= 0 &&
               cell.y < height;
    }
}
