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
    public bool isObstacle { get; private set; }
    void Awake()
    {
        Selected = false;
        isOccupied = false;
        isObstacle = false;
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
        isObstacle = false;
    }
    public void SetSquareImage(bool setFirstImage)
    {
        squareImage.GetComponent<Image>().sprite = setFirstImage ? squareSprites[1] : squareSprites[0];

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        var pathSquare = collision.GetComponent<PathSquare>();
        if (pathSquare != null)
        {
            if (isObstacle)
            {
                pathSquare.SetOccupied();
                return;
            }

            // If this grid square is already occupied, mark the path square as occupied
            if (isOccupied)
            {
                pathSquare.SetOccupied();
                return;
            }

            // If there is a Path parent, check cost before allowing selection
            var path = pathSquare.GetComponentInParent<Path>();
            if (path != null)
            {
                var pathStorage = GameObject.FindObjectOfType<PathStorage>();
                if (pathStorage != null)
                {
                    if (!pathStorage.CanAfford(path))
                    {
                        // cannot select this square because it would exceed max cost
                        return;
                    }
                }
            }

            // allow selection
            Selected = true;
            hooverImage.gameObject.SetActive(true);
        }
        else
        {
            if (isOccupied == false)
            {
                Selected = true;
                hooverImage.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var pathSquare = collision.GetComponent<PathSquare>();
        if (pathSquare != null)
        {
            if (isObstacle)
            {
                pathSquare.SetOccupied();
                return;
            }

            if (isOccupied)
            {
                pathSquare.SetOccupied();
                return;
            }

            var path = pathSquare.GetComponentInParent<Path>();
            if (path != null)
            {
                var pathStorage = GameObject.FindObjectOfType<PathStorage>();
                if (pathStorage != null)
                {
                    if (!pathStorage.CanAfford(path))
                    {
                        // cannot select this square because it would exceed max cost
                        return;
                    }
                }
            }

            Selected = true;
            if (isOccupied == false)
                hooverImage.gameObject.SetActive(true);
        }
        else
        {
            Selected = true;
            if (isOccupied == false)
                hooverImage.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var pathSquare = collision.GetComponent<PathSquare>();
        if (pathSquare != null)
        {
            // Always clear the temporary collision image when leaving a cell.
            // This is especially important for obstacle cells, which are not occupied.
            pathSquare.SetUnoccupied();
        }

        if (isOccupied == false)
        {
            Selected = false;
            hooverImage.gameObject.SetActive(false);
        }
    }

    public void PlacePathOnBoard()
    {
        if (isObstacle) return;
        ActivateSquare();
    }

    public void SetObstacle()
    {
        isObstacle = true;
        Selected = false;
        isOccupied = false;
        hooverImage.gameObject.SetActive(false);
        activeImage.gameObject.SetActive(false);
    }
}
