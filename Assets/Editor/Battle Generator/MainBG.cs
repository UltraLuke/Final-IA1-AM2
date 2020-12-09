using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MainBG : EditorWindow
{
    GUIStyle _header;

    [MenuItem("Tools/Battle Generator")]
    public static void OpenWindow()
    {
        var window = GetWindow<MainBG>();
        window.Show();
    }

    private void OnEnable()
    {
        _header = new GUIStyle
        {
            fontStyle = FontStyle.BoldAndItalic,
            alignment = TextAnchor.MiddleCenter,
            fontSize = 15,
        };
        
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("BATTLE GENERATOR", _header);
        var normalGUIColor = GUI.color;
        GUI.color = new Color(0, 0.82f, 0.93f);
        if (GUILayout.Button("Team 1"))
        {

        }
        GUI.color = new Color(0.92f, 0.69f, 0f);
        if (GUILayout.Button("Team 2"))
        {

        }
    }
}
