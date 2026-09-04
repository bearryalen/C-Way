using UnityEngine;
using System.Collections.Generic;

public class Storage : MonoBehaviour
{
    public List<PathData> pathDatas;
    public List<Path> pathList;

    void Start()
    {

        // Initialize the pathList with Path components from child objects
        // Create Path with random Sprawn data from pathDatas
        foreach ( var path in pathList)
        {
            // Randomly select a PathData from the pathDatas list
            var pathIndex = UnityEngine.Random.Range(0, pathDatas.Count);
            path.CreatePath(pathDatas[pathIndex]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Path GetCurrentSelectedPath()
    {
        foreach (var path in pathList)
        {
            if (path.IsOnStartPosition() == false && path.IsAnyOfSquareActive())
            {
                return path;
            }
        }
        Debug.LogError("No path is currently selected.");
        return null;
    }
}
