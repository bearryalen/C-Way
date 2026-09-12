using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Storage", menuName = "Scriptable Objects/Storage")]
public class Storage : ScriptableObject
{
    public List<PathData> pathDatas;
    public List<Path> pathList;

    void Start()
    {
        int count = Mathf.Min(pathList.Count, pathDatas.Count);
        for (int i = 0; i < count; i++)
        {
            if (pathList[i] != null && pathDatas[i] != null)
            {
                pathList[i].CreatePath(pathDatas[i]);
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
