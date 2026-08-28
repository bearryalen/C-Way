using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public GameObject squrePathImage;

    [HideInInspector]
    public PathData CurrentPathData;

    private List<GameObject> _currentPath = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
}
