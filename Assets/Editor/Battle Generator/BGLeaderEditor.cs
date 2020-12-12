using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BGLeaderEditor : EditorWindow
{
    BGMain _bGMainWindow;
    int _index;
    public BGMain BGMainWindow { set => _bGMainWindow = value; }
    public int Index { get => _index; set => _index = value; }

    
    private void OnGUI()
    {
        if(_bGMainWindow != null)
        {
            var ts = _bGMainWindow.TeamSettings[_index];

            EditorGUILayout.LabelField("Leader");
            ts.leaderHealth = EditorGUILayout.FloatField("Leader Health", ts.leaderHealth);
            ts.leaderSpeed = EditorGUILayout.FloatField("Leader Speed", ts.leaderSpeed);
            ts.leaderMeleeDamage = EditorGUILayout.FloatField("Leader Melee Damage", ts.leaderMeleeDamage);
            ts.leaderMeleeRate = EditorGUILayout.FloatField("Leader Melee Rate", ts.leaderMeleeRate);
            ts.leaderMeleeDistance = EditorGUILayout.FloatField("Leader Melee Distance", ts.leaderMeleeDistance);
            ts.leaderShootDamage = EditorGUILayout.FloatField("Leader Shoot Damage", ts.leaderShootDamage);
            ts.leaderShootRate = EditorGUILayout.FloatField("Leader Shoot Rate", ts.leaderShootRate);
            ts.leaderShootDistance = EditorGUILayout.FloatField("Leader Shoot Distance", ts.leaderShootDistance);
            ts.leaderVisionDistance = EditorGUILayout.FloatField("Leader Vision Distance", ts.leaderVisionDistance);
            ts.leaderVisionRangeAngles = EditorGUILayout.FloatField("Leader Vision RangeAngles", ts.leaderVisionRangeAngles);

            _bGMainWindow.TeamSettings[_index] = ts;
        }
    }
}
