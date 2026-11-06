using System.Collections.Generic;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Storage", menuName = "Scriptable Objects/Storage")]
public class Storage : ScriptableObject
{
    [Header("Path Cost Limit")]
    [Min(0)] public int maxCost = 100;
    [System.NonSerialized] private int currentCost;

    public int CurrentCost => currentCost;
    public int RemainingCost => Mathf.Max(0, maxCost - currentCost);
    public List<PathData> pathDatas;
    public List<Path> pathList;

    public int GetPathCost(Path path)
    {
        if (path == null || path.CurrentPathData == null)
            return 0;

        return path.TotalSquareNumber * Mathf.Max(0, path.CurrentPathData.costValue);
    }

    public bool CanAfford(Path path)
    {
        return currentCost + GetPathCost(path) <= maxCost;
    }

    public bool TryAddPathCost(Path path)
    {
        int cost = GetPathCost(path);
        if (currentCost + cost > maxCost)
            return false;

        currentCost += cost;
        return true;
    }

    public void ResetCost()
    {
        currentCost = 0;
    }

    // Ensure lists are the same size in the editor
    private void OnValidate()
    {
        SyncPathDatasListSize();
    }

    // Context menu to quickly sync list sizes in the inspector
    [ContextMenu("Sync PathDatas List Size")]
    public void SyncPathDatasListSize()
    {
        if (pathList == null) pathList = new List<Path>();
        if (pathDatas == null) pathDatas = new List<PathData>();

        // Expand or shrink pathDatas to match pathList count so indices align in the inspector
        while (pathDatas.Count < pathList.Count)
        {
            pathDatas.Add(null);
        }
        while (pathDatas.Count > pathList.Count)
        {
            pathDatas.RemoveAt(pathDatas.Count - 1);
        }
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
