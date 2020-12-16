using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(BGContainerSave))]
public class BGContainerSaveEditor : Editor
{
    BGContainerSave _bGContainerSave;

    List<GameObject> _leaders;
    List<List<MinionData>> _minions;
    List<TeamSettings> _ts;

    GUIStyle _title;
    GUIStyle _headerlv1;
    GUIStyle _headerlv2;

    private void OnEnable()
    {
        _bGContainerSave = (BGContainerSave)target;

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
    }
    public override void OnInspectorGUI()
    {
        _leaders = _bGContainerSave.leaders;
        _minions = _bGContainerSave.minions;
        _ts = _bGContainerSave.ts;

        EditorGUILayout.LabelField("Leaders", _headerlv1);
        for (int i = 0; i < _leaders.Count; i++)
        {
            TeamSettings ts = _ts[i];
            EditorGUILayout.ObjectField("Leader " + (i + 1), _leaders[i], typeof(GameObject), false);
            ts.leaderPosition = EditorGUILayout.Vector3Field("Position", ts.leaderPosition);
            ts.leaderHealth = EditorGUILayout.FloatField("Health", ts.leaderHealth);
            ts.leaderSpeed = EditorGUILayout.FloatField("Speed", ts.leaderSpeed);
            ts.leaderMeleeDamage = EditorGUILayout.FloatField("Melee Damage", ts.leaderMeleeDamage);
            ts.leaderMeleeRate = EditorGUILayout.FloatField("Melee Rate", ts.leaderMeleeRate);
            ts.leaderMeleeDistance = EditorGUILayout.FloatField("Melee Distance", ts.leaderMeleeDistance);
            ts.leaderShootDamage = EditorGUILayout.FloatField("Shoot Damage", ts.leaderShootDamage);
            ts.leaderShootRate = EditorGUILayout.FloatField("Shoot Rate", ts.leaderShootRate);
            ts.leaderShootDistance = EditorGUILayout.FloatField("Shoot Distance", ts.leaderShootDistance);
            ts.leaderVisionDistance = EditorGUILayout.FloatField("Vision Distance", ts.leaderVisionDistance);
            ts.leaderVisionRangeAngles = EditorGUILayout.FloatField("Vision Angles", ts.leaderVisionRangeAngles);
            EditorGUILayout.Space();
        }
        EditorGUILayout.Space();
        var rect = EditorGUILayout.GetControlRect(true, 1);
        EditorGUI.DrawRect(rect, Color.gray);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Minions", _headerlv1);
        for (int i = 0; i < _minions.Count; i++)
        {
            TeamSettings ts = _ts[i];
            EditorGUILayout.LabelField("Minion group " + (i + 1), _headerlv2);
            //Debug.Log(_minions[i].Count);
            for (int j = 0; j < _minions[i].Count; j++)
            {
                EditorGUILayout.ObjectField("Minion " + (j + 1), _minions[i][j].minion, typeof(GameObject), false);
                EditorGUILayout.Vector3Field("Position", _minions[i][j].position);
                //VER TEMA POSICION
            }

            EditorGUILayout.Space();

            ts.minionHealth = EditorGUILayout.FloatField("Health", ts.minionHealth);
            ts.minionSpeed = EditorGUILayout.FloatField("Speed", ts.minionSpeed);
            ts.minionMeleeDamage = EditorGUILayout.FloatField("Melee Damage", ts.minionMeleeDamage);
            ts.minionMeleeRate = EditorGUILayout.FloatField("Melee Rate", ts.minionMeleeRate);
            ts.minionMeleeDistance = EditorGUILayout.FloatField("Melee Distance", ts.minionMeleeDistance);
            ts.minionShootDamage = EditorGUILayout.FloatField("Shoot Damage", ts.minionShootDamage);
            ts.minionShootRate = EditorGUILayout.FloatField("Shoot Rate", ts.minionShootRate);
            ts.minionVisionDistance = EditorGUILayout.FloatField("Vision Distance", ts.minionVisionDistance);
            ts.minionVisionRangeAngles = EditorGUILayout.FloatField("Vision Angle", ts.minionVisionRangeAngles);
            
            EditorGUILayout.Space();

            ts.flockEntityRadius = EditorGUILayout.FloatField("Flocking radius", ts.flockEntityRadius);
            LayerMask tempMask = EditorGUILayout.MaskField("Flock Entity Mask", InternalEditorUtility.LayerMaskToConcatenatedLayersMask(ts.flockEntityMask), InternalEditorUtility.layers);
            ts.flockEntityMask = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask);
            ts.flockLeaderBehaviourWeight = EditorGUILayout.FloatField("Leader Weight", ts.flockLeaderBehaviourWeight);
            ts.flockLeaderBehaviourMinDistance = EditorGUILayout.FloatField("Leader Min Distance", ts.flockLeaderBehaviourMinDistance);
            ts.flockAlineationBehaviourWeight = EditorGUILayout.FloatField("Alineation Weight", ts.flockAlineationBehaviourWeight);
            ts.flockSeparationBehaviourWeight = EditorGUILayout.FloatField("Separation Weight", ts.flockSeparationBehaviourWeight);
            ts.flockSeparationBehaviourRange = EditorGUILayout.FloatField("Separation Range", ts.flockSeparationBehaviourRange);
            ts.flockCohesionBehaviourWeight = EditorGUILayout.FloatField("Cohesion Weight", ts.flockCohesionBehaviourWeight);
            ts.flockAvoidanceBehaviourWeight = EditorGUILayout.FloatField("Avoidance Weight", ts.flockAvoidanceBehaviourWeight);
            tempMask = EditorGUILayout.MaskField("Avoidance Mask", InternalEditorUtility.LayerMaskToConcatenatedLayersMask(ts.flockAvoidanceBehaviourMask), InternalEditorUtility.layers);
            ts.flockAvoidanceBehaviourMask = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask);
            ts.flockAvoidanceBehaviourRange = EditorGUILayout.FloatField("Avoidance Range", ts.flockAvoidanceBehaviourRange);

            _ts[i] = ts;

            EditorGUILayout.Space();
        }
        rect = EditorGUILayout.GetControlRect(true, 1);
        EditorGUI.DrawRect(rect, Color.gray);

        _bGContainerSave.leaders = _leaders;
        _bGContainerSave.minions = _minions;
        _bGContainerSave.ts = _ts;
    }
}
