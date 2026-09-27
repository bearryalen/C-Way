using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(GridLevel), false)]
[CanEditMultipleObjects]
[System.Serializable]
public class LevelDrawer : Editor
{
    private GridLevel levelDataInstance => target as GridLevel;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        ClearBoardButton();
        EditorGUILayout.Space();

        DrawColumnsInputFields();
        EditorGUILayout.Space();

        levelDataInstance.startPoint = DrawPositivePoint("Start Point", levelDataInstance.startPoint);
        levelDataInstance.endPoint = DrawPositivePoint("End Point", levelDataInstance.endPoint);
        EditorGUILayout.Space();

        DrawObstaclePoints();
        EditorGUILayout.Space();

        if (levelDataInstance.board != null && levelDataInstance.columns > 0 && levelDataInstance.rows > 0)
        {
            DrawBoardTable();
            
        }

        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(levelDataInstance);
        }

        /* inside OnInspectorGUI() after existing UI (for example after DrawBoardTable())
        if (GUILayout.Button("Complete Path Between Start/End"))
        {
            levelDataInstance.CompletePathBetweenStartAndEnd(clearBefore: true);
            EditorUtility.SetDirty(levelDataInstance);
        }*/
    }

    private void ClearBoardButton()
    {
        if (GUILayout.Button("Clear Board"))
        {
            levelDataInstance.Clear();
        }
    }

    private Vector2 DrawPositivePoint(string label, Vector2 point)
    {
        point = EditorGUILayout.Vector2Field(label, point);
        point.x = Mathf.Clamp(Mathf.Round(point.x), 0, Mathf.Max(0, levelDataInstance.columns - 1));
        point.y = Mathf.Clamp(Mathf.Round(point.y), 0, Mathf.Max(0, levelDataInstance.rows - 1));
        return point;
    }

    private void DrawColumnsInputFields()
    {
        var columnsTemp = levelDataInstance.columns;
        var rowsTemp = levelDataInstance.rows;

        levelDataInstance.columns = EditorGUILayout.IntField("Columns", levelDataInstance.columns);
        levelDataInstance.rows = EditorGUILayout.IntField("Rows", levelDataInstance.rows);

        if ((levelDataInstance.columns != columnsTemp || levelDataInstance.rows != rowsTemp) && levelDataInstance.columns > 0 && levelDataInstance.rows > 0)
        {
            levelDataInstance.CreateNewBoard();
        }
    }

    private void DrawBoardTable()
    {
        var tableStyle = new GUIStyle("box");
        tableStyle.padding = new RectOffset(10, 10, 10, 10);
        tableStyle.margin.left = 32;

        var headerColumnStyle = new GUIStyle();
        headerColumnStyle.fixedWidth = 65;
        headerColumnStyle.alignment = TextAnchor.MiddleCenter;

        var rowStyle = new GUIStyle();
        rowStyle.fixedHeight = 25;
        rowStyle.alignment = TextAnchor.MiddleCenter;

        var dataFieldStyle = new GUIStyle(EditorStyles.miniButtonMid);
        dataFieldStyle.normal.background = Texture2D.grayTexture;
        dataFieldStyle.onNormal.background = Texture2D.whiteTexture;

        // check if the board is null or has no rows or columns
        for (var row = 0; row < levelDataInstance.rows; row++)
        {
            EditorGUILayout.BeginHorizontal(headerColumnStyle);

            for (var column = 0; column < levelDataInstance.columns; column++)
            {
                EditorGUILayout.BeginHorizontal(rowStyle);
                var data = EditorGUILayout.Toggle(levelDataInstance.board[row].columns[column], dataFieldStyle);
                levelDataInstance.board[row].columns[column] = data;
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndHorizontal();
        }
    }

    private void DrawObstaclePoints()
    {
        EditorGUILayout.LabelField("Obstacle Points", EditorStyles.boldLabel);

        if (levelDataInstance.obstaclePoints == null)
        {
            levelDataInstance.obstaclePoints = new List<Vector2Int>();
        }

        EditorGUILayout.HelpBox("Coordinates use X = row and Y = column.", MessageType.Info);

        for (int i = 0; i < levelDataInstance.obstaclePoints.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            var point = levelDataInstance.obstaclePoints[i];
            point.x = EditorGUILayout.IntField("X", point.x);
            point.y = EditorGUILayout.IntField("Y", point.y);
            levelDataInstance.obstaclePoints[i] = point;

            if (GUILayout.Button("-", GUILayout.Width(25)))
            {
                levelDataInstance.obstaclePoints.RemoveAt(i);
                i--;
            }

            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add Obstacle Point"))
        {
            levelDataInstance.obstaclePoints.Add(Vector2Int.zero);
        }

        if (GUILayout.Button("Clear Obstacle Points"))
        {
            levelDataInstance.obstaclePoints.Clear();
        }
    }

}
