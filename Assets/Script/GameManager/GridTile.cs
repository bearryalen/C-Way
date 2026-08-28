using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    [Header("Grid Size")]
    [SerializeField]public int columns;
    [SerializeField]public int rows;

    public float squaregap =0.1f;   
    public GameObject gridSquare;
    public Vector2 startPosition = new Vector2(0.0f, 0.0f);
    public float squareScale = 0.5f;
    public float everySquareOffset = 0.0f;

    private Vector2 _offset = new Vector2(0.0f, 0.0f);
    private List<GameObject> _gridSquare = new List<GameObject>();


    void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        SpawnGridSquare();
        SetGridSquarePosition();
    }

    private void SpawnGridSquare()
    {
        // 0,1,2,3,4,
        // 5,6,7,8,9

        int square_index = 0;

        for (var row = 0; row < rows; row++)
        {
            for (var column = 0; column < columns; column++)
            {
                _gridSquare.Add(Instantiate(gridSquare) as GameObject);
                _gridSquare[_gridSquare.Count - 1].transform.SetParent(this.transform);
                _gridSquare[_gridSquare.Count - 1].transform.localScale = new Vector3(squareScale, squareScale, squareScale);
                _gridSquare[_gridSquare.Count - 1].GetComponent<GridSquare>().SetSquareImage(square_index % 2 == 0);
                square_index++;
            }
        }
    }

    private void SetGridSquarePosition()
    {
        int column_number = 0;
        int row_number = 0;
        Vector2 square_gap_number = new Vector2(0.0f, 0.0f);
        bool row_moved = false;

        var square_size = _gridSquare[0].GetComponent<RectTransform>();

        _offset.x = square_size.rect.width * square_size.transform.localScale.x + everySquareOffset;
        _offset.y = square_size.rect.height * square_size.transform.localScale.y + everySquareOffset;

        foreach (GameObject square in _gridSquare)
        {
            if (column_number + 1 > columns)
            {
                square_gap_number.x = 0.0f;
                column_number = 0;
                row_number++;
                row_moved = false;
            }
            var pos_x_offset = _offset.x * column_number + (square_gap_number.x * squaregap);
            var pos_y_offset = _offset.y * row_number + (square_gap_number.y * squaregap);

            if (column_number > 0 && column_number % 3 == 0)
            {
                square_gap_number.x++;
                pos_x_offset += squaregap;
            }

            if (row_number > 0 && row_number % 3 == 0 && row_moved == false)
            {
                row_moved = true;
                square_gap_number.y++;
                pos_y_offset += squaregap;
            }

            square.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPosition.x + pos_x_offset, startPosition.y - pos_y_offset);
            square.GetComponent<RectTransform>().localPosition = new Vector3(startPosition.x + pos_x_offset, startPosition.y - pos_y_offset , 0.0f);

            column_number++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
