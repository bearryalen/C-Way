using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Storage))]
public class StorageEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(6);

        if (GUILayout.Button("Sync PathDatas List Size"))
        {
            var s = target as Storage;
            s.SyncPathDatasListSize();
            EditorUtility.SetDirty(s);
            AssetDatabase.SaveAssets();
            Debug.Log("Storage: Synced pathDatas size to match pathList count.");
        }

        if (GUILayout.Button("Sync PathList From Scene (by name)"))
        {
            var s = target as Storage;
            SyncPathListFromScene(s);
        }

        if (GUILayout.Button("Auto Assign PathData By Name"))
        {
            var s = target as Storage;
            AutoAssignPathDataByName(s);
        }
    }

    private void SyncPathListFromScene(Storage s)
    {
        if (s == null) return;

        // Find all Path instances in the current open scenes
        var paths = UnityEngine.Object.FindObjectsOfType<Path>();
        var list = new List<Path>(paths);

        // Sort by GameObject name for deterministic ordering
        list.Sort((a, b) => string.Compare(a.gameObject.name, b.gameObject.name));

        s.pathList = list;
        s.SyncPathDatasListSize();
        EditorUtility.SetDirty(s);
        AssetDatabase.SaveAssets();

        Debug.Log($"Storage: Found {list.Count} Path objects in scene and synced pathList.\nMake sure pathDatas indexes correspond to this order.");
    }

    private void AutoAssignPathDataByName(Storage s)
    {
        if (s == null) return;

        // Load all PathData assets in project
        var guids = AssetDatabase.FindAssets("t:PathData");
        var pathDataByName = new Dictionary<string, PathData>();
        foreach (var guid in guids)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var pd = AssetDatabase.LoadAssetAtPath<PathData>(assetPath);
            if (pd != null)
            {
                pathDataByName[pd.name] = pd;
            }
        }

        int assigned = 0;
        for (int i = 0; i < s.pathList.Count; i++)
        {
            var path = s.pathList[i];
            if (path == null) continue;

            PathData found = null;
            // Try exact GameObject name
            if (pathDataByName.TryGetValue(path.gameObject.name, out found))
            {
                s.pathDatas[i] = found;
                assigned++;
                continue;
            }

            // Try Path name (component name) without (Clone)
            var cleaned = path.gameObject.name.Replace("(Clone)", "").Trim();
            if (pathDataByName.TryGetValue(cleaned, out found))
            {
                s.pathDatas[i] = found;
                assigned++;
                continue;
            }
        }

        EditorUtility.SetDirty(s);
        AssetDatabase.SaveAssets();
        Debug.Log($"Storage: Auto assigned {assigned} PathData assets by name.");
    }
}
