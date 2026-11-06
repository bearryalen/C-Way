using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private float moveSpeed = 300f; // units per second (localPosition units)

    private void OnEnable()
    {
        Game_Event.RequestNewPath += OnRequestNewPath;
    }

    private void OnDisable()
    {
        Game_Event.RequestNewPath -= OnRequestNewPath;
    }

    private void OnRequestNewPath()
    {
        if (levelManager == null) return;

        var grid = levelManager.GetOccupiedGrid();
        int rows = levelManager.Rows;
        int cols = levelManager.Columns;

        if (rows == 0 || cols == 0) return;

        int startR = Mathf.Clamp((int)levelManager.gridLevel.startPoint.y, 0, rows - 1);
        int startC = Mathf.Clamp((int)levelManager.gridLevel.startPoint.x, 0, cols - 1);
        int endR = Mathf.Clamp((int)levelManager.gridLevel.endPoint.y, 0, rows - 1);
        int endC = Mathf.Clamp((int)levelManager.gridLevel.endPoint.x, 0, cols - 1);

        if (!grid[startR, startC] || !grid[endR, endC])
            return;

        var path = FindShortestPath(grid, startR, startC, endR, endC, rows, cols);
        if (path == null || path.Count == 0) return;

        // Place player at start cell
        transform.SetParent(levelManager.transform);
        transform.localPosition = levelManager.GetCellWorldPosition(startR, startC);
        transform.SetAsLastSibling();

        StopAllCoroutines();
        StartCoroutine(MoveAlongPath(path));
    }

    private void Start()
    {
        // If player prefab is present in scene, place it on start when grid is ready
        if (levelManager != null)
        {
            StartCoroutine(PlaceAtStartWhenReady());
        }
    }

    private IEnumerator PlaceAtStartWhenReady()
    {
        // wait until LevelManager finished creating grid squares
        while (levelManager != null && !levelManager.IsGridReady())
            yield return null;

        if (levelManager == null) yield break;

        int rows = levelManager.Rows;
        int cols = levelManager.Columns;
        if (rows == 0 || cols == 0) yield break;

        int startR = Mathf.Clamp((int)levelManager.gridLevel.startPoint.y, 0, rows - 1);
        int startC = Mathf.Clamp((int)levelManager.gridLevel.startPoint.x, 0, cols - 1);

        transform.SetParent(levelManager.transform);
        transform.localPosition = levelManager.GetCellWorldPosition(startR, startC);
        transform.SetAsLastSibling();
    }

    private List<Vector2Int> FindShortestPath(bool[,] grid, int sr, int sc, int tr, int tc, int rows, int cols)
    {
        var visited = new bool[rows, cols];
        var from = new Dictionary<Vector2Int, Vector2Int>();
        var q = new Queue<Vector2Int>();
        var start = new Vector2Int(sr, sc);
        var target = new Vector2Int(tr, tc);
        q.Enqueue(start);
        visited[sr, sc] = true;

        int[] dr = new int[] { -1, 1, 0, 0 };
        int[] dc = new int[] { 0, 0, -1, 1 };

        bool found = false;
        while (q.Count > 0)
        {
            var cur = q.Dequeue();
            if (cur == target) { found = true; break; }
            for (int i = 0; i < 4; i++)
            {
                int nr = cur.x + dr[i];
                int nc = cur.y + dc[i];
                if (nr >= 0 && nr < rows && nc >= 0 && nc < cols && !visited[nr, nc] && grid[nr, nc])
                {
                    visited[nr, nc] = true;
                    var next = new Vector2Int(nr, nc);
                    from[next] = cur;
                    q.Enqueue(next);
                }
            }
        }

        if (!found) return null;

        var path = new List<Vector2Int>();
        var curNode = target;
        path.Add(curNode);
        while (!curNode.Equals(start))
        {
            curNode = from[curNode];
            path.Add(curNode);
        }
        path.Reverse();
        return path;
    }

    private IEnumerator MoveAlongPath(List<Vector2Int> path)
    {
        foreach (var cell in path)
        {
            Vector3 targetPos = levelManager.GetCellWorldPosition(cell.x, cell.y);
            while (Vector3.Distance(transform.localPosition, targetPos) > 0.01f)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, moveSpeed * Time.deltaTime);
                yield return null;
            }
            yield return new WaitForSeconds(0.05f);
        }

        Debug.Log("Player reached destination.");
        Game_Event.PlayerReachedDestination?.Invoke();
    }

    public void Initialize(LevelManager lm)
    {
        levelManager = lm;
    }

    public void ResetToStart()
    {
        StopAllCoroutines();
        gameObject.SetActive(true);
        transform.SetAsLastSibling();
        if (levelManager != null)
            StartCoroutine(PlaceAtStartWhenReady());
    }
}
