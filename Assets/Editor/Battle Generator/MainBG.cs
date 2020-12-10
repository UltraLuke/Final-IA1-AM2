using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class MainBG : EditorWindow
{
    GUIStyle _title;
    GUIStyle _header;

    bool[] _pressedTeams = new bool[2];
    TeamSettings[] _teamSettings = new TeamSettings[2];

    [MenuItem("Tools/Battle Generator")]
    public static void OpenWindow()
    {
        var window = GetWindow<MainBG>();
        window.Show();
    }

    private void OnEnable()
    {
        _title = new GUIStyle
        {
            fontStyle = FontStyle.BoldAndItalic,
            alignment = TextAnchor.MiddleCenter,
            fontSize = 15,
        };
        _header = new GUIStyle
        {
            fontStyle = FontStyle.Bold,
        };
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("BATTLE GENERATOR", _title);
        var normalGUIColor = GUI.color;
        EditorGUILayout.BeginHorizontal();
        GUI.color = new Color(0, 0.82f, 0.93f);

        if (_pressedTeams[0] = GUILayout.Toggle(_pressedTeams[0], "Team 1", "Button"))
        {
            _pressedTeams[1] = false;
        }
        GUI.color = new Color(0.92f, 0.69f, 0f);
        if (_pressedTeams[1] = GUILayout.Toggle(_pressedTeams[1], "Team 2", "Button"))
        {
            _pressedTeams[0] = false;
        }
        EditorGUILayout.EndHorizontal();

        GUI.color = normalGUIColor;

        var rect = EditorGUILayout.GetControlRect(false, 1);
        EditorGUI.DrawRect(rect, Color.gray);

        if (_pressedTeams[0])
            ShowTeamSettings(0);
        else if (_pressedTeams[1])
            ShowTeamSettings(1);

        rect = EditorGUILayout.GetControlRect(false, 1);
        EditorGUI.DrawRect(rect, Color.gray);

        EditorGUILayout.LabelField("Pathinding", _header);
        if(GUILayout.Button("Pathfinding Settings"))
        {
            var pathFindingEditor = GetWindow<PathFindingEditor>();
            pathFindingEditor.Show();
        }
    }

    private void ShowTeamSettings(int v)
    {
        EditorGUILayout.LabelField("Team Settings", _header);
        _teamSettings[v].leaderHealth = EditorGUILayout.FloatField("Leader Health", _teamSettings[v].leaderHealth);
        _teamSettings[v].leaderSpeed = EditorGUILayout.FloatField("Leader Speed", _teamSettings[v].leaderSpeed);
        _teamSettings[v].leaderMeleeDamage = EditorGUILayout.FloatField("Leader Melee Damage", _teamSettings[v].leaderMeleeDamage);
        _teamSettings[v].leaderMeleeRate = EditorGUILayout.FloatField("Leader Melee Rate", _teamSettings[v].leaderMeleeRate);
        _teamSettings[v].leaderMeleeDistance = EditorGUILayout.FloatField("Leader Melee Distance", _teamSettings[v].leaderMeleeDistance);
        _teamSettings[v].leaderShootDamage = EditorGUILayout.FloatField("Leader Shoot Damage", _teamSettings[v].leaderShootDamage);
        _teamSettings[v].leaderShootRate = EditorGUILayout.FloatField("Leader Shoot Rate", _teamSettings[v].leaderShootRate);
        _teamSettings[v].leaderShootDistance = EditorGUILayout.FloatField("Leader Shoot Distance", _teamSettings[v].leaderShootDistance);
        _teamSettings[v].leaderVisionDistance = EditorGUILayout.FloatField("Leader Vision Distance", _teamSettings[v].leaderVisionDistance);
        _teamSettings[v].leaderVisionRangeAngles = EditorGUILayout.FloatField("Leader Vision RangeAngles", _teamSettings[v].leaderVisionRangeAngles);

        EditorGUILayout.Space();

        _teamSettings[v].flockQuantity = EditorGUILayout.IntField("Flock quantity", _teamSettings[v].flockQuantity);
        _teamSettings[v].flockingHealth = EditorGUILayout.FloatField("Flock Health", _teamSettings[v].flockingHealth);
        _teamSettings[v].flockingSpeed = EditorGUILayout.FloatField("Flock Speed", _teamSettings[v].flockingSpeed);
        _teamSettings[v].flockingMeleeDamage = EditorGUILayout.FloatField("Flock Melee Damage", _teamSettings[v].flockingMeleeDamage);
        _teamSettings[v].flockingMeleeRate = EditorGUILayout.FloatField("Flock Melee Rate", _teamSettings[v].flockingMeleeRate);
        _teamSettings[v].flockingMeleeDistance = EditorGUILayout.FloatField("Flock Melee Distance", _teamSettings[v].flockingMeleeDistance);
        _teamSettings[v].flockingShootDamage = EditorGUILayout.FloatField("Flock Shoot Damage", _teamSettings[v].flockingShootDamage);
        _teamSettings[v].flockingShootRate = EditorGUILayout.FloatField("Flock Shoot Rate", _teamSettings[v].flockingShootRate);
        _teamSettings[v].flockingShootDistance = EditorGUILayout.FloatField("Flock Shoot Distance", _teamSettings[v].flockingShootDistance);
        _teamSettings[v].flockingVisionDistance = EditorGUILayout.FloatField("Flock Vision Distance", _teamSettings[v].flockingVisionDistance);
        _teamSettings[v].flockingVisionRangeAngles = EditorGUILayout.FloatField("Flock Vision RangeAngles", _teamSettings[v].flockingVisionRangeAngles);
    }
}
