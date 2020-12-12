using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

public class BGFlockingEditor : EditorWindow
{
    BGMain _bGMainWindow;
    int _index;

    public BGMain BGMainWindow { set => _bGMainWindow = value; }
    public int Index { get => _index; set => _index = value; }

    private void OnGUI()
    {
        if (_bGMainWindow != null)
        {
            var ts = _bGMainWindow.TeamSettings[_index];
            EditorGUILayout.LabelField("Minion Settings");
            ts.minionsQuantity = EditorGUILayout.IntField("Minions Quantity", ts.minionsQuantity);
            ts.minionHealth = EditorGUILayout.FloatField("Minion Health", ts.minionHealth);
            ts.minionSpeed = EditorGUILayout.FloatField("Minion Speed", ts.minionSpeed);
            ts.minionMeleeDamage = EditorGUILayout.FloatField("Minion Melee Damage", ts.minionMeleeDamage);
            ts.minionMeleeRate = EditorGUILayout.FloatField("Minion Melee Rate", ts.minionMeleeRate);
            ts.minionMeleeDistance = EditorGUILayout.FloatField("Minion Melee Distance", ts.minionMeleeDistance);
            ts.minionShootDamage = EditorGUILayout.FloatField("Minion Shoot Damage", ts.minionShootDamage);
            ts.minionShootRate = EditorGUILayout.FloatField("Minion Shoot Rate", ts.minionShootRate);
            ts.minionShootDistance = EditorGUILayout.FloatField("Minion Shoot Distance", ts.minionShootDistance);
            ts.minionVisionDistance = EditorGUILayout.FloatField("Minion Vision Distance", ts.minionVisionDistance);
            ts.minionVisionRangeAngles = EditorGUILayout.FloatField("Minion Vision Range Angles", ts.minionVisionRangeAngles);

            EditorGUILayout.LabelField("Flocking Settings");
            ts.flockEntityRadius = EditorGUILayout.FloatField("Flock Entity Radius", ts.flockEntityRadius);

            LayerMask tempMask = EditorGUILayout.MaskField("Flock Entity Mask", InternalEditorUtility.LayerMaskToConcatenatedLayersMask(ts.flockEntityMask), InternalEditorUtility.layers);
            ts.flockEntityMask = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask);

            ts.flockLeaderBehaviourWeight = EditorGUILayout.FloatField("Flock LeaderBehaviourWeight", ts.flockLeaderBehaviourWeight);
            ts.flockLeaderBehaviourTarget = (Transform)EditorGUILayout.ObjectField("Leader Target", ts.flockLeaderBehaviourTarget, typeof(Transform), true);
            ts.flockLeaderBehaviourMinDistance = EditorGUILayout.FloatField("Flock Leader Behaviour Min Distance", ts.flockLeaderBehaviourMinDistance);
            ts.flockAlineationBehaviourWeight = EditorGUILayout.FloatField("Flock Alineation Behaviour Weight", ts.flockAlineationBehaviourWeight);
            ts.flockSeparationBehaviourWeight = EditorGUILayout.FloatField("Flock Separation Behaviour Weight", ts.flockSeparationBehaviourWeight);
            ts.flockSeparationBehaviourRange = EditorGUILayout.FloatField("Flock Separation Behaviour Range", ts.flockSeparationBehaviourRange);
            ts.flockCohesionBehaviourWeight = EditorGUILayout.FloatField("Flock Cohesion Behaviour Weight", ts.flockCohesionBehaviourWeight);
            ts.flockAvoidanceBehaviourWeight = EditorGUILayout.FloatField("Flock Avoidance Behaviour Weight", ts.flockAvoidanceBehaviourWeight);

            LayerMask tempMask2 = EditorGUILayout.MaskField("Flock Avoidance Behaviour Mask", InternalEditorUtility.LayerMaskToConcatenatedLayersMask(ts.flockAvoidanceBehaviourMask), InternalEditorUtility.layers);
            ts.flockAvoidanceBehaviourMask = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask2);

            ts.flockAvoidanceBehaviourRange = EditorGUILayout.FloatField("Flock Avoidance Behaviour Range", ts.flockAvoidanceBehaviourRange);

            _bGMainWindow.TeamSettings[_index] = ts;
        }
    }
}
