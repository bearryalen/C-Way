using UnityEngine;

public class PathFinder : MonoBehaviour
{

    [SerializeField] private bool generatePathOnStart;
    [SerializeField] GridLevel gridLevel;

    private bool _pathGenerated;
    public PathData startDestinate;
    public PathData endDestinate;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GeneratePath()
    {
        if (generatePathOnStart && !_pathGenerated)
        {
            // Implement your path generation logic here
            Debug.Log("Generating path...");
            _pathGenerated = true;
        }
    }

    public void ResetPath()
    {
        // Implement your path reset logic here
        Debug.Log("Resetting path...");
        _pathGenerated = false;
    }

    public void FindPath(Vector2 startPosition, Vector2 endPosition)
    {

    }
}
