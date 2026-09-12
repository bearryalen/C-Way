using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    // ติด Error ไม่สามารถสร้างจุดเริ่มกับจุดสิ้นสุดได้
    // 
    [SerializeField]
    public GridLevel gridLevel;
    public GameObject squrePointImage; // must be image not game object or else it will not work
    public int TotalPoint { get; set; }
    private List<GameObject> _currentPoint = new List<GameObject>();

    public PathStorage pathStorage;
    public float squaregap = 0.1f;
    public GameObject gridSquare;
    public Vector2 startPosition = new Vector2(0.0f, 0.0f);
    public float squareScale = 0.5f;
    public float everySquareOffset = 0.0f;

    private Vector2 _offset = new Vector2(0.0f, 0.0f);
    private List<GameObject> _gridSquare = new List<GameObject>();

    private void OnEnable()
    {
        Game_Event.checkIfPathCanBePlace += CheckIfPathCanBePlaced;
    }

    private void OnDisable()
    {
        Game_Event.checkIfPathCanBePlace -= CheckIfPathCanBePlaced;
    }

    private void CheckIfPathCanBePlaced()
    {
        var squareIndexs = new List<int>();

        foreach (var square in _gridSquare)
        {
            var gridSquare = square.GetComponent<GridSquare>();

            if (gridSquare.Selected && !gridSquare.isOccupied)
            {
                squareIndexs.Add(gridSquare.squareIndex);
                gridSquare.Selected = false;
                //gridSquare.ActivateSquare();
            }
        }
        var currentSelectedPath = pathStorage.GetCurrentSelectedPath();
        if (currentSelectedPath == null) return; //there is no path selected, so we cannot place it

        if (currentSelectedPath.TotalSquareNumber == squareIndexs.Count)
        {
            foreach (var squareIndex in squareIndexs)
            {
                _gridSquare[squareIndex].GetComponent<GridSquare>().PlacePathOnBoard();

            }

            currentSelectedPath.DeactivatePath();
        }
        else
        {
            Game_Event.MovePathToStartPosition();
        }

        //pathStorage.GetCurrentSelectedPath().DeactivatePath();
    }

    void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        SpawnGridSquare(gridLevel);
        SetGridSquarePosition(gridLevel);
    }

    public void SpawnGridSquare(GridLevel gridLevel)
    {
        int square_index = 0;

        for (var row = 0; row < gridLevel.rows; row++)
        {
            for (var column = 0; column < gridLevel.columns; column++)
            {
                _gridSquare.Add(Instantiate(gridSquare) as GameObject);
                _gridSquare[_gridSquare.Count - 1].GetComponent<GridSquare>().squareIndex = square_index;
                _gridSquare[_gridSquare.Count - 1].transform.SetParent(this.transform);
                _gridSquare[_gridSquare.Count - 1].transform.localScale = new Vector3(squareScale, squareScale, squareScale);
                //_gridSquare[_gridSquare.Count - 1].GetComponent<GridSquare>().SetSquareImage(square_index % 2 == 0);
                square_index++;
            }
        }
    }
    public void SetGridSquarePosition(GridLevel gridLevel)
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
            if (column_number + 1 > gridLevel.columns)
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
            square.GetComponent<RectTransform>().localPosition = new Vector3(startPosition.x + pos_x_offset, startPosition.y - pos_y_offset, 0.0f);

            column_number++;
        }
    }


    public void CreateDestinatePoint(GridLevel PointData)
    {
        gridLevel = PointData;
        TotalPoint = GetNumberOfPointInLevel(PointData);

        while (_currentPoint.Count <= TotalPoint)
        {
            _currentPoint.Add(Instantiate(squrePointImage, transform) as GameObject);
            //var newSquare = Instantiate(squrePathImage, transform);
            //_currentPath.Add(newSquare);
        }

        foreach (var square in _currentPoint)
        {
            square.gameObject.transform.position = Vector3.zero;
            square.gameObject.SetActive(false);
        }

        var squareRect = squrePointImage.GetComponent<RectTransform>();
        var moveDistance = new Vector2(squareRect.rect.width * squareRect.localScale.x, squareRect.rect.height * squareRect.localScale.y);

        int currentSquareIndexInList = 0;

        // set position to form final Path
        for (var row = 0; row < PointData.rows; row++)
        {
            for (var column = 0; column < PointData.columns; column++)
            {
                if (PointData.board[row].columns[column])
                {
                    var shiftOnX = GetXPositionForPathSquare(PointData, column, moveDistance);
                    var shiftOnY = GetYPositionForPathSquare(PointData, row, moveDistance);
                    _currentPoint[currentSquareIndexInList].gameObject.SetActive(true);
                    _currentPoint[currentSquareIndexInList].gameObject.transform.localPosition = new Vector2(shiftOnX, shiftOnY);

                    currentSquareIndexInList++;
                }
            }
        }
    }

    private int GetNumberOfPointInLevel(GridLevel gridLevel)
    {
        int number = 0;

        foreach (var rowData in gridLevel.board)
        {
            foreach (var active in rowData.columns)
            {
                if (active)
                {
                    number++;
                }
            }
        }

        return number;
    }

    private float GetYPositionForPathSquare(GridLevel pointData, int row, Vector2 moveDistance)
    {
        float shiftOnY = 0f;
        if (pointData.rows > 1 ) //Horizontal position Calculation
        {
            if (pointData.rows % 2 != 0)
            {
                var middleSquareIndex = (pointData.rows - 1) / 2;
                var multiplier = (pointData.rows - 1) / 2;
                if (row < middleSquareIndex) // move it minus
                {
                    shiftOnY = -1 * (multiplier - row) * moveDistance.y;
                    shiftOnY *= multiplier;
                }
                else if (row > middleSquareIndex) //move it plus
                {
                    shiftOnY = (row - multiplier) * moveDistance.y;
                    shiftOnY *= multiplier;
                }
            }
            else
            {
                var middleSquareIndex2 = (pointData.rows == 2) ? 1 : (pointData.rows / 2);
                var middleSquareIndex1 = (pointData.rows == 2) ? 0 : pointData.rows - 1;
                var multiplier = pointData.rows / 2;
                if (row == middleSquareIndex1 || row == middleSquareIndex2)
                {
                    if (row == middleSquareIndex2)
                        shiftOnY = moveDistance.y / 2;
                    if (row == middleSquareIndex1)
                        shiftOnY = (moveDistance.y / 2) * -1;
                }

                if (row < middleSquareIndex1 && row < middleSquareIndex2) // move it minus
                {
                    shiftOnY = moveDistance.y * -1;
                    shiftOnY *= multiplier;
                }
                else if (row > middleSquareIndex1 && row > middleSquareIndex2) //move it plus
                {
                    shiftOnY = moveDistance.y * 1;
                    shiftOnY *= multiplier;
                }
            }
        }
        return shiftOnY;
    }


    private float GetXPositionForPathSquare(GridLevel pointData, int column, Vector2 moveDistance)
    {
        float shiftOnX = 0f;
        if (pointData.columns > 1) //Vertical position Calculation
        {
            if (pointData.columns % 2 != 0)
            {
                var middleSquareIndex = (pointData.columns - 1) / 2;
                var multiplier = (pointData.columns - 1) / 2;
                if (column < middleSquareIndex) // move it minus
                {
                    shiftOnX = -1 * (multiplier - column) * moveDistance.x;
                    shiftOnX *= multiplier;
                }
                else if (column > middleSquareIndex) //move it plus
                {
                    shiftOnX = (column - multiplier) * moveDistance.x;
                    shiftOnX *= multiplier;
                }

            }
            else
            {
                var middleSquareIndex2 = (pointData.columns == 2) ? 1 : (pointData.columns / 2);
                var middleSquareIndex1 = (pointData.columns == 2) ? 0 : pointData.columns - 1;
                var multiplier = pointData.columns / 2;

                if (column == middleSquareIndex1 || column == middleSquareIndex2)
                {
                    if (column == middleSquareIndex2)
                        shiftOnX = moveDistance.x / 2;
                    if (column == middleSquareIndex1)
                        shiftOnX = (moveDistance.x / 2) * -1;
                }

                if (column < middleSquareIndex1 && column < middleSquareIndex2) // move it minus
                {
                    shiftOnX = moveDistance.x * -1;
                    shiftOnX *= multiplier;
                }
                else if (column > middleSquareIndex1 && column > middleSquareIndex2) //move it plus
                {
                    shiftOnX = moveDistance.x * 1;
                    shiftOnX *= multiplier;
                }
            }
        }
        return shiftOnX;
    }


}