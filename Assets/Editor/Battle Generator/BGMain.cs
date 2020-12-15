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

    public TeamSettings[] TeamSettings { get => _teamSettings; set => _teamSettings = value; }
    public string SaveFolderPath { get => _saveFolderPath; }

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
    //private void ShowTeamSettings(int v)
    //{
    //    EditorGUILayout.LabelField("Team Settings", _headerlv1);
    //    EditorGUILayout.LabelField("Leader", _headerlv2);
    //    _teamSettings[v].leaderHealth = EditorGUILayout.FloatField("Leader Health", _teamSettings[v].leaderHealth);
    //    _teamSettings[v].leaderSpeed = EditorGUILayout.FloatField("Leader Speed", _teamSettings[v].leaderSpeed);
    //    _teamSettings[v].leaderMeleeDamage = EditorGUILayout.FloatField("Leader Melee Damage", _teamSettings[v].leaderMeleeDamage);
    //    _teamSettings[v].leaderMeleeRate = EditorGUILayout.FloatField("Leader Melee Rate", _teamSettings[v].leaderMeleeRate);
    //    _teamSettings[v].leaderMeleeDistance = EditorGUILayout.FloatField("Leader Melee Distance", _teamSettings[v].leaderMeleeDistance);
    //    _teamSettings[v].leaderShootDamage = EditorGUILayout.FloatField("Leader Shoot Damage", _teamSettings[v].leaderShootDamage);
    //    _teamSettings[v].leaderShootRate = EditorGUILayout.FloatField("Leader Shoot Rate", _teamSettings[v].leaderShootRate);
    //    _teamSettings[v].leaderShootDistance = EditorGUILayout.FloatField("Leader Shoot Distance", _teamSettings[v].leaderShootDistance);
    //    _teamSettings[v].leaderVisionDistance = EditorGUILayout.FloatField("Leader Vision Distance", _teamSettings[v].leaderVisionDistance);
    //    _teamSettings[v].leaderVisionRangeAngles = EditorGUILayout.FloatField("Leader Vision RangeAngles", _teamSettings[v].leaderVisionRangeAngles);

    //    EditorGUILayout.Space();

    //    EditorGUILayout.LabelField("Minions", _headerlv2);
    //    _teamSettings[v].minionsQuantity = EditorGUILayout.IntField("Minions quantity", _teamSettings[v].minionsQuantity);
    //    _teamSettings[v].minionHealth = EditorGUILayout.FloatField("Minion Health", _teamSettings[v].minionHealth);
    //    _teamSettings[v].minionSpeed = EditorGUILayout.FloatField("Minion Speed", _teamSettings[v].minionSpeed);
    //    _teamSettings[v].minionMeleeDamage = EditorGUILayout.FloatField("Minion Melee Damage", _teamSettings[v].minionMeleeDamage);
    //    _teamSettings[v].minionMeleeRate = EditorGUILayout.FloatField("Minion Melee Rate", _teamSettings[v].minionMeleeRate);
    //    _teamSettings[v].minionMeleeDistance = EditorGUILayout.FloatField("Minion Melee Distance", _teamSettings[v].minionMeleeDistance);
    //    _teamSettings[v].minionShootDamage = EditorGUILayout.FloatField("Minion Shoot Damage", _teamSettings[v].minionShootDamage);
    //    _teamSettings[v].minionShootRate = EditorGUILayout.FloatField("Minion Shoot Rate", _teamSettings[v].minionShootRate);
    //    _teamSettings[v].minionShootDistance = EditorGUILayout.FloatField("Minion Shoot Distance", _teamSettings[v].minionShootDistance);
    //    _teamSettings[v].minionVisionDistance = EditorGUILayout.FloatField("Minion Vision Distance", _teamSettings[v].minionVisionDistance);
    //    _teamSettings[v].minionVisionRangeAngles = EditorGUILayout.FloatField("Minion Vision Range Angles", _teamSettings[v].minionVisionRangeAngles);
    //}
}
