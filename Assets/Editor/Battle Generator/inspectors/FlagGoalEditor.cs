using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FlagGoal))]

public class FlagGoalEditor : Editor
{
    FlagGoal _flagGoal;

    SerializedProperty m_addingValue;
    SerializedProperty m_addingInterval;
    SerializedProperty m_entititesTeam1;
    SerializedProperty m_entititesTeam2;
    SerializedObject so;

    SphereCollider _collider;

    float _dominanceValue;

    Color team1Color = new Color(0, 0.82f, 0.93f);
    Color team2Color = new Color(0.93f, 0.7f, 0);
    private Color _normalcolor;
    private float _lerpedVal;

    private void OnEnable()
    {
        _flagGoal = (FlagGoal)target;

        so = new SerializedObject(_flagGoal);
        m_addingValue = so.FindProperty("addingValue");
        m_addingInterval = so.FindProperty("addingInterval");
        m_entititesTeam1 = so.FindProperty("entitiesTeam1");
        m_entititesTeam2 = so.FindProperty("entitiesTeam2");

        _collider = _flagGoal.GetComponent<SphereCollider>();

        _normalcolor = GUI.color;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(m_addingValue, false);
        EditorGUILayout.PropertyField(m_addingInterval, false);
        _collider.radius = EditorGUILayout.FloatField("Radius", _collider.radius);

        EditorGUILayout.MinMaxSlider(new GUIContent("Min y Max"), ref _flagGoal.leftValue, ref _flagGoal.rightValue, -200, 200);
        _flagGoal.dominanceValue = EditorGUILayout.Slider("Dominance", _flagGoal.dominanceValue, _flagGoal.leftValue, _flagGoal.rightValue);
        EditorGUILayout.BeginHorizontal();
        _flagGoal.leftValue = EditorGUILayout.FloatField(_flagGoal.leftValue);
        _flagGoal.dominanceValue = EditorGUILayout.FloatField(_flagGoal.dominanceValue);
        _flagGoal.rightValue = EditorGUILayout.FloatField(_flagGoal.rightValue);
        EditorGUILayout.EndHorizontal();

        ProgressBar(_flagGoal.dominanceValue, _flagGoal.leftValue, _flagGoal.rightValue);

    }

    private void ProgressBar(float val, float min, float max)
    {
        _lerpedVal = (val - min) / (max - min);
        GUI.color = Color.Lerp(team2Color, team1Color, _lerpedVal);
        var rect = EditorGUILayout.GetControlRect(false, 20);
        EditorGUI.BeginChangeCheck();
        EditorGUI.ProgressBar(rect, _lerpedVal, "Dominance");
        GUI.color = _normalcolor;
        if (EditorGUI.EndChangeCheck())
        {
            SceneView.RepaintAll();
        }
    }

    private void OnSceneGUI()
    {
        Handles.BeginGUI();

        var cmraPoint = Camera.current.WorldToScreenPoint(_flagGoal.transform.position);
        var cmraRectHeight = Camera.current.pixelHeight;
        var rect = new Rect(cmraPoint.x - 75, cmraRectHeight - cmraPoint.y, 200, 255);

        GUILayout.BeginArea(rect);
        var rec = EditorGUILayout.BeginVertical();
        GUI.Box(rec, GUIContent.none);
        var progressRect = new Rect(rec);
        progressRect.x += 5;
        progressRect.y += 5;
        progressRect.height = 20;
        progressRect.width -= 10;

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUI.color = Color.Lerp(team2Color, team1Color, _lerpedVal);
        EditorGUI.ProgressBar(progressRect, _lerpedVal, "Dominance");
        GUI.color = _normalcolor;
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        Rect lineRect = EditorGUILayout.GetControlRect(false, 25);
        EditorGUI.DrawRect(lineRect, new Color(0, 0, 0, 0));

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        _flagGoal.leftValue = EditorGUILayout.FloatField(_flagGoal.leftValue);
        _flagGoal.dominanceValue = EditorGUILayout.FloatField(_flagGoal.dominanceValue);
        _flagGoal.rightValue = EditorGUILayout.FloatField(_flagGoal.rightValue);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        _collider.radius = EditorGUILayout.FloatField("Radius", _collider.radius);

        lineRect = EditorGUILayout.GetControlRect(false, 1);
        EditorGUI.DrawRect(lineRect, Color.grey);

        EditorGUILayout.EndVertical();
        GUILayout.EndArea();
        Handles.EndGUI();
    }
}
