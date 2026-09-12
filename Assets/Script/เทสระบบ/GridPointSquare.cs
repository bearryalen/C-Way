using Unity.VisualScripting;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GridPointSquare : MonoBehaviour
{
    [SerializeField] public Image pointOccupiedImage;

    void Start()
    {
        pointOccupiedImage.gameObject.SetActive(true);
    }


    void Update()
    {

    }

    public void DeactivatePath()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        gameObject.SetActive(false);
    }

    public void ActivatePath()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        gameObject.SetActive(true);
    }

    public void SetOccupied()
    {
        pointOccupiedImage.gameObject.SetActive(true);
    }

    public void SetUnoccupied()
    {
        pointOccupiedImage.gameObject.SetActive(false);
    }
}
