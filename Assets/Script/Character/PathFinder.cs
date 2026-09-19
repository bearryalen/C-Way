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
        if (generatePathOnStart)
            GeneratePath();
    }

    public void GeneratePath()
    {
        if (gridLevel == null) return;

        // Example: set start/end (use your own values or expose UI)
        gridLevel.startPoint = new Vector2(0, 0);   // column, row
        gridLevel.endPoint = new Vector2(gridLevel.columns - 1, gridLevel.rows - 1);

        // Draw a contiguous line between the points on the grid
        gridLevel.CompletePathBetweenStartAndEnd(clearBefore: true);

        _pathGenerated = true;
        Debug.Log("Grid path generated between start and end.");
    }

    public void ResetPath()
    {
        if (gridLevel == null) return;
        gridLevel.Clear();
        _pathGenerated = false;
        Debug.Log("Grid path reset.");
    }

    // Optional helper to generate using parameters
    public void FollowedPath(Vector2 startPosition, Vector2 endPosition)
    {
        if (gridLevel == null) return;
        gridLevel.startPoint = startPosition;
        gridLevel.endPoint = endPosition;
        gridLevel.CompletePathBetweenStartAndEnd();
    }
}
