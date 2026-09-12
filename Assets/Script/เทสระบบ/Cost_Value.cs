using UnityEngine;

public class Cost_Value : MonoBehaviour
{
    
    [SerializeField] public int maxCost;
    public int currentCost = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //TODO: print( currentcost/maxcost )
        //
        if(currentCost >= maxCost)
        {
            //TODO cant select more path

        }


        //TODO if reset == true then currentCost = 0
    }


    public void ValueChange(int value)
    {
        currentCost += value;
    }
}
