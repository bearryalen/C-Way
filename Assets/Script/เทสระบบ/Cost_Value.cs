using UnityEngine;

public class Cost_Value : MonoBehaviour
{
    
    [SerializeField] public int maxCost;
    public int currentCost = 0;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void ValueChange(int value)
    {
        currentCost += value;
        Debug.Log("Current Cost: " + currentCost + "/" + maxCost);
    }
}
