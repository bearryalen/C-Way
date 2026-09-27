using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Storage", menuName = "Scriptable Objects/Storage")]
public class Storage : ScriptableObject
{
    public List<PathData> pathDatas;
    public List<Path> pathList;

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
