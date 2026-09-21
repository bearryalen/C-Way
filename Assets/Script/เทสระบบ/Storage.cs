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

    void Start()
    {
        // At runtime, try to initialize each Path with its corresponding PathData if available
        if (pathList == null) return;
        if (pathDatas == null) pathDatas = new List<PathData>();

        // Make sure lists align
        SyncPathDatasListSize();

        for (int i = 0; i < pathList.Count; i++)
        {
            var path = pathList[i];
            PathData data = (i < pathDatas.Count) ? pathDatas[i] : null;
            if (path != null && data != null)
            {
                path.CreatePath(data);
            }
            else if (path != null && data == null)
            {
                Debug.LogWarning($"Storage: Path at index {i} has no PathData assigned. Assign a PathData in the Storage inspector or use Sync PathDatas List Size.");
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
