using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GridLevel", menuName = "Scriptable Objects/GridLevel")]
public class GridLevel : ScriptableObject
{
    [System.Serializable]
    public class Row
    {
        public bool[] columns;
        private int _size;

        public Row() { }

        public Row(int size)
        {
            CreateRow(size);
        }

        public void CreateRow(int size)
        {
            _size = size;
            columns = new bool[_size];
            ClearRow();
        }

        public void ClearRow()
        {
            for (int i = 0; i < _size; i++)
            {
                columns[i] = false;
            }
        }
    }


    public int columns;
    public int rows;
    public Row[] board;
    public Vector2 startPoint = Vector2.zero;
    public Vector2 endPoint = Vector2.zero;

    public void Clear()
    {
        for (int i = 0; i < rows; i++)
        {
            board[i].ClearRow();
        }
    }

    public void CreateNewBoard()
    {
        board = new Row[rows];
        for (var i = 0; i < rows; i++)
        {
            board[i] = new Row(columns);
        }
    }

    /// <summary>
    /// Sets board cells along a continuous grid line between startPoint and endPoint.
    /// Coordinates: startPoint.x => column, startPoint.y => row.
    /// Values are rounded and clamped to the board bounds.
    /// Uses Bresenham's line algorithm to ensure contiguous cells.
    /// If clearBefore is true, the board is cleared before drawing the path.
    /// </summary>
    public void CompletePathBetweenStartAndEnd(bool clearBefore = true)
    {
        if (board == null || rows <= 0 || columns <= 0)
            return;

        if (clearBefore)
            Clear();

        int x0 = Mathf.RoundToInt(startPoint.x);
        int y0 = Mathf.RoundToInt(startPoint.y);
        int x1 = Mathf.RoundToInt(endPoint.x);
        int y1 = Mathf.RoundToInt(endPoint.y);

        x0 = Mathf.Clamp(x0, 0, columns - 1);
        x1 = Mathf.Clamp(x1, 0, columns - 1);
        y0 = Mathf.Clamp(y0, 0, rows - 1);
        y1 = Mathf.Clamp(y1, 0, rows - 1);

        int dx = Mathf.Abs(x1 - x0);
        int sx = x0 < x1 ? 1 : -1;
        int dy = -Mathf.Abs(y1 - y0);
        int sy = y0 < y1 ? 1 : -1;
        int err = dx + dy;

        while (true)
        {
            board[y0].columns[x0] = true;

            if (x0 == x1 && y0 == y1) break;

            int e2 = 2 * err;
            if (e2 >= dy)
            {
                err += dy;
                x0 += sx;
            }
            if (e2 <= dx)
            {
                err += dx;
                y0 += sy;
            }
        }
    }
}
