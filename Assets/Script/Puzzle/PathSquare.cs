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
}
