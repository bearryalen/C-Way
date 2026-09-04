using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(PathData), false)]
[CanEditMultipleObjects]
[System.Serializable]
public class PathDrawer : Editor
{
    private PathData pathDataInstance => target as PathData;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        ClearBoardButton();
        EditorGUILayout.Space();

        DrawColumnsInputFields();
        EditorGUILayout.Space();

        if (pathDataInstance.board != null && pathDataInstance.columns > 0 && pathDataInstance.rows > 0)
        {
            DrawBoardTable();
        }

        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(pathDataInstance);
        }
    }

    private void ClearBoardButton() 
    {
        if (GUILayout.Button("Clear Board"))
        {
            pathDataInstance.Clear();
        }
    }

    private void DrawColumnsInputFields()
    {
        var columnsTemp = pathDataInstance.columns;
        var rowsTemp = pathDataInstance.rows;

        pathDataInstance.columns = EditorGUILayout.IntField("Columns", pathDataInstance.columns);
        pathDataInstance.rows = EditorGUILayout.IntField("Rows", pathDataInstance.rows);

        if ((pathDataInstance.columns != columnsTemp || pathDataInstance.rows != rowsTemp) && pathDataInstance.columns > 0 && pathDataInstance.rows > 0)
        {
            pathDataInstance.CreateNewBoard();
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
        for (var row = 0; row < pathDataInstance.rows; row++)
        {
            EditorGUILayout.BeginHorizontal(headerColumnStyle);

            for(var column = 0; column < pathDataInstance.columns; column++)
            {
                EditorGUILayout.BeginHorizontal(rowStyle);
                var data = EditorGUILayout.Toggle(pathDataInstance.board[row].columns[column], dataFieldStyle);
                pathDataInstance.board[row].columns[column] = data;
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndHorizontal();
        }
    }
    
}
