using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
public class GridArea : GridSquare
{
    public Image startImage;
    public Image endImage;
    public Image obstacleImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AreaActive(int areaActive) // 0 = start, 1 = end, 2 = obstacle ,// สามารถเปลี่ยน int เป็น GridArea เพื่อสามารถใช้ if ข้างในไปใช้ได้ แต่มันบัค
    {
        hooverImage.gameObject.SetActive(false);
        activeImage.gameObject.SetActive(true);
        Selected = true;
        isOccupied = true;

        switch(areaActive)
        {
            case 0:
                startImage.gameObject.SetActive(true);
                break;
            case 1:
                endImage.gameObject.SetActive(true);
                break;
            case 2:
                obstacleImage.gameObject.SetActive(true);
                break;
        }

        /*if (areaGrid.startPoint.x == transform.position.x && areaGrid.startPoint.y == transform.position.y)
        {
            startImage.gameObject.SetActive(true);
            Debug.Log("Start point activated at: " + areaGrid.startPoint.x);
        }
        if (areaGrid.endPoint.x == transform.position.x && areaGrid.endPoint.y == transform.position.y)
        {
            endImage.gameObject.SetActive(true);
            Debug.Log("End point activated at: " + areaGrid.endPoint.x);
        }
        /*if (areaGrid.board[(int)transform.position.y].columns[(int)transform.position.x])
        {
            obstacleImage.gameObject.SetActive(true);
            Debug.Log("Obstacle activated at: " + transform.position);
        }*/
    }
}
