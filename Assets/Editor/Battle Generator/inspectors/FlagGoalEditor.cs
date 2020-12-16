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

    float _dominanceValue;

    Color team1Color = new Color(0, 0.82f, 0.93f);
    Color team2Color = new Color(0.93f, 0.7f, 0);

    private void OnEnable()
    {
        _flagGoal = (FlagGoal)target;

        m_addingValue = serializedObject.FindProperty("addingValue");
        m_addingInterval = serializedObject.FindProperty("addingInterval");
        m_entititesTeam1 = serializedObject.FindProperty("entitiesTeam1");
        m_entititesTeam2 = serializedObject.FindProperty("entitiesTeam2");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(m_addingValue, false);
        EditorGUILayout.PropertyField(m_addingInterval, false);

        EditorGUILayout.MinMaxSlider(ref _flagGoal.leftValue, ref _flagGoal.rightValue, -200, 200);
        _flagGoal.dominanceValue = EditorGUILayout.Slider(_flagGoal.dominanceValue, _flagGoal.leftValue, _flagGoal.rightValue);
        EditorGUILayout.BeginHorizontal();
        _flagGoal.leftValue = EditorGUILayout.FloatField(_flagGoal.leftValue);
        _flagGoal.dominanceValue = EditorGUILayout.FloatField(_flagGoal.dominanceValue);
        _flagGoal.rightValue = EditorGUILayout.FloatField(_flagGoal.rightValue);
        EditorGUILayout.EndHorizontal();

        ProgressBar(_flagGoal.dominanceValue, _flagGoal.leftValue, _flagGoal.rightValue);

        EditorGUILayout.PropertyField(m_entititesTeam1, true);
        EditorGUILayout.PropertyField(m_entititesTeam2, true);
    }

    private void ProgressBar(float val, float min, float max)
    {

        float lerpedVal = (val - min) / (max - min);
        GUI.color = Color.Lerp(team2Color, team1Color, lerpedVal);
        var rect = EditorGUILayout.GetControlRect(false, 20);
        EditorGUI.ProgressBar(rect, lerpedVal, "Dominance");
    }
}
