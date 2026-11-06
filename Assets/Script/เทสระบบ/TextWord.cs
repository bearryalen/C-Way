using UnityEngine;
using UnityEngine.UI;

public class TextWord : MonoBehaviour
{

    public Text textComponent;
    public string text_value;

    void Start()
    {
        textComponent = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        textComponent.text = text_value;
    }

}
