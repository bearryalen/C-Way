using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GridSquare : MonoBehaviour
{
    [Header("Insert Square Image and Sprites")]
    public Image squareImage;

    public List<Sprite> squareSprites;

    void Start()
    {
        
    }


    public void SetSquareImage(bool setFirstImage)
    {
        squareImage.GetComponent<Image>().sprite = setFirstImage ? squareSprites[1] : squareSprites[0];

    }
    
}
