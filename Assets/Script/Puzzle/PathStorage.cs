using UnityEngine;
using System.Collections.Generic;
using UnityEngine;


public class PathStorage : MonoBehaviour
{
    [Tooltip("Optional: assigned automatically from LevelManager.LevelStorage")]
    public Storage storageAsset;

    public List<PathData> pathDatas = new List<PathData>();
    public List<Path> pathList = new List<Path>();

    void Start()
    {
        // If a Storage asset is assigned, try to use its lists (indices should match)
        if (storageAsset != null)
        {
            // Prefer stored list only when it contains valid scene references
            var storedList = storageAsset.pathList ?? new List<Path>();
            bool hasNull = false;
            if (storedList.Count == 0) hasNull = true;
            else
            {
                foreach (var p in storedList)
                {
                    if (p == null) { hasNull = true; break; }
                }
            }

            if (!hasNull)
            {
                pathList = new List<Path>(storedList);
            }
            else
            {
                // Stored list contains nulls or is empty (ScriptableObject cannot reliably store scene refs).
                // Fall back to finding Path components in the open scenes.
                var found = UnityEngine.Object.FindObjectsOfType<Path>(true);
                var listFound = new List<Path>(found);
                listFound.Sort((a, b) => string.Compare(a.gameObject.name, b.gameObject.name));
                pathList = listFound;
            }

            pathDatas = storageAsset.pathDatas ?? new List<PathData>();
        }

        // If pathList is empty, try to find Path components among children
        if (pathList == null || pathList.Count == 0)
        {
            var found = GetComponentsInChildren<Path>(true);
            pathList = new List<Path>(found);
        }

        // Ensure pathDatas list matches pathList size if possible
        if (pathDatas == null) pathDatas = new List<PathData>();

        // Initialize each Path. If pathDatas has the same size as pathList, assign by index.
        for (int i = 0; i < pathList.Count; i++)
        {
            var path = pathList[i];
            if (path == null) continue;

            PathData dataToUse = null;
            if (i < pathDatas.Count && pathDatas[i] != null)
            {
                dataToUse = pathDatas[i];
            }
            else if (pathDatas.Count > 0)
            {
                // fallback: random selection
                dataToUse = pathDatas[Random.Range(0, pathDatas.Count)];
            }

            if (dataToUse != null)
            {
                path.CreatePath(dataToUse);
            }
            else
            {
                Debug.LogWarning($"PathStorage: No PathData for Path '{path.gameObject.name}' (index {i}).");
            }
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
