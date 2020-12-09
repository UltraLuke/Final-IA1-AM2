using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Node))]
public class NodeEditor : Editor
{
    Node _node;


    GUIStyle _header;

    private void OnEnable()
    {
        _node = (Node)target;
        _header = new GUIStyle { fontStyle = FontStyle.Bold };
    }
    private void OnSceneGUI()
    {
        Handles.BeginGUI();
        var cmraPoint = Camera.current.WorldToScreenPoint(_node.transform.position);
        var cmraRectHeight = Camera.current.pixelHeight;
        var rect = new Rect(cmraPoint.x - 75, cmraRectHeight - cmraPoint.y, 200, 255);
        //string text = _node.gameObject.name;

        GUILayout.BeginArea(rect);
        var rec = EditorGUILayout.BeginVertical();
        GUI.color = new Color32(200, 200, 200, 255);
        GUI.Box(rec, GUIContent.none);

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(_node.gameObject.name, _header);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        Rect lineRect = EditorGUILayout.GetControlRect(false, 1);
        EditorGUI.DrawRect(lineRect, Color.gray);

        GUILayout.Label("Connected nodes");
        var currColor = GUI.color;
        for (int i = 0; i < _node.Neighbours.Count; i++)
        {
            GUILayout.BeginHorizontal();
            GUI.color = currColor;
            EditorGUI.BeginChangeCheck();
            _node.Neighbours[i] = (Node)EditorGUILayout.ObjectField(_node.Neighbours[i], typeof(Node), true);
            GUI.color = Color.red;
            if (GUILayout.Button("Delete"))
            {
                _node.Neighbours.RemoveAt(i);
            }
            if (EditorGUI.EndChangeCheck())
            {
                PrefabUtility.RecordPrefabInstancePropertyModifications(_node);
            }
            GUILayout.EndHorizontal();
        }
        GUI.color = currColor;

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUI.color = Color.cyan;
        if (GUILayout.Button("Add"))
        {
            _node.Neighbours.Add(null);
            PrefabUtility.RecordPrefabInstancePropertyModifications(_node);
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUI.color = Color.white;

        EditorGUILayout.Space();

        EditorGUILayout.EndVertical();
        GUILayout.EndArea();

        Handles.EndGUI();
    }
}
