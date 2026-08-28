using UnityEngine;

[CreateAssetMenu(fileName = "GridLevel", menuName = "Scriptable Objects/GridLevel")]
public class GridLevel : ScriptableObject
{
    [Header("Grid Size")]
    public int width = 10;
    public int height = 10;

    [Header("Grid Reference")]
    public GridLevel grid;

    private void Awake()
    {
        //if (grid == null)
            //grid = GetComponent<GridLevel>();
    }

    public Vector3Int WorldToCell(Vector3 worldPosition)
    {
        return grid.WorldToCell(worldPosition);
    }

    public Vector3 GetCellCenter(Vector3Int cellPosition)
    {
        return grid.GetCellCenter(cellPosition);
    }

    public bool IsInsideGrid(Vector3Int cell)
    {
        return cell.x >= 0 &&
               cell.x < width &&
               cell.y >= 0 &&
               cell.y < height;
    }
}
