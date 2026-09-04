using Unity.VisualScripting;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PathSquare : MonoBehaviour
{

    [SerializeField]public Image occupiedImage;

    void Start()
    {
        occupiedImage.gameObject.SetActive(false);
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
}
