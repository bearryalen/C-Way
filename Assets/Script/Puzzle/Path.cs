using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Path : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IBeginDragHandler , IDragHandler, IEndDragHandler , IPointerDownHandler
{
    public GameObject squrePathImage;
    public Vector3 pathSelectedScale;
    public Vector2 offset = new Vector2(-500, 10f);
    public PathData CurrentPathData;

    public int TotalSquareNumber {get; set;}

    private List<GameObject> _currentPath = new List<GameObject>();
    private Vector3 _pathStartScale;
    private RectTransform _transform;
    //private bool _isPathSelected = false;
    //private bool _pathDraggable = true;
    private Canvas _canvas;
    private Vector3 _startPosition;
    private bool _pathActive = true;
    private bool _blockedByObstacle;


    public void Awake()
    {
        _pathStartScale = this.GetComponent<RectTransform>().localScale;
        _transform = this.GetComponent<RectTransform>();
        _canvas = this.GetComponentInParent<Canvas>();
        //_pathDraggable = true;
        _startPosition = _transform.localPosition;
        _pathActive = true;
        _blockedByObstacle = false;
    }

    public bool IsOnStartPosition()
    {
        return _transform.localPosition == _startPosition;
    }

    public bool IsAnyOfSquareActive()
    {
        foreach (var square in _currentPath)
        {
            if (square.gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    public void ResetForReplay()
    {
        StopAllCoroutines();
        _transform.localPosition = _startPosition;
        _transform.localScale = _pathStartScale;
        _pathActive = true;
        _blockedByObstacle = false;

        if (CurrentPathData != null)
        {
            // Rebuild the visual path so only the required squares are active.
            CreatePath(CurrentPathData);
        }
    }

    public void DeactivatePath()
    {
        if (_pathActive)
        {
            foreach (var square in _currentPath)
            {
                square?.GetComponent<PathSquare>().DeactivatePath();
            }
        }

        _pathActive = false;
    }

    public void ActivatePath()
    {
        if (!_pathActive)
        {
            foreach (var square in _currentPath)
            {
                square?.GetComponent<PathSquare>().ActivatePath();
            }
        }
        _pathActive = true;
    }


    public void RequestNewPath(PathData pathData)
    {
        _transform.localPosition = _startPosition;
        CreatePath(pathData);
    }

    public void CreatePath(PathData pathData)
    {
        CurrentPathData = pathData;
        TotalSquareNumber = GetNumberOfSquaresInPath(pathData);

        while (_currentPath.Count <= TotalSquareNumber)
        {
            _currentPath.Add(Instantiate(squrePathImage, transform) as GameObject);
            //var newSquare = Instantiate(squrePathImage, transform);
            //_currentPath.Add(newSquare);
        }

        foreach (var square in _currentPath)
        {
            square.gameObject.transform.position = Vector3.zero;
            square.gameObject.SetActive(false);
        }

        var squareRect = squrePathImage.GetComponent<RectTransform>();
        var moveDistance = new Vector2(squareRect.rect.width * squareRect.localScale.x, squareRect.rect.height * squareRect.localScale.y);

        int currentSquareIndexInList = 0;

        // set position to form final Path
        for (var row = 0; row < pathData.rows; row++)
        {
            for (var column = 0; column < pathData.columns; column++)
            {
                if (pathData.board[row].columns[column])
                {
                    var shiftOnX = GetXPositionForPathSquare(pathData, column, moveDistance);
                    var shiftOnY = GetYPositionForPathSquare(pathData, (pathData.rows - 1) - row, moveDistance);
                    var pathSquareObject = _currentPath[currentSquareIndexInList];
                    pathSquareObject.transform.localPosition = new Vector2(shiftOnX, shiftOnY);

                    // DeactivatePath disables the collider. Re-enable it when rebuilding
                    // the path so it can be dragged and detected after a reset.
                    var pathSquare = pathSquareObject.GetComponent<PathSquare>();
                    if (pathSquare != null)
                        pathSquare.ActivatePath();
                    else
                        pathSquareObject.SetActive(true);

                    currentSquareIndexInList++;
                }
            }
        }
    }

    private void OnEnable()
    {
        Game_Event.MovePathToStartPosition += MovePathToStartPosition; 
    }

    private void OnDisable()
    {
        Game_Event.MovePathToStartPosition -= MovePathToStartPosition; 
    }

    private int GetNumberOfSquaresInPath(PathData pathData)
    {
        int number = 0;

        foreach (var rowData in pathData.board)
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



    private float GetYPositionForPathSquare(PathData pathData,int row,Vector2 moveDistance)
    {
        float shiftOnY = 0f;
        if(pathData.rows > 1 ) //Horizontal position Calculation
        {
            if (pathData.rows % 2 != 0)
            {
                var middleSquareIndex = (pathData.rows - 1) / 2;
                var multiplier = (pathData.rows - 1) / 2;
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
                var middleSquareIndex2 = (pathData.rows == 2) ? 1 : (pathData.rows / 2);
                var middleSquareIndex1 = (pathData.rows == 2) ? 0 : pathData.rows - 1;
                var multiplier = pathData.rows / 2;
                if (row == middleSquareIndex1 || row == middleSquareIndex2)
                {
                    if(row == middleSquareIndex2)
                        shiftOnY = moveDistance.y / 2;
                    if(row == middleSquareIndex1)
                        shiftOnY = (moveDistance.y / 2) * -1;
                }
                
                if(row < middleSquareIndex1 && row < middleSquareIndex2) // move it minus
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


    private float GetXPositionForPathSquare(PathData pathData,int column,Vector2 moveDistance)
    {
        float shiftOnX = 0f;
        if(pathData.columns > 1 ) //Vertical position Calculation
        {
            if (pathData.columns % 2 != 0)
            {
                var middleSquareIndex = (pathData.columns - 1) / 2;
                var multiplier = (pathData.columns - 1) / 2;
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
                var middleSquareIndex2 = (pathData.columns == 2) ? 1 : (pathData.columns / 2);
                var middleSquareIndex1 = (pathData.columns == 2) ? 0 : pathData.columns - 1;
                var multiplier = pathData.columns / 2;

                if (column == middleSquareIndex1 || column == middleSquareIndex2)
                {
                    if(column == middleSquareIndex2)
                        shiftOnX = moveDistance.x / 2;
                    if(column == middleSquareIndex1)
                        shiftOnX = (moveDistance.x / 2) * -1;
                }
                
                if(column < middleSquareIndex1 && column < middleSquareIndex2) // move it minus
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



    public void OnPointerClick(PointerEventData eventData)
    {
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _blockedByObstacle = false;

        var pathStorage = FindObjectOfType<PathStorage>();
        if (pathStorage != null)
            pathStorage.SelectPath(this);

        // Clear collision indicators left from the previous attempt or a reset.
        foreach (var square in _currentPath)
        {
            if (square == null) continue;

            var pathSquare = square.GetComponent<PathSquare>();
            if (pathSquare != null)
                pathSquare.SetUnoccupied();
        }

        _pathActive = true;
        this.GetComponent<RectTransform>().localScale = pathSelectedScale;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _transform.anchorMin = new Vector2(0, 0);
        _transform.anchorMax = new Vector2(0, 0);
        _transform.pivot = new Vector2(0, 0);

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, eventData.position, Camera.main, out pos);
        _transform.localPosition = pos + offset;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.GetComponent<RectTransform>().localScale = _pathStartScale;

        if (_blockedByObstacle)
        {
            Game_Event.MovePathToStartPosition?.Invoke();
            return;
        }

        Game_Event.checkIfPathCanBePlace();
    }

    public void MarkBlockedByObstacle()
    {
        _blockedByObstacle = true;
    }

    public bool IsBlockedByObstacle()
    {
        return _blockedByObstacle;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    private void MovePathToStartPosition()
    {
        _transform.transform.localPosition = _startPosition;
    }
}
