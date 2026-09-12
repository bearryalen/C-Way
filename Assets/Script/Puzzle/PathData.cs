using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PathData", menuName = "Scriptable Objects/PathData")]
public class PathData : ScriptableObject
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

    
}
