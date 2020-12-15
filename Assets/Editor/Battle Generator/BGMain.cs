using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class BGMain : EditorWindow
{
    GUIStyle _title;
    GUIStyle _headerlv1;
    GUIStyle _headerlv2;

    string _saveFolderPath = "Assets/battle_generator_saves";
    //string _savePresetName;

    PathFindingEditor _pathFindingEditorWindow;
    BGLeaderEditor _bGLeaderEditorWindow;
    BGFlockingEditor _bGFlockingEditorWindow;

    int _tsIndex;
    bool[] _pressedTeams = new bool[2];
    TeamSettings[] _teamSettings = new TeamSettings[2];
    List<List<GameObject>> _minions = new List<List<GameObject>>() { null, null };
    List<GameObject> _leaders = new List<GameObject>() { null, null };

    public TeamSettings[] TeamSettings { get => _teamSettings; set => _teamSettings = value; }
    public string SaveFolderPath { get => _saveFolderPath; }
    public List<List<GameObject>> Minions { get => _minions; set => _minions = value; }
    public List<GameObject> Leader { get => _leaders; set => _leaders = value; }

    [MenuItem("Tools/Battle Generator")]
    public static void OpenWindow()
    {
        var window = GetWindow<BGMain>();
        window.Show();
    }

    private void OnEnable()
    {
        CreateDirectories();
        _tsIndex = -1;

        _title = new GUIStyle
        {
            fontStyle = FontStyle.BoldAndItalic,
            alignment = TextAnchor.MiddleCenter,
            fontSize = 15,
        };
        _headerlv1 = new GUIStyle
        {
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            fontSize = 13,
        };
        _headerlv2 = new GUIStyle
        {
            fontStyle = FontStyle.Bold,
        };

        for (int i = 0; i < _minions.Count; i++)
            _minions[i] = new List<GameObject>();

        SceneView.duringSceneGui += OnSceneGUI;
    }
    private void OnDisable()
    {
        CloseEditingSubwindows();

        if (_pathFindingEditorWindow != null)
            _pathFindingEditorWindow.Close();

        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("BATTLE GENERATOR", _title);

        var normalGUIColor = GUI.color;
        var backgroundColor = GUI.backgroundColor;
        var normalBGColor = backgroundColor;
        EditorGUILayout.BeginHorizontal();
        GUI.color = new Color(0, 0.82f, 0.93f);
        EditorGUI.BeginChangeCheck();
        if (_pressedTeams[0] = GUILayout.Toggle(_pressedTeams[0], "Team 1", "Button"))
        {
            _pressedTeams[1] = false;
            backgroundColor = new Color(0, 0.82f, 0.93f);
        }
        GUI.color = new Color(0.92f, 0.69f, 0f);
        if (_pressedTeams[1] = GUILayout.Toggle(_pressedTeams[1], "Team 2", "Button"))
        {
            _pressedTeams[0] = false;
            backgroundColor = new Color(0.92f, 0.69f, 0f);
        }
        if (EditorGUI.EndChangeCheck())
        {
            CloseEditingSubwindows();
        }
        GUI.color = normalGUIColor;
        GUI.backgroundColor = backgroundColor;
        _tsIndex = -1;
        for (int i = 0; i < _pressedTeams.Length; i++)
        {
            if (_pressedTeams[i])
            {
                _tsIndex = i;
                break;
            }
        }
        EditorGUILayout.EndHorizontal();


        var rect = EditorGUILayout.GetControlRect(false, 1);
        if (_tsIndex >= 0)
        {
            EditorGUI.DrawRect(rect, Color.gray);
            EditorGUILayout.LabelField("Team Settings", _headerlv1);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("Leader", "Opciones relacionadas al Leader del equipo")))
            {
                if(_bGLeaderEditorWindow == null)
                    _bGLeaderEditorWindow = GetWindow<BGLeaderEditor>();

                _bGLeaderEditorWindow.BGMainWindow = this;
                _bGLeaderEditorWindow.Index = _tsIndex;
                _bGLeaderEditorWindow.Show();
            }
            EditorGUI.BeginDisabledGroup(_teamSettings[_tsIndex].leaderEntity == null);
            if(GUILayout.Button(new GUIContent("Flocking", "Opciones relacionadas a los minions del equipo.\nPara acceder debe haber un GameObject Leader asignado.")))
            {
                if (_bGFlockingEditorWindow == null)
                    _bGFlockingEditorWindow = GetWindow<BGFlockingEditor>();

                _bGFlockingEditorWindow.BGMainWindow = this;
                _bGFlockingEditorWindow.Index = _tsIndex;
                _bGFlockingEditorWindow.Show();
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
        }

        rect = EditorGUILayout.GetControlRect(false, 1);
        EditorGUI.DrawRect(rect, Color.gray);

        GUI.backgroundColor = normalBGColor;

        EditorGUILayout.LabelField("Pathfinding", _headerlv1);
        if (GUILayout.Button(new GUIContent("Pathfinding Settings", "Herramienta de edicion de pathfinding")))
        {
            _pathFindingEditorWindow = GetWindow<PathFindingEditor>();
            _pathFindingEditorWindow.wantsMouseMove = true;
            _pathFindingEditorWindow.Show();
        }
    }

    private void CreateDirectories()
    {
        if (!AssetDatabase.IsValidFolder(_saveFolderPath))
        {
            var splittedPath = _saveFolderPath.Split('/');
            AssetDatabase.CreateFolder(splittedPath[0], splittedPath[1]);
        }
    }

    private void CloseEditingSubwindows()
    {
        if (_bGLeaderEditorWindow != null)
            _bGLeaderEditorWindow.Close();

        if (_bGFlockingEditorWindow != null)
            _bGFlockingEditorWindow.Close();
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
    }
}
