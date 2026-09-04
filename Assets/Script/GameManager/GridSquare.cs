using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class GridSquare : MonoBehaviour
{
    [Header("Insert Square Image and Sprites")]
    public Image squareImage;
    public Image hooverImage;
    public Image activeImage;

    public List<Sprite> squareSprites;

    public bool Selected { get; set; }
    public int squareIndex { get; set; }
    public bool isOccupied { get; set; }
    void Start()
    {
        Selected = false;
        isOccupied = false;
    }


    //temp function. Remove later
    public bool CanWeUseThisSquare()
    {
        return hooverImage.gameObject.activeSelf;
    }


    public void ActivateSquare()
    {
        hooverImage.gameObject.SetActive(false);
        activeImage.gameObject.SetActive(true);
        Selected = true;
        isOccupied = true;
    }
    public void SetSquareImage(bool setFirstImage)
    {
        squareImage.GetComponent<Image>().sprite = setFirstImage ? squareSprites[1] : squareSprites[0];

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isOccupied == false)
        {
            Selected = true;
            hooverImage.gameObject.SetActive(true);
        }
        Debug.Log("OnTriggerEnter2D");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Selected = true;
        if (isOccupied == false)
            hooverImage.gameObject.SetActive(true);
        Debug.Log("OnTriggerStay2D");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isOccupied == false)
        {
            Selected = false;
            hooverImage.gameObject.SetActive(false);
        }
        Debug.Log("OnTriggerExit2D");
    }

    public void PlacePathOnBoard()
    {
        ActivateSquare();
    }
}
